using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Views;
using Web.Models;

namespace MvcContrib.UI.InputBuilder
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new {controller = "Home", action = "Index", id = ""} // Parameter defaults
				);
		}

		private void Application_Error(object sender, EventArgs e)
		{
			// handle generic application errors
			Exception ex = Server.GetLastError();
			Trace.WriteLine(ex);
		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);

			//ModelBinders.Binders.DefaultBinder = new Microsoft.Web.Mvc.DataAnnotations.DataAnnotationsModelBinder();

			InputBuilder.BootStrap();
			InputBuilder.SetPropertyConvention(() => new MyConventions());
		}
	}

	public class MyConventions : DefaultConventionsFactory
	{
		public MyConventions():base()
		{
			Insert(0,new ArrayConvention());
			
		}
	}
public static class TypeViewModelExtensions
{
	public static bool HasDeleteButton(this TypeViewModel model)
	{
		return !(model.AdditionalValues.ContainsKey(ArrayConvention.HIDE_DELETE_BUTTON) && (bool)model.AdditionalValues[ArrayConvention.HIDE_DELETE_BUTTON]);
		
	}
	public static bool HasAddButton(this TypeViewModel model)
	{
		return !(model.AdditionalValues.ContainsKey(ArrayConvention.HIDE_ADD_BUTTON) && (bool)model.AdditionalValues[ArrayConvention.HIDE_ADD_BUTTON]);

	}
}
	public class ArrayConvention : ArrayPropertyConvention
	{
		public const string HIDE_ADD_BUTTON = "hideaddbutton";
		public const string HIDE_DELETE_BUTTON = "hidedeletebutton";
		public const string CAN_DELETE_ALL = "candeleteall";

		public override PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type)
		{
			PropertyViewModel value = base.Create(propertyInfo, model, name, type);
			if (propertyInfo.AttributeExists<NoAddAttribute>())
			{
				value.AdditionalValues.Add(HIDE_ADD_BUTTON, true);
			}
			if (propertyInfo.AttributeExists<CanDeleteAllAttribute>())
			{
				value.AdditionalValues.Add(CAN_DELETE_ALL, true);
			}
			if (propertyInfo.AttributeExists<NoDeleteAttribute>())
			{
				value.AdditionalValues.Add(HIDE_DELETE_BUTTON, true);
				foreach (TypeViewModel typeViewModel in (IEnumerable)value.Value)
				{
					typeViewModel.AdditionalValues.Add(HIDE_DELETE_BUTTON, true);
				}
			}
			return value;
		}
	}
}