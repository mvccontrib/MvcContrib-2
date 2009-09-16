using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using Xunit;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class IncludeCombinerHtmlExtensionsFacts
	{
		private readonly HtmlHelper _html;
		private readonly IHttpContextProvider _mockHttpContextProvider;
		private readonly MockRepository _mocks;
		private readonly ViewDataDictionary _viewData;
		private readonly IIncludeHandlingSettings _mockSettings;

		public IncludeCombinerHtmlExtensionsFacts()
		{
			_mocks = new MockRepository();

			_mockHttpContextProvider = _mocks.Stub<IHttpContextProvider>();
			_mockHttpContextProvider.Expect(hcp => hcp.Request).Return(_mocks.Stub<HttpRequestBase>()).Repeat.Twice();
			_mockSettings = _mocks.Stub<IIncludeHandlingSettings>();
			ServiceLocator.SetLocatorProvider(() => QnDServiceLocator.Create(_mockHttpContextProvider, _mockSettings, new Controller[] { }));

			_viewData = new ViewDataDictionary();

			_html = WebTestUtility.BuildHtmlHelper(_mocks, _viewData, null);
			_mocks.ReplayAll();
		}

		[Fact]
		public void AddInclude_ShouldAppendIncludeToSetInViewData()
		{
			_html.IncludeCss("~/content/css/site.css");

			var set = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.NotNull(set);
			Assert.Equal(1, set.Count);
			Assert.Equal("~/content/css/site.css", set[0]);
		}

		[Fact]
		public void AddMultipleIncludes_ShouldAppendIncludeToSetInSameOrderAsAdded()
		{
			_html.IncludeJs("foo");
			_html.IncludeJs("bar");
			_html.IncludeJs("baz");

			var set = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.NotNull(set);
			Assert.Equal(3, set.Count);
			Assert.Equal("foo", set[0]);
			Assert.Equal("bar", set[1]);
			Assert.Equal("baz", set[2]);
		}

		[Fact]
		public void AddSameIncludeMoreThanOnce_ShouldOnlyAddIncludeTheFirstTime()
		{
			_html.IncludeJs("foo");
			_html.IncludeJs("bar");
			_html.IncludeJs("foo");

			var set = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.NotNull(set);
			Assert.Equal(2, set.Count);
			Assert.Equal("foo", set[0]);
			Assert.Equal("bar", set[1]);
		}

		[Fact]
		public void IncludeJs_ViaParams_ShouldWork()
		{
			_html.IncludeJs("foo.js", "bar.js");
			var set = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.NotNull(set);
			Assert.Equal(2, set.Count);
			Assert.Equal("foo.js", set[0]);
			Assert.Equal("bar.js", set[1]);
		}

		[Fact]
		public void IncludeCss_ViaParams_ShouldWork()
		{
			_html.IncludeCss("foo.css", "bar.css");
			var set = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.NotNull(set);
			Assert.Equal(2, set.Count);
			Assert.Equal("foo.css", set[0]);
			Assert.Equal("bar.css", set[1]);
		}

		[Fact]
		public void AddIncludesOfDifferentTypes_ShouldAddToAppropriateSet()
		{
			_html.IncludeJs("foo.js");
			_html.IncludeCss("foo.css");

			var jsSet = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.NotNull(jsSet);
			Assert.Equal(1, jsSet.Count);
			Assert.Equal("foo.js", jsSet[0]);

			var cssSet = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.NotNull(cssSet);
			Assert.Equal(1, cssSet.Count);
			Assert.Equal("foo.css", cssSet[0]);
		}

		[Fact]
		public void RenderCss_ShouldFlushTheSet()
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttpContextProvider.Expect(s => s.Context).Return(stubContext);
			_html.IncludeCss("/foo.css");
			var before = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.Equal(1, before.Count);

			_html.RenderCss(true);

			var after = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.Equal(0, after.Count);
		}

		[Fact]
		public void RenderJs_ShouldFlushTheSet()
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttpContextProvider.Expect(s => s.Context).Return(stubContext);
			_html.IncludeJs("/foo.js");
			var before = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.Equal(1, before.Count);

			_html.RenderJs(true);

			var after = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.Equal(0, after.Count);
		}

		private static string getViewDataKey(IncludeType type)
		{
			return typeof (IncludeCombinerHtmlExtensions).FullName + "_" + type;
		}
	}
}