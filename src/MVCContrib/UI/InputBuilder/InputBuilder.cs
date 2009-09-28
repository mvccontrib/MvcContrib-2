using System;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
	public class InputBuilder
	{
		private static Func<IModelPropertyConventions> _conventionProvider = () => new DefaultConventions();
		public static Action<VirtualPathProvider> RegisterPathProvider = HostingEnvironment.RegisterVirtualPathProvider;

		public static IModelPropertyConventions Conventions
		{
			get { return _conventionProvider(); }
		}

		public static void BootStrap()
		{
			VirtualPathProvider pathProvider = new AssemblyResourceProvider();

			RegisterPathProvider(pathProvider);

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new InputBuilderViewEngine(new[] {"{1}", "Shared"}));
		}

		public static void SetConventionProvider(Func<IModelPropertyConventions> conventionProvider)
		{
			_conventionProvider = conventionProvider;
		}
	}
}