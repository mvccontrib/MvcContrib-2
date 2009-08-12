using System;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult InputForm()
		{
			return View(new SampleModel()
			                {
			                    Name = "stuff",
                                TimeStamp = DateTime.Today.AddHours(13).AddMinutes(30)
			                });
		}

		[OutputCache(Duration = 10,VaryByParam = "")]
		public ActionResult Index()
		{
			ViewData["Message"] = "Welcome to ASP.NET MVC!";

			//ViewData.ModelState.AddModelError("Html","The Html is required!");
			return View(new SampleModel()
			                {
			                    Name = "foo",
                                TimeStamp = DateTime.Today.AddHours(13).AddMinutes(30)
			                });
		}

		public ActionResult Save(SampleModel model)
		{
			if (ModelState.IsValid)
			{
			}
			return View("index", model);
		}
	}
}