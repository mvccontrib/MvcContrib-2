using System.Web.Mvc;
using MvcContrib.Filters;

namespace MvcContrib.Samples.NVelocityViewFactory.Controllers
{
    [Layout("Site")]
    public class MainController : Controller
    {
        public ActionResult Index(WidgetSubController widgetSubController)
        {
            //notice that we call GetResult, not the actual action
            ViewData["widgetSubController"] = widgetSubController.GetResult(this);
            return View();
        }
    }
}