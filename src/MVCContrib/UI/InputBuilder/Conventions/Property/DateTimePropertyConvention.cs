using System;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public class DateTimePropertyConvention : DefaultProperyConvention
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType == typeof(DateTime);
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<DateTime> {};
		}
	}
}