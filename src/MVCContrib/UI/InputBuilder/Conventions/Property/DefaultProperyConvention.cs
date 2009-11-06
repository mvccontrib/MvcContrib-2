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

		public virtual PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, bool indexed, Type type, IViewModelFactory factory)
		{
			PropertyViewModel viewModel = CreateViewModel<object>();
			viewModel.PartialName = PartialNameConvention(propertyInfo, indexed);
			viewModel.Type = type;
			viewModel.Example = ExampleForPropertyConvention(propertyInfo);
			viewModel.Label = LabelForPropertyConvention(propertyInfo);
			viewModel.PropertyIsRequired = PropertyIsRequiredConvention(propertyInfo);
			viewModel.Layout = Layout(propertyInfo, indexed);
			viewModel.Value = ValueFromModelPropertyConvention(propertyInfo, model, name,factory);
			viewModel.Name = name;
			return viewModel;
		}

		public virtual string Layout(PropertyInfo propertyInfo, bool indexed)
		{
			if (propertyInfo.PropertyType.IsArray&& !indexed)
			{
				return "Array";
			}

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

		public virtual object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model, string parentName, IViewModelFactory factory)
		{
			if(model != null)
			{
				object value = propertyInfo.GetValue(model, new object[0]);
				if(value != null)
				{
					return value;
				}
			}
			return string.Empty;
		}

		public virtual string PartialNameConvention(PropertyInfo propertyInfo, bool indexed)
		{
			if(propertyInfo.AttributeExists<UIHintAttribute>())
			{
				return propertyInfo.GetAttribute<UIHintAttribute>().UIHint;
			}

			if(propertyInfo.AttributeExists<DataTypeAttribute>())
			{
				return propertyInfo.GetAttribute<DataTypeAttribute>().DataType.ToString();
			}
			if(propertyInfo.PropertyType.IsArray)
			{
				if(indexed)
				{
					return propertyInfo.PropertyType.Name.Replace("[]","");
				}
				return "Array";
			}
			return propertyInfo.PropertyType.Name;
		}
	}
}