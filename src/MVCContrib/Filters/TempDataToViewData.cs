using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <summary>
	/// Action filter that copies the contents of TempData into ViewData if a ViewResult is returned by a controller action.
	/// </summary>
	public class TempDataToViewDataAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// If a ViewResult is returned by a Controller Action, the contents of the TempData dictionary is copied to the ViewData dictionary.
		/// </summary>
		/// <param name="filterContext">The ActionExecutedContext</param>
		public override void OnActionExecuted(ActionExecutedContext filterContext) 
		{
			if(! (filterContext.Result is ViewResult)) return;

			var tempData = filterContext.Controller.TempData;
			var viewData = filterContext.Controller.ViewData;

			foreach(var pair in tempData)
			{
				if(!viewData.ContainsKey(pair.Key))
				{
					viewData[pair.Key] = pair.Value;
				}
			}
		}
	}
}