using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core;
using Castle.Windsor;
using MvcContrib.Samples.WindsorControllerFactory.Controllers;
using MvcContrib.Samples.WindsorControllerFactory.Models;
using MvcContrib.Castle;

namespace MvcContrib.Samples.WindsorControllerFactory
{
	public class Global : HttpApplication, IContainerAccessor
	{
		private static WindsorContainer _container;
		
		public static IWindsorContainer Container
		{
			get { return _container; }
		}

		IWindsorContainer IContainerAccessor.Container
		{
			get { return Container; }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			InitializeWindsor();
			AddRoutes();
		}

		/// <summary>
		/// Instantiate the container and add all Controllers that derive from 
		/// WindsorController to the container.  Also associate the Controller 
		/// with the WindsorContainer ControllerFactory.
		/// </summary>
		protected virtual void InitializeWindsor()
		{
			if (_container == null)
			{
				_container = new WindsorContainer();

				ControllerBuilder.Current.SetControllerFactory(new MvcContrib.Castle.WindsorControllerFactory(Container));
				
				_container
					.RegisterControllers(typeof(HomeController).Assembly)
					.AddComponent<IService, Service>();
			}
		}

		protected virtual void AddRoutes()
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
