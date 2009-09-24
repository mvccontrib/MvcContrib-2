using System;
using System.Collections.Generic;
using System.Web;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeCombinerStateTester
	{
		private IIncludeCombiner _combiner;
		private IIncludeReader _mockReader;
		private MockRepository _mocks;
		private IIncludeStorage _mockStorage;
		private IIncludeHandlingSettings _mockSettings;
		private IHttpContextProvider _mockHttp;
		
		[Datapoint] 
		public RenderingInDebug ScriptWithPath;
		[Datapoint]
		public RenderingInDebug CssInDebug;
		[Datapoint]
		public RenderingInDebug ScriptInDebug;

		[Datapoint]
		public RenderingInRelease CssInReleaseWithArseyUrl;
		[Datapoint]
		public RenderingInRelease ScriptInRelease;
		[Datapoint]
		public RenderingInRelease CssInRelease;

		public IncludeCombinerStateTester()
		{
			CssInDebug = new RenderingInDebug
				{
					Includes = new Dictionary<string, string> { { "/foo.css", "/foo.css" }, { "/bar.css", "/bar.css" } },
					Type = IncludeType.Css,
					Expected = string.Format("<link rel='stylesheet' type='text/css' href='/foo.css'/>{0}<link rel='stylesheet' type='text/css' href='/bar.css'/>{0}", Environment.NewLine)
				};
			ScriptInDebug = new RenderingInDebug
				{
					Includes = new Dictionary<string, string> { { "/foo.js", "/foo.js" }, { "/bar.js", "/bar.js" } },
					Type = IncludeType.Js,
					Expected = string.Format("<script type='text/javascript' src='/foo.js'></script>{0}<script type='text/javascript' src='/bar.js'></script>{0}", Environment.NewLine)
				};
			ScriptWithPath = new RenderingInDebug
				{
					Includes = new Dictionary<string, string> { { "~/content/js/foo.js", "/content/js/foo.js" }, { "/bar.js", "/bar.js" } },
					Type = IncludeType.Js,
					Expected = string.Format("<script type='text/javascript' src='/content/js/foo.js'></script>{0}<script type='text/javascript' src='/bar.js'></script>{0}", Environment.NewLine)
				};

			ScriptInRelease = new RenderingInRelease
				{
					Includes = new Dictionary<string, string> { { "~/content/js/foo.js", "/content/js/foo.js" }, { "/bar.js", "/bar.js" } },
					Type = IncludeType.Js,
					Key = "hashed",
					Expected = "<script type='text/javascript' src='/content/js/hashed.js'></script>",
					Settings = new JsTypeElement()
				};
			CssInRelease = new RenderingInRelease
				{
					Includes = new Dictionary<string, string> { { "~/content/css/foo.css", "/content/css/foo.css" }, { "/bar.css", "/bar.css" } },
					Type = IncludeType.Css,
					Key = "hashed==",
					Expected = "<link rel='stylesheet' type='text/css' href='/content/css/hashed==.css'/>",
					Settings = new CssTypeElement()
				};
			CssInReleaseWithArseyUrl = new RenderingInRelease
				{
					Includes = new Dictionary<string, string> { { "~/content/css/foo.css", "/content/css/foo.css" }, { "/bar.css", "/bar.css" } },
					Type = IncludeType.Css,
					Key = "really/nasty%20url=",
					Expected = "<link rel='stylesheet' type='text/css' href='/content/css/really/nasty%20url=.css'/>",
					Settings = new CssTypeElement()
				};

		}

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_mockSettings = _mocks.DynamicMock<IIncludeHandlingSettings>();
			_mockReader = _mocks.DynamicMock<IIncludeReader>();
			_mockStorage = _mocks.DynamicMock<IIncludeStorage>();
			_mockHttp = _mocks.DynamicMock<IHttpContextProvider>();
			_combiner = new IncludeCombiner(_mockSettings, _mockReader, _mockStorage, _mockHttp);
			_mocks.ReplayAll();
		}

		[Test]
		public void RenderIncludes_ShouldWriteNothing_WhenNoSourcesArePending()
		{
			string rendered = _combiner.RenderIncludes(new string[0], IncludeType.Js, false);
			Assert.AreEqual("", rendered);
		}

		public class RenderingInDebug
		{
			public IDictionary<string, string> Includes { get; set; }
			public IncludeType Type { get; set; }
			public string Expected { get; set; }
		}

		[Theory]
		public void RenderIncludes_ShouldWriteOutEachIncludeSeparately_WhenInDebugMode(RenderingInDebug data)
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttp.Expect(s => s.Context).Return(stubContext);
			foreach (var kvp in data.Includes)
			{
				_mockReader.Expect(sr => sr.ToAbsolute(kvp.Key)).Return(kvp.Value);
			}
			_mockStorage.Expect(s => s.Clear());
			string rendered = _combiner.RenderIncludes(data.Includes.Keys, data.Type, true);
			Assert.AreEqual(rendered, data.Expected);
		}

		public class RenderingInRelease
		{
			public IDictionary<string, string> Includes { get; set; }
			public IncludeType Type { get; set; }
			public string Key { get; set; }
			public string Expected { get; set; }
			public IncludeTypeElement Settings { get; set; }
		}

		[Theory]
		public void RenderIncludes_ShouldWriteOutASingleReferenceToTheCompressorController_WhenInReleaseMode(RenderingInRelease data)
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(false);
			_mockHttp.Expect(s => s.Context).Return(stubContext);
			_mockSettings.Expect(s => s.Types[data.Type]).Return(data.Settings);
			foreach (var kvp in data.Includes)
			{
				var include = new Include(data.Type, kvp.Key, "foo", DateTime.UtcNow);
				_mockReader.Expect(r => r.Read(kvp.Key, data.Type)).Return(include);
				_mockStorage.Expect(s => s.Store(include));
			}
			_mockReader.Expect(r => r.ToAbsolute(Arg<string>.Is.NotNull)).Return(string.Format("/content/{0}/{1}.{0}", data.Type.ToString().ToLowerInvariant(), data.Key));
			string hash = null;
			_mockStorage.Expect(s => hash = s.Store(Arg<IncludeCombination>.Is.NotNull)).Return("foo");

			string reference = _combiner.RenderIncludes(data.Includes.Keys, data.Type, false);
			Assert.AreEqual(data.Expected, reference);
		}

		[Test]
		public void RegisterCombination_ShouldReadAllSourcesToAddEachToTheCombination_AndReturnAHash()
		{
			var type = IncludeType.Js;
			var sources = new Dictionary<string, Include>
			{
				{
					"~/content/js/foo.js", new Include(IncludeType.Js, "/content/js/foo.js", "alert('hello world!');", DateTime.UtcNow)
					}
			};
			var settings = new JsTypeElement();
			foreach (var kvp in sources)
			{
				_mockReader.Expect(r => r.Read(kvp.Key, kvp.Value.Type)).Return(kvp.Value);
				_mockStorage.Expect(s => s.Store(kvp.Value));
			}
			_mockStorage.Expect(s => s.Store(new IncludeCombination(type, sources.Keys, "content", DateTime.UtcNow, settings))).IgnoreArguments().Return("foo");
			_mockSettings.Expect(s => s.Types).Return(new Dictionary<IncludeType, IIncludeTypeSettings> {{type,settings }});
			string key = _combiner.RegisterCombination(sources.Keys, IncludeType.Js, DateTime.UtcNow);
			Assert.AreEqual("foo", key);
		}
	}
}