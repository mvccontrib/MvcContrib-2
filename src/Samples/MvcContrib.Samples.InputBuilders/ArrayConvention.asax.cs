using System;
using System.Collections;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Views;
using Web.Models;

namespace MvcContrib.UI.InputBuilder
{
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