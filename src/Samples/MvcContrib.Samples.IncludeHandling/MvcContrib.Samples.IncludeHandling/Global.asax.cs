using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Demo.Site.Controllers;
using Microsoft.Practices.ServiceLocation;
using Nrws.Web;
using Nrws.Web.IncludeHandling.Configuration;

namespace Demo.Site
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
				new { controller = "Home", action = "Index", id = "" } // Parameter defaults
				);
		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
			var httpContextProvider = new HttpContextProvider(HttpContext.Current);
			var controllers = new Controller[] { new HomeController(), new AccountController() };
			var includeHandlingSettings = (IIncludeHandlingSettings) ConfigurationManager.GetSection("includeHandling");
			ServiceLocator.SetLocatorProvider(() => QnDServiceLocator.Create(httpContextProvider, includeHandlingSettings, controllers));
			ControllerBuilder.Current.SetControllerFactory(new CommonServiceLocatorControllerFactory());
		}
	}
}