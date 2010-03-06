using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.ActionResults;

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
	}
}