using System.Web.Mvc;
using MvcContrib;

namespace MvcContrib.Filters
{
	///<summary>
	/// Takes the System.Action from GetResult of subcontrollers and adds it to ViewData using the key that is equal to the action parameter name for the subcontroller.
	///</summary>
	public class SubControllerActionToViewDataAttribute : ActionFilterAttribute
	{
		///<summary>
		/// Adds to ViewData before the Action executes.
		///</summary>
		///<param name="filterContext"></param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			foreach (var pair in filterContext.ActionParameters)
			{
				object value = pair.Value;
				if(value == null)
				{
					continue;
				}

				if (typeof(ISubController).IsAssignableFrom(value.GetType()))
				{
					var controller = (ISubController) value;
					filterContext.Controller.ViewData.Add(pair.Key, controller.GetResult(filterContext.Controller));
				}
			}

			base.OnActionExecuting(filterContext);
		}
	}
}