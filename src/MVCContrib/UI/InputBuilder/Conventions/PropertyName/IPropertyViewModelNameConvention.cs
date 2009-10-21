using System.Reflection;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelNameConvention
	{
		string PropertyName(PropertyInfo propertyInfo);
	}
}