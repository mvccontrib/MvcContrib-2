using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc.Internal;
using MvcContrib.ActionResults;
using MvcContrib.Filters;

namespace MvcContrib
{
	///<summary>
	/// Static class containing extension methods for controllers
	///</summary>
	public static class ControllerExtensions
	{
		/// <summary>
		/// Redirects to an action on the same controller using expression-based syntax
		/// </summary>
		/// <typeparam name="T">The type of the controller on which to call the action</typeparam>
		/// <param name="controller">The instance of the controller of type <typeparamref name="T"/> which provides access to this method</param>
		/// <param name="action">An expression which identifies the action to redirect to on the controller of type <typeparamref name="T"/></param>
		/// <returns>A <see cref="RedirectToRouteResult"/> pointing to the action specified by the <paramref name="action"/> expression</returns>
		public static RedirectToRouteResult RedirectToAction<T>(this T controller, Expression<Action<T>> action)
			where T : Controller
		{
			return ((Controller)controller).RedirectToAction(action);
		}

		/// <summary>
		/// Redirects to an action on the same or another controller using expression-based syntax
		/// </summary>
		/// <typeparam name="T">The type of the controller on which to call the action</typeparam>
		/// <param name="controller">The instance of the controller which provides access to this method</param>
		/// <param name="action">An expression which identifies the action to redirect to on the controller of type <typeparamref name="T"/></param>
		/// <returns>A <see cref="RedirectToRouteResult"/> pointing to the action specified by the <paramref name="action"/> expression</returns>
		public static RedirectToRouteResult RedirectToAction<T>(this Controller controller, Expression<Action<T>> action)
			where T : Controller
		{
			/*var body = action.Body as MethodCallExpression;
			AddParameterValuesFromExpressionToTempData(controller, body);
			var routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
			RemoveReferenceTypesFromRouteValues(routeValues);
			return new RedirectToRouteResult(routeValues);*/
			return new RedirectToRouteResult<T>(action);
		}

		/// <summary>
		/// Determines whether the specified type is a controller
		/// </summary>
		/// <param name="type">Type to check</param>
		/// <returns>True if type is a controller, otherwise false</returns>
		public static bool IsController(Type type)
		{
			return type != null
			       && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
			       && !type.IsAbstract
			       && typeof(IController).IsAssignableFrom(type);
		}


		// Copied this method from Microsoft.Web.Mvc.dll (MVC Futures)...
		// Microsoft.Web.Mvc.Internal.ExpresisonHelper.AddParameterValuesFromExpressionToDictionary().
		// The only change I made is saving the parameter values to TempData instead
		// of a RouteValueDictionary.
		private static void AddParameterValuesFromExpressionToTempData(Controller controller, MethodCallExpression call)
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
						Expression<Func<object>> expression3 =
							Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)), new ParameterExpression[0]);
						obj2 = expression3.Compile()();
					}
					controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + parameters[i].Name] = obj2;
				}
			}
		}
	}
}