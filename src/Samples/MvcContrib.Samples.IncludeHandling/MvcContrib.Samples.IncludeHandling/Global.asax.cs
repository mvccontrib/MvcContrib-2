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
			var controllers = new [] { typeof(HomeController), typeof(AccountController) };
			var includeHandlingSettings = (IIncludeHandlingSettings) ConfigurationManager.GetSection("includeHandling");
			DependencyResolver.InitializeWith(new QnDDepResolver(httpContextProvider, includeHandlingSettings, controllers));
			ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory());
		}
	}

	public class QnDDepResolver : IDependencyResolver
	{
		private readonly IDictionary<Type, Func<object>> types;

		public QnDDepResolver(IHttpContextProvider httpContextProvider, IIncludeHandlingSettings settings, Type[] controllers)
		{
			types = new Dictionary<Type, Func<object>>
			{
				{ typeof (IHttpContextProvider),() => httpContextProvider },
				{ typeof (IKeyGenerator), () => new KeyGenerator() },
				{ typeof (IIncludeHandlingSettings), () => settings }
			};
			types.Add(typeof(IIncludeReader), () => new FileSystemIncludeReader(GetImplementationOf<IHttpContextProvider>()));

			types.Add(typeof(IIncludeStorage), () => new StaticIncludeStorage(GetImplementationOf<IKeyGenerator>()));

			types.Add(typeof(IIncludeCombiner), () => new IncludeCombiner(settings, GetImplementationOf<IIncludeReader>(), GetImplementationOf<IIncludeStorage>(), GetImplementationOf<IHttpContextProvider>()));

			types.Add(typeof(IncludeController), () => new IncludeController(settings, GetImplementationOf<IIncludeCombiner>()));
			foreach (var controller in controllers)
			{
				var controllerType = controller;
				types.Add(controllerType, () => Activator.CreateInstance(controllerType));
			}
		}

		public Interface GetImplementationOf<Interface>()
		{
			return (Interface)types[typeof(Interface)]();
		}

		public Interface GetImplementationOf<Interface>(Type type)
		{
			return (Interface)types[type]();
		}

		public object GetImplementationOf(Type type)
		{
			return types[type]();
		}

		public void DisposeImplementation(object instance)
		{
			
		}
	}

}