using System;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.Services;
using MvcContrib.Spring;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;

namespace MvcContrib.Samples.IoC
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigureIoC();
            AddRoutes();
        }

        private void ConfigureIoC()
        {
            IResource input = new FileSystemResource(Server.MapPath("objects.xml"));
            IObjectFactory factory = new XmlObjectFactory(input);
            DependencyResolver.InitializeWith(new SpringDependencyResolver(factory));

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
