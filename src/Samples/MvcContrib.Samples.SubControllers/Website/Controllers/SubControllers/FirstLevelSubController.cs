using System.Web.Mvc;
using MvcContrib;

namespace Website.Controllers.SubControllers
{
	public class FirstLevelSubController : SubController
	{
		public ViewResult FirstLevel(SecondLevelSubController secondLevel)
		{
			ViewData["text"] = "I am a first level controller";
			return View();
		}
	}
}