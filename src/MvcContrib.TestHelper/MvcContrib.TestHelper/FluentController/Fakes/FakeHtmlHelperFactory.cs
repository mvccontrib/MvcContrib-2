using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using System.Web;


namespace MvcContrib.TestHelper.FluentController.Fakes
{
    public static class FakeHtmlHelperFactory
    {
        public static HtmlHelper CreateHtmlHelper(this HttpContextBase httpContext)
        {
            return CreateHtmlHelper(httpContext, null);
        }

        public static HtmlHelper CreateHtmlHelper(this HttpContextBase httpContext, RouteBase route)
        {
            var mockView = new Mock<IView>();
            var mockViewData = new Mock<IViewDataContainer>();

            var routeData = new RouteData();
           
            var controllerContext = new ControllerContext(httpContext, routeData, new Mock<ControllerBase>().Object);

            var routeCollection = new RouteCollection();
            if (route != null)
                routeCollection.Add(route);

            var viewContext = new ViewContext(controllerContext, mockView.Object, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());

            var htmlHelper = new HtmlHelper(viewContext, mockViewData.Object, routeCollection);

            return htmlHelper;
        }

        public static Mock<RouteBase> CreateMockRoute(string expectedAction, string expectedController, string outputUrl)
        {
            var mockRoute = new Mock<RouteBase>();
            var returnVal = new VirtualPathData(mockRoute.Object, outputUrl);

            mockRoute.Setup(x => x.GetVirtualPath(It.IsAny<RequestContext>(), It.Is<RouteValueDictionary>(dict => dict["action"].As<string>() == expectedAction && dict["controller"].As<string>() == expectedController))).Returns(returnVal).Verifiable();


            return mockRoute;
        }

        public static Mock<RouteBase> AddMockRoute(this HtmlHelper htmlHelper, string expectedAction, string expectedController, string outputUrl)
        {
            var mockRoute = CreateMockRoute(expectedAction, expectedController, outputUrl);
            htmlHelper.RouteCollection.Add(mockRoute.Object);

            return mockRoute;
        }
    }
}