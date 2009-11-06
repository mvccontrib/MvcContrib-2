using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Attributes;
using MvcContrib.UI.InputBuilder.Helpers;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public class DefaultProperyConvention : IPropertyViewModelFactory
	{
		public virtual bool CanHandle(PropertyInfo propertyInfo)
		{
			return true;
		}

		public virtual PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type)
		{
			PropertyViewModel viewModel = CreateViewModel<object>();
			viewModel.PartialName = PartialNameConvention(propertyInfo);
			viewModel.Type = type;
			viewModel.Example = ExampleForPropertyConvention(propertyInfo);
			viewModel.Label = LabelForPropertyConvention(propertyInfo);
			viewModel.PropertyIsRequired = PropertyIsRequiredConvention(propertyInfo);
			viewModel.Layout = Layout(propertyInfo);
			viewModel.Value = ValueFromModelPropertyConvention(propertyInfo, model, name);
			viewModel.Name = name;
			return viewModel;
		}

		public virtual string Layout(PropertyInfo propertyInfo)
		{
			return "Field";
		}

		public virtual PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<T> {};
		}

		public virtual string ExampleForPropertyConvention(PropertyInfo propertyInfo)
		{
			if(propertyInfo.AttributeExists<ExampleAttribute>())
			{
				return propertyInfo.GetAttribute<ExampleAttribute>().Example;
			}
			return string.Empty;
		}

		public virtual string LabelForPropertyConvention(PropertyInfo propertyInfo)
		{
			if(propertyInfo.AttributeExists<LabelAttribute>())
			{
				return propertyInfo.GetAttribute<LabelAttribute>().Label;
			}
			return propertyInfo.Name.ToSeparatedWords();
		}

		public virtual bool PropertyIsRequiredConvention(PropertyInfo propertyInfo)
		{
			return propertyInfo.AttributeExists<RequiredAttribute>();
		}

		public virtual bool ModelIsInvalidConvention<T>(PropertyInfo propertyInfo, HtmlHelper<T> htmlHelper) where T : class
		{
			return htmlHelper.ViewData.ModelState.ContainsKey(propertyInfo.Name) &&
			       htmlHelper.ViewData.ModelState[propertyInfo.Name].Errors.Count > 0;
		}

		public virtual object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model, string parentName)
		{
			if(model != null)
			{
				object value=null;
				try
				{
					value = propertyInfo.GetValue(model, new object[0]);
				}catch{}

				if(value != null)
				{
					return value;
				}
			}
			return string.Empty;
		}

		public virtual string PartialNameConvention(PropertyInfo propertyInfo)
		{
			if(propertyInfo.AttributeExists<UIHintAttribute>())
			{
				return propertyInfo.GetAttribute<UIHintAttribute>().UIHint;
			}

			if(propertyInfo.AttributeExists<DataTypeAttribute>())
			{
				return propertyInfo.GetAttribute<DataTypeAttribute>().DataType.ToString();
			}
			return propertyInfo.PropertyType.Name;
		}
	}
}