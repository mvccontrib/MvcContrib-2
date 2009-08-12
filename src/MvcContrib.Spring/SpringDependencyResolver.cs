using System;
using MvcContrib.Interfaces;
using Spring.Objects.Factory;

namespace MvcContrib.Spring
{
	public class SpringDependencyResolver : IDependencyResolver
	{
		private readonly IObjectFactory _factory;

		public SpringDependencyResolver(IObjectFactory factory)
		{
			_factory = factory;
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
				return _factory.GetObject(type.Name);
			}
			catch(NoSuchObjectDefinitionException)
			{
				return null;
			}
		}

		public void DisposeImplementation(object instance)
		{
		}
	}
}