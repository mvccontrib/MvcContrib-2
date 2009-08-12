using System;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.Samples.IoC.Controllers;
using MvcContrib.Services;
using MvcContrib.StructureMap;
using StructureMap;

namespace MvcContrib.Samples.IoC
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Note: Change Url= to Url="[controller].mvc/[action]/[id]" to enable 
            //       automatic support on IIS6 
            ConfigureIoC();
            AddRoutes();
        }

        private void ConfigureIoC()
        {
            StructureMapConfiguration.UseDefaultStructureMapConfigFile = false;
            StructureMapConfiguration.BuildInstancesOf<HomeController>().TheDefaultIsConcreteType<HomeController>();
            DependencyResolver.InitializeWith(new StructureMapDependencyResolver());
            ControllerBuilder.Current.SetControllerFactory(typeof(IoCControllerFactory));
        }

        private void AddRoutes()
        {
			RouteTable.Routes.Add(new Route("{controller}.mvc/{action}/{id}", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { action = "Index", id = "" }),
			});

			RouteTable.Routes.Add(new Route("Default.aspx", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { controller = "Home", action = "Index", id = "" }),
			});
        }
    }
}