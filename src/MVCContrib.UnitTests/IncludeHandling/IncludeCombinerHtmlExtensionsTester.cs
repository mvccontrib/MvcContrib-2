using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeCombinerHtmlExtensionsTester
	{
		private HtmlHelper _html;
		private IHttpContextProvider _mockHttpContextProvider;
		private MockRepository _mocks;
		private ViewDataDictionary _viewData;
		private IIncludeHandlingSettings _mockSettings;

		[SetUp]
		public void TestSetup()
		{
			_viewData = new ViewDataDictionary();
			_mocks = new MockRepository();
			_mockHttpContextProvider = _mocks.Stub<IHttpContextProvider>();
			_mockHttpContextProvider.Expect(hcp => hcp.Request).Return(_mocks.Stub<HttpRequestBase>()).Repeat.Twice();
			_mockSettings = _mocks.Stub<IIncludeHandlingSettings>();

			_html = WebTestUtility.BuildHtmlHelper(_mocks, _viewData, null);
			_mocks.ReplayAll();
			var resolver = new QnDDepResolver(_mockHttpContextProvider, _mockSettings, new Controller[] { });
			DependencyResolver.InitializeWith(resolver);
		}

		[Test]
		public void AddInclude_ShouldAppendIncludeToSetInViewData()
		{
			_html.IncludeCss("~/content/css/site.css");

			var set = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.IsNotNull(set);
			Assert.AreEqual(1, set.Count);
			Assert.AreEqual("~/content/css/site.css", set[0]);
		}

		[Test]
		public void AddMultipleIncludes_ShouldAppendIncludeToSetInSameOrderAsAdded()
		{
			_html.IncludeJs("foo");
			_html.IncludeJs("bar");
			_html.IncludeJs("baz");

			var set = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.IsNotNull(set);
			Assert.AreEqual(3, set.Count);
			Assert.AreEqual("foo", set[0]);
			Assert.AreEqual("bar", set[1]);
			Assert.AreEqual("baz", set[2]);
		}

		[Test]
		public void AddSameIncludeMoreThanOnce_ShouldOnlyAddIncludeTheFirstTime()
		{
			_html.IncludeJs("foo");
			_html.IncludeJs("bar");
			_html.IncludeJs("foo");

			var set = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.IsNotNull(set);
			Assert.AreEqual(2, set.Count);
			Assert.AreEqual("foo", set[0]);
			Assert.AreEqual("bar", set[1]);
		}

		[Test]
		public void IncludeJs_ViaParams_ShouldWork()
		{
			_html.IncludeJs("foo.js", "bar.js");
			var set = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.IsNotNull(set);
			Assert.AreEqual(2, set.Count);
			Assert.AreEqual("foo.js", set[0]);
			Assert.AreEqual("bar.js", set[1]);
		}

		[Test]
		public void IncludeCss_ViaParams_ShouldWork()
		{
			_html.IncludeCss("foo.css", "bar.css");
			var set = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.IsNotNull(set);
			Assert.AreEqual(2, set.Count);
			Assert.AreEqual("foo.css", set[0]);
			Assert.AreEqual("bar.css", set[1]);
		}

		[Test]
		public void AddIncludesOfDifferentTypes_ShouldAddToAppropriateSet()
		{
			_html.IncludeJs("foo.js");
			_html.IncludeCss("foo.css");

			var jsSet = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.IsNotNull(jsSet);
			Assert.AreEqual(1, jsSet.Count);
			Assert.AreEqual("foo.js", jsSet[0]);

			var cssSet = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.IsNotNull(cssSet);
			Assert.AreEqual(1, cssSet.Count);
			Assert.AreEqual("foo.css", cssSet[0]);
		}

		[Test]
		public void RenderCss_ShouldFlushTheSet()
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttpContextProvider.Expect(s => s.Context).Return(stubContext);
			_html.IncludeCss("/foo.css");
			var before = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.AreEqual(1, before.Count);

			_html.RenderCss(true);

			var after = _viewData[getViewDataKey(IncludeType.Css)] as IList<string>;
			Assert.AreEqual(0, after.Count);
		}

		[Test]
		public void RenderJs_ShouldFlushTheSet()
		{
			var stubContext = _mocks.Stub<HttpContextBase>();
			stubContext.Replay();
			stubContext.Expect(c => c.IsDebuggingEnabled).Return(true);
			_mockHttpContextProvider.Expect(s => s.Context).Return(stubContext);
			_html.IncludeJs("/foo.js");
			var before = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.AreEqual(1, before.Count);

			_html.RenderJs(true);

			var after = _viewData[getViewDataKey(IncludeType.Js)] as IList<string>;
			Assert.AreEqual(0, after.Count);
		}

		private static string getViewDataKey(IncludeType type)
		{
			return typeof (IncludeCombinerHtmlExtensions).FullName + "_" + type;
		}
	}

	public class QnDDepResolver : IDependencyResolver
	{
		private readonly IDictionary<Type, object> types;

		public QnDDepResolver(IHttpContextProvider httpContextProvider, IIncludeHandlingSettings settings, Controller[] controllers)
		{
			types = new Dictionary<Type, object>
			{
				{ typeof (IHttpContextProvider), httpContextProvider },
				{ typeof (IKeyGenerator), new KeyGenerator() },
				{ typeof (IIncludeHandlingSettings), settings }
			};
			types.Add(typeof(IIncludeReader), new FileSystemIncludeReader((IHttpContextProvider)types[typeof(IHttpContextProvider)]));

			var keyGen = (IKeyGenerator)types[typeof(IKeyGenerator)];

			types.Add(typeof(IIncludeStorage), new StaticIncludeStorage(keyGen));

			var includeReader = (IIncludeReader)types[typeof(IIncludeReader)];
			var storage = (IIncludeStorage)types[typeof(IIncludeStorage)];
			var combiner = new IncludeCombiner(settings, includeReader, storage, (IHttpContextProvider)types[typeof(IHttpContextProvider)]);
			types.Add(typeof(IIncludeCombiner), combiner);

			types.Add(typeof(IncludeController), new IncludeController(settings, combiner));
			foreach (var controller in controllers)
			{
				types.Add(controller.GetType(), controller);
			}
		}

		public Interface GetImplementationOf<Interface>()
		{
			return (Interface)types[typeof(Interface)];
		}

		public Interface GetImplementationOf<Interface>(Type type)
		{
			return (Interface)types[type];
		}

		public object GetImplementationOf(Type type)
		{
			return types[type];
		}

		public void DisposeImplementation(object instance)
		{
			throw new NotImplementedException();
		}
	}
}