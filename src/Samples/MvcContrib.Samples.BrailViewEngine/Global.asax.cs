using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core;
using Castle.Windsor;
using MvcContrib.Castle;
using MvcContrib.ControllerFactories;
using MvcContrib.ViewFactories;

namespace MvcContrib.Samples
{
    public class Global : HttpApplication, IContainerAccessor
    {
        private static WindsorContainer _container;

        public static IWindsorContainer Container
        {
            get { return _container; }
        }

        #region IContainerAccessor Members

        IWindsorContainer IContainerAccessor.Container
        {
            get { return Container; }
        }

        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            InitializeWindsor();
            AddRoutes();
        	ViewEngines.Engines.Add( new BrailViewFactory() );  
        }

        /// <summary>
        /// Instantiate the container and add all Controllers that derive from 
        /// WindsorController to the container.  Also associate the Controller 
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeWindsor()
        {
            if(_container == null)
            {
                _container = new WindsorContainer();

                _container.AddComponent("ViewFactory", typeof(IViewEngine), typeof(BrailViewFactory));
				ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container));

                Type[] assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

                foreach(Type type in assemblyTypes)
                {
                    if(typeof(IController).IsAssignableFrom(type))
                    {
                        _container.AddComponentLifeStyle(type.Name.ToLower(), type, LifestyleType.Transient);
                    }
                }
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
				Defaults = new RouteValueDictionary(new { controller="Home", action = "Index", id = "" }),
			});
        }
    }
}
