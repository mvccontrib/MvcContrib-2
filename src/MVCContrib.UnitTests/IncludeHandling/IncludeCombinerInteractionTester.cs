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
	public class IncludeCombinerInteractionTester
	{
		private IIncludeCombiner _combiner;
		private IIncludeReader _mockReader;
		private MockRepository _mocks;
		private IIncludeStorage _mockStorage;
		private IIncludeHandlingSettings _mockSettings;
		private IHttpContextProvider _mockHttp;

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_mockSettings = _mocks.StrictMock<IIncludeHandlingSettings>();
			_mockReader = _mocks.StrictMock<IIncludeReader>();
			_mockStorage = _mocks.StrictMock<IIncludeStorage>();
			_mockHttp = _mocks.StrictMock<IHttpContextProvider>();
			_combiner = new IncludeCombiner(_mockSettings, _mockReader, _mockStorage, _mockHttp);
			_mocks.ReplayAll();
		}

		[Test]
		public void RegisterInclude_ShouldAskStorageToStoreIt()
		{
			var include = new Include(IncludeType.Js, "foo.js", "alert('');", DateTime.UtcNow);
			_mockReader.Expect(r => r.Read("~/foo.js", include.Type)).Return(include);
			_mockStorage.Expect(s => s.Store(include));
			_combiner.RegisterInclude("~/foo.js", IncludeType.Js);
			_mocks.VerifyAll();
		}

		[Test]
		public void GetCombination_ShouldAskStorageForCombination()
		{
			_mockStorage.Expect(s => s.GetCombination("foo")).Return(new IncludeCombination(IncludeType.Css, new[] { "~/content/css/foo.css" }, ".foo{}", DateTime.UtcNow, new CssTypeElement()));
			IncludeCombination combination = _combiner.GetCombination("foo");
			Assert.IsNotNull(combination);
			_mocks.VerifyAll();
		}

		[Test]
		public void SetCombination_ShouldTellStorageToStore()
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, "alert();", DateTime.UtcNow, new JsTypeElement());
			_mockStorage.Expect(s => s.Store(combination)).Return("foo");

			_combiner.UpdateCombination(combination);
			_mocks.VerifyAll();
		}

		[Test]
		public void GetAllIncludes_ShouldAskStorageForIncludes()
		{
			_mockStorage.Expect(s => s.GetAllIncludes()).Return(new[] { new Include(IncludeType.Css, "~/foo.css", "#foo{color:red;}", DateTime.UtcNow) });
			IEnumerable<Include> includes = _combiner.GetAllIncludes();
			Assert.IsNotNull(includes);
			_mocks.VerifyAll();
		}

		[Test]
		public void GetAllCombinations_ShouldAskStorageForCombinations()
		{
			_mockStorage.Expect(s => s.GetAllCombinations()).Return(new Dictionary<string, IncludeCombination>());
			IDictionary<string, IncludeCombination> combinations = _combiner.GetAllCombinations();
			Assert.IsNotNull(combinations);
			_mocks.VerifyAll();
		}

		[Test]
		public void RenderIncludes_InDebugMode_ShouldClearStorage()
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttp.Expect(s => s.Context).Return(stubContext);
			_mockReader.Expect(r => r.ToAbsolute("foo.js")).Return("/foo.js");
			_mockStorage.Expect(s => s.Clear());
			string rendered = _combiner.RenderIncludes(new[] { "foo.js" }, IncludeType.Js, true);
			_mocks.VerifyAll();
		}

		[Test]
		public void Clear_ShouldTellStorageToClear()
		{
			_mockStorage.Expect(s => s.Clear());
			_combiner.Clear();
			_mocks.VerifyAll();
		}
	}
}