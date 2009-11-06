using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public class EnumPropertyConvention : DefaultProperyConvention
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType.IsEnum;
		}

		public override PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type)
		{
			object value = base.ValueFromModelPropertyConvention(propertyInfo, model, name);

			SelectListItem[] selectListItems = Enum.GetNames(propertyInfo.PropertyType).Select(
				s => new SelectListItem {Text = s, Value = s, Selected = s == value.ToString()}).ToArray();

			PropertyViewModel viewModel = base.Create(propertyInfo, model, name, type);
			viewModel.Value = selectListItems;
			viewModel.PartialName = "Enum";

			return viewModel;
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<IEnumerable<SelectListItem>> {};
		}
	}
}