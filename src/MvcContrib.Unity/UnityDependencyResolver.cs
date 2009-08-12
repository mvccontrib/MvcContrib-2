using System;
using Microsoft.Practices.Unity;
using MvcContrib.Interfaces;

namespace MvcContrib.Unity
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer container;

        public UnityDependencyResolver([Dependency] IUnityContainer container)
        {
            this.container = container;
        }

        public UnityDependencyResolver()
        {
            container = new UnityContainer();
        }

        public IUnityContainer Container
        {
            get { return container; }
        }

        public Interface GetImplementationOf<Interface>()
        {
            return (Interface)GetImplementationOf(typeof(Interface));
        }

        public Interface GetImplementationOf<Interface>(Type type)
        {
            return (Interface)GetImplementationOf(type);
        }

        public object GetImplementationOf(Type type)
        {
            try
            {
                return Container.Resolve(type);
            }
            catch(ResolutionFailedException)
            {
                return null;
            }
        }

    	public void DisposeImplementation(object instance)
    	{
    	}
    }
}