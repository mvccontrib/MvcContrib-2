using System.Web.Mvc;

namespace MvcContrib.Samples.NVelocityViewFactory.Controllers
{
    public class WidgetSubController : SubController
    {
        //notice how the action is named the same as the controller (minus the "SubController" part) -- this is important
        public ActionResult Widget()
        {
            return Content("I'm in a subcontroller!");
        }
    }
}