using System.Web.Mvc;

namespace MvcContrib.UnitTests.ConventionController
{
	public class TestFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			((TestController)filterContext.Controller).BinderFilterOrdering += "Filter";
		}
	}
}