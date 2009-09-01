using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System;

namespace MvcContrib.UnitTests.ActionResults
{
    [TestFixture]
    public class PermanentRedirectToActionResultTester
    {
        private MockRepository _mocks;
        private ControllerContext _controllerContext;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockRepository();
            _controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<ControllerBase>());
            _mocks.ReplayAll();
        }

        [Test]
        public void ActionName_should_return_the_action_name()
        {
            var result = new PermanentRedirectToActionResult("action", "controller", new { id = 1 });
            Assert.AreEqual("action", result.ActionName);
        }

        [Test]
        public void ControllerName_should_return_the_controller_name()
        {
            var result = new PermanentRedirectToActionResult("action", "controller", new { id = 1 });
            Assert.AreEqual("controller", result.ControllerName);
        }

        [Test]
        public void RouteValues_should_return_the_route_values()
        {
            var routeValues = new { id = 1 };
            var result = new PermanentRedirectToActionResult("action", "controller", routeValues);
            Assert.AreEqual(routeValues, result.RouteValues);
        }

        [Test]
        public void Should_set_status_code()
        {
            var result = new PermanentRedirectToActionResult("action", "controller", new { id = 1 });
            result.ExecuteResult(_controllerContext);
            Assert.AreEqual(301, _controllerContext.HttpContext.Response.StatusCode);
        }

        [Test, Ignore]
        // Something in DynamicHttpContextBase prevents UrlHelper.Action from ever
        // returning anything but null.
        public void Should_set_redirect_location()
        {
            var result = new PermanentRedirectToActionResult("action", "controller", new { id = 1 });
            result.ExecuteResult(_controllerContext);
            Assert.IsNotNull(_controllerContext.HttpContext.Response.RedirectLocation);
        }
    }
}
