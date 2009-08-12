using System;
using System.Web.Mvc;
using MvcContrib;

namespace Website.Controllers.SubControllers
{
	public class FormSubmitController : SubController<DateTime>
	{
		[AcceptVerbs("POST")]
		public ViewResult FormSubmit(string sometextbox)
		{
			ViewData["sometextbox"] = sometextbox + " submitted on " + Model.ToShortDateString();
			return View("posted");
		}

		[AcceptVerbs("GET")]
		public ViewResult FormSubmit()
		{
			return View();
		}
	}
}