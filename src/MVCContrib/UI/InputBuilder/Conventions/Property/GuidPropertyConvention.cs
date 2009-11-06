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
		public override string Layout(PropertyInfo info, bool indexed)
		{
			return "HiddenField";
		}
		public override string PartialNameConvention(PropertyInfo propertyInfo, bool indexed)
		{
			return "Guid";
		}
	}
}