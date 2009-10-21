using System;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public class GuidPropertyConvention : DefaultProperyConvention, IPropertyViewModelFactory
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType.IsAssignableFrom(typeof(Guid));
		}

		public override PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name)
		{
			PropertyViewModel result = base.Create(propertyInfo, model, name);
			result.Layout = "HiddenField";
			return result;
		}
	}
}