using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.ActionResults
{
    [TestFixture]
    public class PermanentRedirectResultTester
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
        public void Url_should_return_the_url_to_redirect_to()
        {
            var result = new PermanentRedirectResult("test");
            Assert.That(result.Url, Is.EqualTo("test"));
        }

        [Test]
        public void Should_set_status_code()
        {
            var result = new PermanentRedirectResult("test");   
            result.ExecuteResult(_controllerContext);
            Assert.AreEqual(301, _controllerContext.HttpContext.Response.StatusCode);
        }

        [Test]
        public void Should_set_redirect_location()
        {
            var result = new PermanentRedirectResult("test");
            result.ExecuteResult(_controllerContext);
            Assert.AreEqual("test", _controllerContext.HttpContext.Response.RedirectLocation);
            
        }
    }
}
