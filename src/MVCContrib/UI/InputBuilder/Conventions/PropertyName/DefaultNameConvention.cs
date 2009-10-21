using System.Reflection;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public class DefaultNameConvention : IPropertyViewModelNameConvention
	{
		public virtual string PropertyName(PropertyInfo propertyInfo)
		{
			return propertyInfo.Name;
		}
	}
}