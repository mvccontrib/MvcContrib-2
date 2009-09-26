using System.Web.Mvc;

namespace Demo.Site.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewData["Message"] = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult JsJquery()
		{
			return View();
		}

		public ActionResult JsMooTools()
		{
			return View();
		}

		public ActionResult JsPrototype()
		{
			return View();
		}

		public ActionResult JsMany()
		{
			return View();
		}

		public ActionResult CssOne()
		{
			return View();
		}

		public ActionResult CssMany()
		{
			return View();
		}
	}
}