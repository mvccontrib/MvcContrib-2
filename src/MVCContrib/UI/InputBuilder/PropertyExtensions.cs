using System.Linq;
using System.Reflection;

namespace MvcContrib.UI.InputBuilder
{
	public static class PropertyExtensions
	{
		public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : class
		{
			var attribute = propertyInfo.GetCustomAttributes(typeof(T), false)
			                	.FirstOrDefault() as T;
			if(attribute == null)
			{
				return false;
			}
			return true;
		}

		public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : class
		{
			return propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
		}
	}
}