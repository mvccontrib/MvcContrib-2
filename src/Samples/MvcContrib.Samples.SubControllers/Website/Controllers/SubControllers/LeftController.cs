using System.Web.Mvc;
using MvcContrib;

namespace Website.Controllers.SubControllers
{
	public class LeftController : SubController
	{
		public ViewResult Left(int? number, string name, RightController right)
		{
			//Subcontroller actions have all the behavior of regular actions, including pulling things off querystrings
			ViewData["text"] = string.Format("{0}:{1}", name, number);
			return View();
		}
	}
}