namespace MvcContrib.Services
{
	using System;
	using Interfaces;

	public static class DependencyResolver
	{
		private static IDependencyResolver resolver;

		public static void InitializeWith(IDependencyResolver resolver)
		{
			DependencyResolver.resolver = resolver;
		}

		public static IDependencyResolver Resolver
		{
			get { return resolver; }
		}

		public static T GetImplementationOf<T>()
		{
			return (T)GetImplementationOf(typeof(T));
		}

		public static T GetImplementationOf<T>(Type type)
		{
			return (T)GetImplementationOf(type);
		}

		public static object GetImplementationOf(Type type)
		{
			if(resolver != null)
			{
				object instance = resolver.GetImplementationOf(type);
				if(instance != null)
				{
					return instance;
				}
			}

			try
			{
				return Activator.CreateInstance(type);
			}
			catch(Exception exc)
			{
				throw new InvalidOperationException(string.Format("Could not create instance of type '{0}'", type.Name), exc);
			}
		}
	}
}