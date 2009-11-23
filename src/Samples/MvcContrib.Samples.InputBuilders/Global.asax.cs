using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.UI.InputBuilder
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new {controller = "Home", action = "Index", id = ""} // Parameter defaults
				);
		}

		private void Application_Error(object sender, EventArgs e)
		{
			// handle generic application errors
			Exception ex = Server.GetLastError();
			Trace.WriteLine(ex);
		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);

			//ModelBinders.Binders.DefaultBinder = new Microsoft.Web.Mvc.DataAnnotations.DataAnnotationsModelBinder();

			InputBuilder.BootStrap();
		}
	}
}