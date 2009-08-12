using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <summary>
	/// When placed on a controller or action, this attribute will ensure
	/// that parameters passed into RedirectToAction&lt;T&gt;() will get
	/// passed to the controller or action that this attribute is placed on.
	/// </summary>
	public class PassParametersDuringRedirectAttribute : ActionFilterAttribute
	{
		public const string RedirectParameterPrefix = "__RedirectParameter__";

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			LoadParameterValuesFromTempData(filterContext);
		}

		private void LoadParameterValuesFromTempData(ActionExecutingContext filterContext)
		{
			foreach(var parameterValue in GetStoredParameterValues(filterContext))
			{
				filterContext.ActionParameters[GetParameterName(parameterValue.Key)] = parameterValue.Value;
			}
		}

		private string GetParameterName(string key)
		{
			if(key.StartsWith(RedirectParameterPrefix))
			{
				return key.Substring(RedirectParameterPrefix.Length);
			}
			return key;
		}

		private IList<KeyValuePair<string, object>> GetStoredParameterValues(ActionExecutingContext filterContext)
		{
			return filterContext.Controller.TempData.Where(td => td.Key.StartsWith(RedirectParameterPrefix)).ToList();
		}
	}
}