using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Unity;
using MvcContrib.Samples.UnityControllerFactory.Controllers;
using MvcContrib.Samples.UnityControllerFactory.Models;
using Microsoft.Practices.Unity;

namespace MvcContrib.Samples.UnityControllerFactory
{
	public class Global : HttpApplication, IUnityContainerAccessor
	{
		private static UnityContainer _container;

        public static IUnityContainer Container
		{
			get { return _container; }
		}

        IUnityContainer IUnityContainerAccessor.Container
		{
			get { return Container; }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			InitializeContainer();
			AddRoutes();
		}

		/// <summary>
		/// Instantiate the container and add all Controllers that derive from 
        /// UnityController to the container.  Also associate the Controller 
        /// with the UnityContainer ControllerFactory.
		/// </summary>
        protected virtual void InitializeContainer()
		{
			if (_container == null)
			{
				_container = new UnityContainer();
				_container.RegisterType(typeof(IService), typeof(Service));
				ControllerBuilder.Current.SetControllerFactory(typeof(MvcContrib.Unity.UnityControllerFactory));

                Type[] assemblyTypes = typeof(HomeController).Assembly.GetTypes();
				foreach (Type type in assemblyTypes)
				{
					if(typeof(IController).IsAssignableFrom(type) )
					{
						_container.RegisterType(type, type);
					}
				}
			}
		}

		protected virtual void AddRoutes()
		{
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
