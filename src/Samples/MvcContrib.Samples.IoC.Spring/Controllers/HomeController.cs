using System;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.Samples.IoC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        
        public ActionResult About()
        {
            return View("About");
        }
    }
}
