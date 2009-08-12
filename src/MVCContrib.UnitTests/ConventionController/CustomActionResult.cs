using System.Web.Mvc;

namespace MvcContrib.UnitTests.ConventionController
{
	public class CustomActionResult : ActionResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			((TestController)context.Controller).CustomActionResultCalled = true;
		}
	}
}