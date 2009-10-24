using System;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.ViewEngine;

namespace MvcContrib.PortableAreas
{
	public abstract class PortableAreaRegistration:AreaRegistration
	{
		public abstract void RegisterArea(AreaRegistrationContext context,IApplicationBus bus);
		
		public override void RegisterArea(AreaRegistrationContext context)
		{
			RegisterArea(context,PortableArea.Bus);
		}

		public virtual void RegisterTheViewsInTheEmbeddedViewEngine( Type areaRegistrationType)
		{
			AssemblyResourceProvider.AddResource(
				new AssemblyResource()
			{
				VirtualPath = GetVirtualPath(AreaName), 
				TypeToLocateAssembly = areaRegistrationType,
				Namespace = GetNamespace(areaRegistrationType)
			});
		}

		public string GetNamespace(Type type)
		{
			return type.Namespace+".";
		}

		public string GetVirtualPath(string name)
		{
			return "/areas/"+name.ToLower();
		}
	}
}