using System;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Objects.Factory;

namespace MvcContrib.Spring
{
	/// <summary>
	/// Controller Factory implementation for Spring.net
	/// </summary>
	public class SpringControllerFactory : IControllerFactory
	{
		private static IObjectFactory _objectFactory;

		/// <summary>
		/// Configures the controller factory to use the 
		/// given spring.net IObjectFactory for controller lookup.
		/// If you call Configure multiple times, the last call will prevail.
		/// </summary>
		/// <param name="objectFactory">IObjectFactory instance to use for lookups.</param>
		public static void Configure(IObjectFactory objectFactory)
		{
			_objectFactory = objectFactory;
		}

		public IController CreateController(RequestContext context, string controllerName)
		{
			if(string.IsNullOrEmpty(controllerName))
			{
				throw new ArgumentNullException("controllerName");
			}

			controllerName = GetArea(context) + controllerName + "Controller";

			if(_objectFactory == null)
			{
				throw new ArgumentException("CreateController has been called before Configure.");
			}

			try
			{
				return (IController)_objectFactory.GetObject(controllerName);
			}
			catch(Exception e)
			{
				throw new InvalidOperationException("Failed creating instance of: " +
				                                    controllerName + " using spring.net object factory", e);
			}
		}

		private string GetArea(RequestContext context)
		{
			if(context.RouteData.DataTokens.ContainsKey("area"))
			{
				return context.RouteData.DataTokens["area"].ToString();
			}
			return string.Empty;
		}

		public void ReleaseController(IController controller)
		{
			var disposable = controller as IDisposable;

			if(disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}