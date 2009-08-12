using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.Services;
using MvcContrib.StructureMap;
using StructureMap;
using Website.Controllers;
using Website.Models;

namespace Website
{
    public class GlobalApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Note: Change the URL to "{controller}.mvc/{action}/{id}" to enable
            //       automatic support on IIS6 and IIS7 classic mode

            routes.Add(new Route("{controller}/{action}/{id}", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { action = "Index", id = "" }),
            });

            routes.Add(new Route("Default.aspx", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "Home", action = "Index", id = "" }),
            });
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            ConfigureIoC();
        }

        private void ConfigureIoC()
        {
            StructureMapConfiguration.UseDefaultStructureMapConfigFile = false;
            StructureMapConfiguration.BuildInstancesOf<HomeController>().TheDefaultIsConcreteType<HomeController>();
			StructureMapConfiguration.BuildInstancesOf<ILinkRepository>().TheDefaultIsConcreteType<InMemoryLinkRepository>();
            DependencyResolver.InitializeWith(new StructureMapDependencyResolver());
            ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory());
        }

    }
}