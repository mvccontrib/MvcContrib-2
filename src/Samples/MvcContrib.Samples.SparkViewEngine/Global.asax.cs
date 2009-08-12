using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.Samples.SparkViewEngine
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterViewEngine(ViewEngineCollection engines)
		{
			var spark = new MvcContrib.SparkViewEngine.SparkViewFactory();
			engines.Insert(0, spark);
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			RegisterViewEngine(ViewEngines.Engines);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}
