using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
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
            var redirectResult = filterContext.Result as RedirectToRouteResult;

            if(result != null && result.Expression != null && redirectResult != null)
            {
                var parsedParameters = AddParameterValuesFromExpressionToTempData(filterContext.Controller.TempData,
                                                                                  result.Expression);
                RemoveReferenceTypesFromRouteValues(redirectResult.RouteValues, parsedParameters);
            }
        }

        private void RemoveReferenceTypesFromRouteValues(RouteValueDictionary dictionary,
                                                         IDictionary<string, object> parameters)
        {
            var keysToRemove = parameters
                .Where(x => x.Value != null && !(x.Value is string || x.Value.GetType().IsSubclassOf(typeof(ValueType))))
                .Select(x => x.Key);

            foreach(var key in keysToRemove)
            {
                dictionary.Remove(key);
            }
        }

        // Copied this method from Microsoft.Web.Mvc.dll (MVC Futures)...
        // Microsoft.Web.Mvc.Internal.ExpresisonHelper.AddParameterValuesFromExpressionToDictionary().
        // The only change I made is saving the parameter values to TempData instead
        // of a RouteValueDictionary.
        private IDictionary<string, object> AddParameterValuesFromExpressionToTempData(TempDataDictionary tempData,
                                                                                       MethodCallExpression call)
        {
            ParameterInfo[] parameters = call.Method.GetParameters();
            var parsedParameters = new Dictionary<string, object>();

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
                        Expression<Func<object>> expression3 =
                            Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)),
                                                            new ParameterExpression[0]);
                        obj2 = expression3.Compile()();
                    }

                    tempData[RedirectParameterPrefix + parameters[i].Name] = obj2;
                    parsedParameters.Add(parameters[i].Name, obj2);
                }
            }

            return parsedParameters;
        }

        private void LoadParameterValuesFromTempData(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.GetParameters();

            foreach(var storedParameterValue in GetStoredParameterValues(filterContext))
            {
                if (storedParameterValue.Value == null)
                    continue;

                var storedParameterName = GetParameterName(storedParameterValue.Key);

                if(actionParameters.Any(actionParameter => actionParameter.ParameterName == storedParameterName &&
                                                           actionParameter.ParameterType.IsAssignableFrom(storedParameterValue.Value.GetType()))
                   && filterContext.ActionParameters.ContainsKey(storedParameterName) == false)
                {
                    filterContext.ActionParameters[storedParameterName] = storedParameterValue.Value;

                    filterContext.Controller.TempData.Keep(storedParameterValue.Key);
                }
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