using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Binders;

namespace Website
{
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

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
			ModelBinders.Binders.DefaultBinder = new SubControllerBinder();
		}
	}
}