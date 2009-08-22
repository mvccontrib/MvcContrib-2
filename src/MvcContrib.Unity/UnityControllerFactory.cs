using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace MvcContrib.Unity
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            Type type = GetControllerType(context, controllerName);

            if (type == null)
            {
                throw new InvalidOperationException(string.Format("Could not find a controller with the name {0}", controllerName));
            }

            IUnityContainer container = GetContainer(context);
            return (IController)container.Resolve(type);
        }

        protected virtual IUnityContainer GetContainer(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var unityContainerAccessor = context.HttpContext.ApplicationInstance as IUnityContainerAccessor;
            if (unityContainerAccessor == null)
            {
                throw new InvalidOperationException(
                    "You must extend the HttpApplication in your web project and implement the IContainerAccessor to properly expose your container instance");
            }

            IUnityContainer container = unityContainerAccessor.Container;
            if (container == null)
            {
                throw new InvalidOperationException("The container seems to be unavailable in your HttpApplication subclass");
            }

            return container;
        }
    }
}
