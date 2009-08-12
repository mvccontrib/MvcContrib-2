namespace MvcContrib.Interfaces
{
	using System;

	public interface IDependencyResolver
	{
		Interface GetImplementationOf<Interface>();
		Interface GetImplementationOf<Interface>(Type type);
		object GetImplementationOf(Type type);
		void DisposeImplementation(object instance);
	}
}