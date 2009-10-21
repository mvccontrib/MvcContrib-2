using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public class DefaultConventions //: IPropertyViewModelConventions
	{
		public virtual PropertyViewModel ModelPropertyBuilder(PropertyInfo propertyInfo, object value)
		{
			if(propertyInfo.PropertyType.IsEnum)
			{
				SelectListItem[] selectList = Enum.GetNames(propertyInfo.PropertyType).Select(
					s => new SelectListItem {Text = s, Value = s, Selected = s == value.ToString()}).ToArray();

				return new PropertyViewModel<IEnumerable<SelectListItem>> {Value = selectList};
			}
			if(propertyInfo.PropertyType == typeof(DateTime))
			{
				return new PropertyViewModel<DateTime> {Value = (DateTime)value};
			}

			return new PropertyViewModel<object> {Value = value};
		}
	}
}