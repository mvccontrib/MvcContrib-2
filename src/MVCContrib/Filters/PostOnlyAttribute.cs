using System;
using System.Web.Mvc;

namespace MvcContrib.Filters
{
	[Obsolete("Use MvcContrib.Attributes.AcceptPostAttribute instead.")]
	public class PostOnlyAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string actionName = filterContext.RouteData.GetRequiredString("action");
			if (filterContext.HttpContext.Request.RequestType != "POST")
				throw new InvalidOperationException(
					string.Format("Action '{0}' can only be accessed using an HTTP Post.", actionName));
		}
	}
}
