using System.Net;
using System.Web.Mvc;
using MvcContrib.FluentController;
using MvcContrib.SimplyRestful;
using MvcContrib.UnitTests.TestHelper.FluentController.Core;

namespace MvcContrib.UnitTests.TestHelper.FluentController.UI
{
    public class UserController : AbstractRestfulFluentController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(object model)
        {
            return CheckValidCall()
                .Valid(x => RedirectToAction(RestfulAction.Index))
                .Invalid(() => View("New", model));
        }

        public ActionResult CreateWithModel()
        {
            return CheckValidCall(() => new CustomerResult { FirstName = "Bob" })
                .Valid(x => View(RestfulAction.New, x))
                .Invalid(() => RedirectToAction(RestfulAction.Index));
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Show()
        {
            return new HeadResult(HttpStatusCode.OK);
        }

        public ActionResult EmptyAction()
        {
            return new EmptyResult();
        }

        public ActionResult NullAction()
        {
            return null;
        }

        public ActionResult CheckHeaderLocation()
        {
            var canReadRequestHeader = Request.Url.AbsoluteUri;
            return null;
        }
    }
}