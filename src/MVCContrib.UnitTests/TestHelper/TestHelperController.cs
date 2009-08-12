using System.Web.Mvc;

namespace MvcContrib.UnitTests.TestHelper
{
	public class TestHelperController : Controller
	{
		public ActionResult RedirectWithAction()
		{
			return RedirectToAction("ActionName1");
		}


		public ActionResult RedirectWithActionAndController()
		{
			return RedirectToAction("ActionName2", "ControllerName2");
		}


		public ActionResult RedirectWithObject()
		{
			return RedirectToAction("ActionName3", "ControllerName3", new {Id = 1});
		}


		public ActionResult RenderViewWithViewName()
		{
			return View("View1");
		}


		public ActionResult RenderViewWithViewNameAndData()
		{
			return View("View2", new {Prop1 = 1, Prop2 = 2});
		}


		public ActionResult RenderViewWithViewNameAndMaster()
		{
			return View("View3", "Master3");
		}


		public ActionResult RenderViewWithViewNameAndMasterAndData()
		{
			return View("View4", "Master4", new {Prop1 = 3, Prop2 = 4});
		}

		public virtual int RandomOtherFunction()
		{
			return 12345;
		}
	}
}