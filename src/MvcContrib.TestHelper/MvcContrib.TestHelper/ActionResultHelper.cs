using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.Filters;

namespace MvcContrib.TestHelper
{
	/// <summary>
	/// Contains extension methods for testing action results.
	/// </summary>
	public static class ActionResultHelper
	{
		/// <summary>
		/// Asserts that the ActionResult is of the specified type.
		/// </summary>
		/// <typeparam name="T">Type of action result to convert to.</typeparam>
		/// <param name="result">Action Result to convert.</param>
		/// <returns></returns>
		public static T AssertResultIs<T>(this ActionResult result) where T : ActionResult
		{
			var converted = result as T;

			if(converted == null)
			{
				throw new ActionResultAssertionException(string.Format("Expected result to be of type {0}. It is actually of type {1}.", typeof(T).Name, result.GetType().Name));
			}

			return converted;
		}

		/// <summary>
		/// Asserts that the action result is a RenderViewResult.
		/// </summary>
		/// <param name="result">The result to convert.</param>
		/// <returns></returns>
		public static ViewResult AssertViewRendered(this ActionResult result)
		{
			return result.AssertResultIs<ViewResult>();
		}

		/// <summary>
		/// Asserts that the action result is a RenderPartialViewResult
		/// </summary>
		/// <param name="result">The result to convert</param>
		/// <returns></returns>
		public static PartialViewResult AssertPartialViewRendered(this ActionResult result)
		{
			return result.AssertResultIs<PartialViewResult>();
		}

		/// <summary>
		/// Asserts that the action result is a HttpRedirectResult.
		/// </summary>
		/// <param name="result">The result to convert.</param>
		/// <returns></returns>
		public static RedirectResult AssertHttpRedirect(this ActionResult result)
		{
			return result.AssertResultIs<RedirectResult>();
		}

		/// <summary>
		/// Asserts that the action result is an ActionRedirectResult.
		/// </summary>
		/// <param name="result">The result to convert.</param>
		/// <returns></returns>
		public static RedirectToRouteResult AssertActionRedirect(this ActionResult result)
		{
			return result.AssertResultIs<RedirectToRouteResult>();
		}

		/// <summary>
		/// Asserts that an ActionRedirectResult is for the specified controller.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <returns></returns>
		public static RedirectToRouteResult ToController(this RedirectToRouteResult result, string controller)
		{
			return result.WithParameter("controller", controller);
		}

		/// <summary>
		/// Asserts that an ActionRedirectReslt is for the specified action.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="action">The name of the action.</param>
		/// <returns></returns>
		public static RedirectToRouteResult ToAction(this RedirectToRouteResult result, string action)
		{
			return result.WithParameter("action", action);

		}

		/// <summary>
		/// Asserts that an ActionRedirectResult is for the specified action on the specified controller
		/// </summary>
		/// <typeparam name="TController">The type of the controller.</typeparam>
		/// <param name="result">The result to check.</param>
		/// <param name="action">The action to call on the controller.</param>
		/// <returns></returns>
		public static RedirectToRouteResult ToAction<TController>(this RedirectToRouteResult result, Expression<Action<TController>> action) where TController : IController
		{
			var methodCall = (MethodCallExpression)action.Body;
			string actionName = methodCall.Method.Name;

			const string ControllerSuffix = "Controller";
			var controllerTypeName = typeof(TController).Name;
			if (controllerTypeName.EndsWith(ControllerSuffix, StringComparison.OrdinalIgnoreCase))
			{
				controllerTypeName = controllerTypeName.Substring(0, controllerTypeName.Length - ControllerSuffix.Length);
			}
			return result.ToController(controllerTypeName).ToAction(actionName);
		}

		/// <summary>
		/// Asserts that an ActionRedirectResult contains a specified value in its RouteValueCollection.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="paramName">The name of the parameter to check for.</param>
		/// <param name="value">The expected value.</param>
		/// <returns></returns>
		public static RedirectToRouteResult WithParameter(this RedirectToRouteResult result, string paramName, object value)
		{
			if(!result.RouteValues.ContainsKey(paramName))
			{
				throw new ActionResultAssertionException(string.Format("Could not find a parameter named '{0}' in the result's Values collection.", paramName));
			}

			var paramValue = result.RouteValues[paramName];

			if(!paramValue.Equals(value))
			{
				throw new ActionResultAssertionException(string.Format("When looking for a parameter named '{0}', expected '{1}' but was '{2}'.", paramName, value, paramValue));
			}

			return result;
		}

		/// <summary>
		/// Gets a parameter from a RedirectToRouteResult.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="controller">The controller that you redirected FROM.</param>
		/// <param name="paramName">The name of the parameter to check for.</param>
		/// <returns></returns>
		public static object GetStronglyTypedParameter(this RedirectToRouteResult result, Controller controller,
		                                               string paramName)
		{
			if(result.RouteValues.ContainsKey(paramName))
			{
				return result.RouteValues[paramName];
			}

			if(controller.TempData.ContainsKey(PassParametersDuringRedirectAttribute.RedirectParameterPrefix + paramName))
			{
				return controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + paramName];
			}

			throw new ActionResultAssertionException(
				string.Format("Could not find a parameter named '{0}' in the result's Values collection.", paramName));
		}

		/// <summary>
		/// Asserts that a RenderViewResult is rendering the specified view.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="viewName">The name of the view.</param>
		/// <returns></returns>
		public static ViewResult ForView(this ViewResult result, string viewName)
		{
			if(result.ViewName != viewName)
			{
				throw new ActionResultAssertionException(string.Format("Expected view name '{0}', actual was '{1}'", viewName, result.ViewName));
			}
			return result;
		}

        /// <summary>
        /// Asserts that a RenderPartialViewResult is rendering the specified partial view
        /// </summary>
        /// <param name="result">The result to check</param>
        /// <param name="partialViewName">The name of the partial view</param>
        /// <returns></returns>
        public static PartialViewResult ForView(this PartialViewResult result, string partialViewName)
        {
            if(result.ViewName != partialViewName)
            {
                throw new ActionResultAssertionException(string.Format("Expected partial view name '{0}', actual was '{1}'", partialViewName, result.ViewName));
            }   

            return result;
        }

		/// <summary>
		/// Asserts that a HttpRedirectResult is redirecting to the specified URL.
		/// </summary>
		/// <param name="result">The result to check</param>
		/// <param name="url">The URL that the result should be redirecting to.</param>
		/// <returns></returns>
		public static RedirectResult ToUrl(this RedirectResult result, string url)
		{
			if(result.Url != url)
			{
				throw new ActionResultAssertionException(string.Format("Expected redirect to '{0}', actual was '{1}'", url, result.Url));
			}
			return result;
		}

        /// <summary>
        /// Asserts that a RenderViewResult's value has been set using a strongly typed value, returning that value if successful.
        /// If the type is a reference type, a view data set to null will be returned as null.
        /// If the type is a value type, a view data set to null will throw an exception.
        /// </summary>
        /// <typeparam name="TViewData">The custom type for the view data.</typeparam>
        /// <param name="actionResult">The result to check.</param>
        /// <returns>The ViewData in it's strongly-typed form.</returns>
        public static TViewData WithViewData<TViewData>(this PartialViewResult actionResult)
        {
            return AssertViewDataModelType<TViewData>(actionResult);
        }

	    /// <summary>
		/// Asserts that a RenderViewResult's value has been set using a strongly typed value, returning that value if successful.
		/// If the type is a reference type, a view data set to null will be returned as null.
		/// If the type is a value type, a view data set to null will throw an exception.
		/// </summary>
		/// <typeparam name="TViewData">The custom type for the view data.</typeparam>
		/// <param name="actionResult">The result to check.</param>
		/// <returns>The ViewData in it's strongly-typed form.</returns>
		public static TViewData WithViewData<TViewData>(this ViewResult actionResult)
		{
            return AssertViewDataModelType<TViewData>(actionResult);
		}

        private static TViewData AssertViewDataModelType<TViewData>(ViewResultBase actionResult)
        {
            var actualViewData = actionResult.ViewData.Model;
            var expectedType = typeof(TViewData);

            if (actualViewData == null && expectedType.IsValueType)
            {
                throw new ActionResultAssertionException(string.Format("Expected view data of type '{0}', actual was NULL",
                                                                       expectedType.Name));
            }

            if (actualViewData == null)
            {
                return (TViewData)actualViewData;
            }

            if (!typeof(TViewData).IsAssignableFrom(actualViewData.GetType()))
            {
                throw new ActionResultAssertionException(string.Format("Expected view data of type '{0}', actual was '{1}'",
                                                                       typeof(TViewData).Name, actualViewData.GetType().Name));
            }

            return (TViewData)actualViewData;
        }
	}
}
