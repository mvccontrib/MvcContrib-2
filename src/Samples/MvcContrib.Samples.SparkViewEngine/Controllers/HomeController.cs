using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MvcContrib.Samples.SparkViewEngine.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewData["Title"] = "Home Page";
			ViewData["Message"] = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			ViewData["Title"] = "About Page";

			return View();
		}

		public ActionResult ThrowViewException()
		{
			return View();
		}

	}
}
