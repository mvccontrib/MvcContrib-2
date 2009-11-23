using System;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.ViewEngine;

namespace MvcContrib.UI.InputBuilder
{
	public class InputBuilder
	{
		private static Func<IList<IPropertyViewModelFactory>> _propertyConventionProvider = () => new DefaultPropertyConventionsFactory();
		private static Func<IList<ITypeViewModelFactory>> _typeConventionProvider = () => new DefaultTypeConventionsFactory();
		public static Action<VirtualPathProvider> RegisterPathProvider = HostingEnvironment.RegisterVirtualPathProvider;

		public static IList<IPropertyViewModelFactory> Conventions
		{
			get { return _propertyConventionProvider(); }
		}

		public static IList<ITypeViewModelFactory> TypeConventions
		{
			get { return _typeConventionProvider(); }
		}

		public static void BootStrap()
		{
			VirtualPathProvider pathProvider = new AssemblyResourceProvider();

			RegisterPathProvider(pathProvider);

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new InputBuilderViewEngine(new[] {"{1}", "Shared"}));
		}

		public static void SetPropertyConvention(Func<IList<IPropertyViewModelFactory>> conventionProvider)
		{
			_propertyConventionProvider = conventionProvider;
		}
		public static void SetTypeConventions(Func<IList<ITypeViewModelFactory>> conventionProvider)
		{
			_typeConventionProvider = conventionProvider;
		}
	}
}