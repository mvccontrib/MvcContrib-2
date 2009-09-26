using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Demo.Site.Controllers;
using MvcContrib;
using MvcContrib.ControllerFactories;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using MvcContrib.Interfaces;
using MvcContrib.Services;

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
			DependencyResolver.InitializeWith(new QnDDepResolver(httpContextProvider, includeHandlingSettings, controllers));
			ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory());
		}
	}

	public class QnDDepResolver : IDependencyResolver
	{
		private readonly IDictionary<Type, object> types;

		public QnDDepResolver(IHttpContextProvider httpContextProvider, IIncludeHandlingSettings settings, Controller[] controllers)
		{
			types = new Dictionary<Type, object>
			{
				{ typeof (IHttpContextProvider), httpContextProvider },
				{ typeof (IKeyGenerator), new KeyGenerator() },
				{ typeof (IIncludeHandlingSettings), settings }
			};
			types.Add(typeof(IIncludeReader), new FileSystemIncludeReader((IHttpContextProvider)types[typeof(IHttpContextProvider)]));

			var keyGen = (IKeyGenerator)types[typeof(IKeyGenerator)];

			types.Add(typeof(IIncludeStorage), new StaticIncludeStorage(keyGen));

			var includeReader = (IIncludeReader)types[typeof(IIncludeReader)];
			var storage = (IIncludeStorage)types[typeof(IIncludeStorage)];
			var combiner = new IncludeCombiner(settings, includeReader, storage, (IHttpContextProvider)types[typeof(IHttpContextProvider)]);
			types.Add(typeof(IIncludeCombiner), combiner);

			types.Add(typeof(IncludeController), new IncludeController(settings, combiner));
			foreach (var controller in controllers)
			{
				types.Add(controller.GetType(), controller);
			}
		}

		public Interface GetImplementationOf<Interface>()
		{
			return (Interface)types[typeof(Interface)];
		}

		public Interface GetImplementationOf<Interface>(Type type)
		{
			return (Interface)types[type];
		}

		public object GetImplementationOf(Type type)
		{
			return types[type];
		}

		public void DisposeImplementation(object instance)
		{
			
		}
	}

}