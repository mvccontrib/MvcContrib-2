using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using System.Web;

namespace MvcContrib.ControllerFactories
{
	public class IoCControllerFactory : DefaultControllerFactory
	{
		private IDependencyResolver resolver;

		public IoCControllerFactory(IDependencyResolver resolver)
		{
			if(resolver == null)
				throw new ArgumentNullException("resolver");
			this.resolver = resolver;
		}

		public IoCControllerFactory()
		{
		}

		public override IController CreateController(RequestContext context, string controllerName)
		{
			if(controllerName == null)
			{
				throw new ArgumentNullException("controllerName");
			}

			Type controllerType = GetControllerType(context, controllerName);

			if(controllerType != null)
			{
				if(resolver != null)
				{
					return resolver.GetImplementationOf<IController>(controllerType);
				}
				else
				{
					return DependencyResolver.GetImplementationOf<IController>(controllerType);
				}
			}
			else
                throw new HttpException(404, string.Format("Could not find a type for the controller name '{0}'", controllerName));
		}

        public override void ReleaseController(IController controller)
		{
			var disposable = controller as IDisposable;

			if(disposable != null)
			{
				disposable.Dispose();	
			}

			if(resolver != null)
			{
				resolver.DisposeImplementation(controller);
			}
			else
			{
				DependencyResolver.DisposeImplementation(controller);
			}
		} 
	}
}
