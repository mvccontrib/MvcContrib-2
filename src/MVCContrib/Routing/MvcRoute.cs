using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc;

namespace MvcContrib.Routing
{
	///<summary>
	/// Use this class to define url->controller action mappings
	///</summary>
	public class MvcRoute : Route
	{
		private MvcRoute(string url)
			: base(url, new MvcRouteHandler())
		{
			Constraints = new RouteValueDictionary();
			Defaults = new RouteValueDictionary();
		}

		///<summary>
		/// Fluent interface start method
		///</summary>
		///<param name="url">The url pattern</param>
		public static MvcRoute MappUrl(string url)
		{
			return new MvcRoute(url);
		}

		///<summary>
		/// Sets the default controller, action and parameters via a lambda method, you can also 
		/// specify additional defaults
		///</summary>
		///<param name="action">default action</param>
		///<param name="defaults">other default parameters</param>
		public MvcRoute ToDefaultAction<T>(Expression<Func<T, ActionResult>> action, object defaults) where T : IController
		{
			ToDefaultAction(action);

			foreach(var pair in new RouteValueDictionary(defaults))
			{
				Defaults.Add(pair.Key, pair.Value);
			}

			return this;
		}

		///<summary>
		/// Sets the default controller, action and parameters via a lambda method
		///</summary>
		///<param name="action"></param>
		public MvcRoute ToDefaultAction<T>(Expression<Func<T, ActionResult>> action) where T : IController
		{
			var body = action.Body as MethodCallExpression;

			if(body == null)
			{
				throw new ArgumentException("Expression must be a method call");
			}

			if(body.Object != action.Parameters[0])
			{
				throw new ArgumentException("Method call must target lambda argument");
			}

			string actionName = body.Method.Name;

			// check for ActionName attribute
			var attributes = body.Method.GetCustomAttributes(typeof(ActionNameAttribute), false);
			if(attributes.Length > 0)
			{
				var actionNameAttr = (ActionNameAttribute)attributes[0];
				actionName = actionNameAttr.Name;
			}

			string controllerName = typeof(T).Name;

			if(controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
			{
				controllerName = controllerName.Remove(controllerName.Length - 10, 10);
			}

			Defaults = LinkBuilder.BuildParameterValuesFromExpression(body) ?? new RouteValueDictionary();
			foreach(var pair in Defaults.Where(x => x.Value == null).ToList())
				Defaults.Remove(pair.Key);

			Defaults.Add("controller", controllerName);
			Defaults.Add("action", actionName);

			return this;
		}

		/// <summary>
		/// Add constraints to this route
		/// </summary>
		/// <param name="constraints">The constraints defined as an anonymous object</param>
		public MvcRoute WithConstraints(object constraints)
		{
			foreach(var pair in new RouteValueDictionary(constraints))
			{
				Constraints.Add(pair.Key, pair.Value);
			}

			return this;
		}

		/// <summary>
		/// Add defaults to this route, if you use the ToDefaultAction 
		/// method the controller and action default will be overwritten.
		/// </summary>
		/// <param name="defaults">The defaults defined as an anonymous object</param>
		public MvcRoute WithDefaults(object defaults)
		{
			foreach(var pair in new RouteValueDictionary(defaults))
			{
				Defaults.Add(pair.Key, pair.Value);
			}

			return this;
		}

		///<summary>
		/// Sets the list of namespaces to look for controllers
		///</summary>
		///<param name="namespaces">An array of namespaces</param>
		public MvcRoute WithNamespaces(string[] namespaces)
		{
			if(namespaces == null)
			{
				throw new ArgumentNullException("namespaces");
			}

			DataTokens = new RouteValueDictionary();
			DataTokens["Namespaces"] = namespaces;

			return this;
		}

		///<summary>
		/// Add this route to the route collection with the specified name
		///</summary>
		///<param name="routeName">The name of the route</param>
		///<param name="routes">The route collection to add the route to</param>
		public MvcRoute AddWithName(string routeName, RouteCollection routes)
		{
			routes.Add(routeName, this);
			return this;
		}
	}
}