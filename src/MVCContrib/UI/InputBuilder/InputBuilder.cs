using System;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
    public class InputBuilder
    {
        public static Action<System.Web.Hosting.VirtualPathProvider> RegisterPathProvider = HostingEnvironment.RegisterVirtualPathProvider;
		private static Func<IModelPropertyConventions> _conventionProvider = () => new DefaultConventions();

        public static void BootStrap()
        {
            VirtualPathProvider pathProvider = new AssemblyResourceProvider();
            
            RegisterPathProvider(pathProvider);            

            ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new InputBuilderViewEngine(new string[] { "{1}", "Shared" }));        		
        }

		public static void SetConventionProvider(Func<IModelPropertyConventions> conventionProvider)
		{
			_conventionProvider = conventionProvider;
		}

    	public static IModelPropertyConventions Conventions
    	{
			get { return _conventionProvider(); }
    	}
    }
}