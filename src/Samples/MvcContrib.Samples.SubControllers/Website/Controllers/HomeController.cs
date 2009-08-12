using System.Web.Mvc;
using MvcContrib.Filters;
using Website.Controllers.SubControllers;

namespace Website.Controllers
{
	[HandleError]
	[SubControllerActionToViewData]
	public class HomeController : Controller
	{
		public ActionResult Index(FirstLevelSubController firstLevel)
		{
			ViewData["Title"] = "Home Page";
			ViewData["Message"] = "Welcome to ASP.NET MVC!";
			ViewData["text"] = "I am a top-level controller";

			return View();
		}

		public ActionResult About()
		{
			ViewData["Title"] = "About Page";

			return View();
		}
	}
}