using System.Web.Mvc;
using MvcContrib.UI.MenuBuilder;

namespace MvcContrib.Samples.UI.Controllers 
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewData["Message"] = "Welcome to ASP.NET MVC!";

			return View();
		}

		[MenuHelpText("A page about us")]
		public ActionResult About()
		{
			return View();
		}

		[Authorize]
		[MenuTitle("A Very Secure Page")]
		public ActionResult SecurePage1()
		{
			return View();
		}

		[Authorize]
		public ActionResult SecurePage2()
		{
			return View();
		}
	}
}