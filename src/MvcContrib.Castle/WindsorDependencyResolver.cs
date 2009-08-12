using System;
using Castle.Windsor;
using MvcContrib.Interfaces;

namespace MvcContrib.Castle
{
	public class WindsorDependencyResolver : IDependencyResolver
	{
		private IWindsorContainer _container;

		public WindsorDependencyResolver(IWindsorContainer container)
		{
			_container = container;
		}

		public WindsorDependencyResolver()
		{
			_container = new WindsorContainer();
		}

		public IWindsorContainer Container
		{
			get { return _container; }
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
			if( _container.Kernel.HasComponent(type))
			{
				return _container.Resolve(type);
			}

			return null;
		}

		public void DisposeImplementation(object instance)
		{
			_container.Release(instance);	
		}
	}
}