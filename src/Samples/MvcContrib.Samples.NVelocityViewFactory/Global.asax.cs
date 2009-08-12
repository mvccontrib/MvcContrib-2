using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MvcContrib.Binders;
using MvcContrib.Castle;

namespace MvcContrib.Samples.NVelocityViewFactory
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
			System.Web.Mvc.ViewEngines.Engines.Add(new MvcContrib.ViewEngines.NVelocityViewEngine());
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
                ModelBinders.Binders.DefaultBinder = new SubControllerBinder();

				_container = new WindsorContainer();

				// Add our singleton NVelocityViewFactory
				Type[] assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

				_container.RegisterControllers(assemblyTypes);
				ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container));
			}
		}

		protected virtual void AddRoutes()
		{
            //For IIS6
            //RouteTable.Routes.Add(new Route("{controller}.mvc/{action}/{id}", new MvcRouteHandler())
            //{
            //    Defaults = new RouteValueDictionary(new { action = "Index", id = "" }),
            //});
            
            RouteTable.Routes.Add(new Route("{controller}/{action}/{id}", new MvcRouteHandler())
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
