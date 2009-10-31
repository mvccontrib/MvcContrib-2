using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.ActionResults;

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

		/// <summary>
		/// Loads parameters from TempData into the ActionParameters dictionary.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			LoadParameterValuesFromTempData(filterContext);
		}

		/// <summary>
		/// Stores any parameters passed to the generic RedirectToAction method in TempData.
		/// </summary>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var result = filterContext.Result as IControllerExpressionContainer;

			if(result != null && result.Expression != null)
			{
				AddParameterValuesFromExpressionToTempData(filterContext.Controller.TempData, result.Expression);
			}
		}

		// Copied this method from Microsoft.Web.Mvc.dll (MVC Futures)...
		// Microsoft.Web.Mvc.Internal.ExpresisonHelper.AddParameterValuesFromExpressionToDictionary().
		// The only change I made is saving the parameter values to TempData instead
		// of a RouteValueDictionary.
		private void AddParameterValuesFromExpressionToTempData(TempDataDictionary tempData, MethodCallExpression call)
		{
			ParameterInfo[] parameters = call.Method.GetParameters();

			if(parameters.Length > 0)
			{
				for(int i = 0; i < parameters.Length; i++)
				{
					Expression expression = call.Arguments[i];
					object obj2 = null;
					ConstantExpression expression2 = expression as ConstantExpression;
					if(expression2 != null)
					{
						obj2 = expression2.Value;
					}
					else
					{
						Expression<Func<object>> expression3 = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)), new ParameterExpression[0]);
						obj2 = expression3.Compile()();
					}

					tempData[RedirectParameterPrefix + parameters[i].Name] = obj2;
				}
			}
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