using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.BrailViewEngine;
using MvcContrib.ViewFactories;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;
using Rhino.Mocks;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class BrailViewFactoryTester
	{
		private IViewEngine _viewEngine;
	    private MockRepository _mocks;

		private static readonly string VIEW_ROOT_DIRECTORY = @"BrailViewEngine\Views";

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			var httpContext = new TestHttpContext();
			var requestContext = new RequestContext(httpContext, new RouteData());
            var controller = _mocks.StrictMock<ControllerBase>();
			_mocks.Replay(controller);

			var viewEngine = new BooViewEngine
			                 	{
			                 		ViewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY),
			                 		Options = new BooViewEngineOptions()
			                 	};
			viewEngine.Initialize();

			_viewEngine = new BrailViewFactory(viewEngine);
			_mocks.ReplayAll();
		}

		[Test]
		public void Can_Create_Default_ViewFactory()
		{
			var viewFactory = new BrailViewFactory();
			Assert.IsNotNull(viewFactory.ViewEngine);
		}

		[Test]
		public void Can_Create_View()
		{
			var routeData = new RouteData();
			routeData.Values.Add("controller", "home");
			var result = _viewEngine.FindView(new ControllerContext() { RouteData = routeData }, "view", null, false);
			Assert.IsNotNull(result.View);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_BooViewEngine()
		{
			new BrailViewFactory(null);
		}

		[Test]
		public void ReleaseView_should_not_throw()
		{
			_viewEngine.ReleaseView(null, null);
		}
	}
}
