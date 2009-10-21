using System;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelConventions
	{
		string ExampleForPropertyConvention(PropertyInfo propertyInfo);
		object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model);
		string LabelForPropertyConvention(PropertyInfo propertyInfo);
		bool ModelIsInvalidConvention<T>(PropertyInfo propertyInfo, HtmlHelper<T> htmlHelper) where T : class;
		Type PropertyTypeConvention(PropertyInfo propertyInfo);
		string PartialNameConvention(PropertyInfo propertyInfo);
		PropertyViewModel ModelPropertyBuilder(PropertyInfo propertyInfo, object model);
		bool PropertyIsRequiredConvention(PropertyInfo propertyInfo);
		string Layout(string partialName);
		string PartialNameForTypeConvention(Type type);
		string LabelForTypeConvention(Type type);
	}
}