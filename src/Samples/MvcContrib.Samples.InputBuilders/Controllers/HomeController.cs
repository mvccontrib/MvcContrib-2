using System;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Display()
		{
			return
				View(new SampleDisplay
				     	{
				     		Name = "Jeffrey Palermo",
				     		Html = "<i>some html content</i>",
				     		IsNeeded = true,
				     		TimeStamp = new DateTime(2000, 1, 1, 20, 15, 0)
				     	});
		}

		public ActionResult InputForm()
		{
			return View(new SampleInput
			            	{
			            		Name = "stuff",
			            		TimeStamp = DateTime.Today.AddHours(13).AddMinutes(30)
			            	});
		}

		[OutputCache(Duration = 10, VaryByParam = "")]
		public ActionResult Index()
		{
			return View(new SampleInput
			            	{
			            		Name = "foo",
			            		TimeStamp = DateTime.Today.AddHours(13).AddMinutes(30)
			            	});
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ActionName("InputForm")]
		public ActionResult Save(SampleInput model)
		{
			if (ModelState.IsValid)
			{
			}
			return View(model);
		}
	}
}