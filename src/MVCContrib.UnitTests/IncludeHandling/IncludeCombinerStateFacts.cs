using System;
using System.Collections.Generic;
using System.Web;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using Rhino.Mocks;
using Xunit;
using Xunit.Extensions;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class IncludeCombinerStateFacts
	{
		private readonly IIncludeCombiner _combiner;
		private readonly IIncludeReader _mockReader;
		private readonly MockRepository _mocks;
		private readonly IIncludeStorage _mockStorage;
		private readonly IIncludeHandlingSettings _mockSettings;
		private readonly IHttpContextProvider _mockHttp;

		public IncludeCombinerStateFacts()
		{
			_mocks = new MockRepository();
			_mockSettings = _mocks.DynamicMock<IIncludeHandlingSettings>();
			_mockReader = _mocks.DynamicMock<IIncludeReader>();
			_mockStorage = _mocks.DynamicMock<IIncludeStorage>();
			_mockHttp = _mocks.DynamicMock<IHttpContextProvider>();
			_combiner = new IncludeCombiner(_mockSettings, _mockReader, _mockStorage, _mockHttp);
			_mocks.ReplayAll();
		}

		public static IEnumerable<object[]> RenderingInDebug
		{
			get
			{
				yield return new object[]
				{
					new Dictionary<string, string> { { "/foo.css", "/foo.css" }, { "/bar.css", "/bar.css" } },
					IncludeType.Css,
					string.Format("<link rel='stylesheet' type='text/css' href='/foo.css'/>{0}<link rel='stylesheet' type='text/css' href='/bar.css'/>{0}", Environment.NewLine)
				};
				yield return new object[]
				{
					new Dictionary<string, string> { { "/foo.js", "/foo.js" }, { "/bar.js", "/bar.js" } },
					IncludeType.Js,
					string.Format("<script type='text/javascript' src='/foo.js'></script>{0}<script type='text/javascript' src='/bar.js'></script>{0}", Environment.NewLine)
				};
				yield return new object[]
				{
					new Dictionary<string, string> { { "~/content/js/foo.js", "/content/js/foo.js" }, { "/bar.js", "/bar.js" } },
					IncludeType.Js,
					string.Format("<script type='text/javascript' src='/content/js/foo.js'></script>{0}<script type='text/javascript' src='/bar.js'></script>{0}", Environment.NewLine)
				};
			}
		}

		public static IEnumerable<object[]> RenderingInRelease
		{
			get
			{
				yield return new object[]
				{
					new Dictionary<string, string> { { "~/content/js/foo.js", "/content/js/foo.js" }, { "/bar.js", "/bar.js" } },
					IncludeType.Js,
					"hashed",
					"<script type='text/javascript' src='/content/js/hashed.js'></script>",
					new JsTypeElement()
				};
				yield return new object[]
				{
					new Dictionary<string, string> { { "~/content/css/foo.css", "/content/css/foo.css" }, { "/bar.css", "/bar.css" } },
					IncludeType.Css,
					"hashed==",
					"<link rel='stylesheet' type='text/css' href='/content/css/hashed==.css'/>",
					new CssTypeElement()
				};
				yield return new object[]
				{
					new Dictionary<string, string> { { "~/content/css/foo.css", "/content/css/foo.css" }, { "/bar.css", "/bar.css" } },
					IncludeType.Css,
					"really/nasty%20url=",
					"<link rel='stylesheet' type='text/css' href='/content/css/really/nasty%20url=.css'/>",
					new CssTypeElement()
				};
			}
		}

		public static IEnumerable<object[]> RegisterCombination
		{
			get
			{
				yield return new object[]
				{
					IncludeType.Js,
					new Dictionary<string, Include>
					{
						{ "~/content/js/foo.js", new Include(IncludeType.Js, "/content/js/foo.js", "alert('hello world!');", Clock.UtcNow) }
					},
					new JsTypeElement()
				};
			}
		}

		[Fact]
		public void RenderIncludes_ShouldWriteNothing_WhenNoSourcesArePending()
		{
			string rendered = null;
			Assert.DoesNotThrow(() => rendered = _combiner.RenderIncludes(new string[0], IncludeType.Js, false));
			Assert.Equal("", rendered);
		}

		[Theory]
		[PropertyData("RenderingInDebug")]
		public void RenderIncludes_ShouldWriteOutEachIncludeSeparately_WhenInDebugMode(IDictionary<string, string> includes, IncludeType type, string expected)
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttp.Expect(s => s.Context).Return(stubContext);
			foreach (var kvp in includes)
			{
				_mockReader.Expect(sr => sr.ToAbsolute(kvp.Key)).Return(kvp.Value);
			}
			_mockStorage.Expect(s => s.Clear());
			string rendered = null;
			Assert.DoesNotThrow(() => rendered = _combiner.RenderIncludes(includes.Keys, type, true));
			Assert.Equal(rendered, expected);
		}

		[Theory]
		[FreezeClock]
		[PropertyData("RenderingInRelease")]
		public void RenderIncludes_ShouldWriteOutASingleReferenceToTheCompressorController_WhenInReleaseMode(IDictionary<string, string> includes, IncludeType type, string key, string expected, IncludeTypeElement settings)
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(false);
			_mockHttp.Expect(s => s.Context).Return(stubContext);
			_mockSettings.Expect(s => s.Types[type]).Return(settings);
			foreach (var kvp in includes)
			{
				var include = new Include(type, kvp.Key, "foo", Clock.UtcNow);
				_mockReader.Expect(r => r.Read(kvp.Key, type)).Return(include);
				_mockStorage.Expect(s => s.Store(include));
			}
			_mockReader.Expect(r => r.ToAbsolute(Arg<string>.Is.NotNull)).Return(string.Format("/content/{0}/{1}.{0}", type.ToString().ToLowerInvariant(), key));
			string hash = null;
			_mockStorage.Expect(s => hash = s.Store(Arg<IncludeCombination>.Is.NotNull)).Return("foo");

			string reference = null;
			Assert.DoesNotThrow(() => reference = _combiner.RenderIncludes(includes.Keys, type, false));
			Assert.Equal(expected, reference);
		}

		[Theory]
		[FreezeClock]
		[PropertyData("RegisterCombination")]
		public void RegisterCombination_ShouldReadAllSourcesToAddEachToTheCombination_AndReturnAHash(IncludeType type, IDictionary<string, Include> sources, IncludeTypeElement settings)
		{
			foreach (var kvp in sources)
			{
				_mockReader.Expect(r => r.Read(kvp.Key, kvp.Value.Type)).Return(kvp.Value);
				_mockStorage.Expect(s => s.Store(kvp.Value));
			}
			_mockStorage.Expect(s => s.Store(new IncludeCombination(type, sources.Keys, "content", Clock.UtcNow, settings))).IgnoreArguments().Return("foo");
			_mockSettings.Expect(s => s.Types).Return(new Dictionary<IncludeType, IIncludeTypeSettings> {{type,settings }});
			string key = null;
			Assert.DoesNotThrow(() => key = _combiner.RegisterCombination(sources.Keys, IncludeType.Js, Clock.UtcNow));
			Assert.Equal("foo", key);
		}
	}
}