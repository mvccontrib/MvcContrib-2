using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Attributes;

namespace MvcContrib.UI.InputBuilder
{
    public class DefaultConventions : IModelPropertyConventions
    {
        public virtual InputModelProperty ModelPropertyBuilder(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo.PropertyType.IsEnum)
            {

                var selectList = Enum.GetNames(propertyInfo.PropertyType).Select(
                    s => new SelectListItem() { Text = s, Value = s, Selected = s == value.ToString() }).ToArray();

                return new ModelProperty<IEnumerable<SelectListItem>> { Value = selectList };

            }
            if (propertyInfo.PropertyType==typeof(DateTime) )
            {
                return new ModelProperty<DateTime> {Value = (DateTime) value};
            }

            return new ModelProperty<object> { Value = value };
        }

        public virtual string PartialNameConvention(PropertyInfo propertyInfo)
        {
            if (propertyInfo.AttributeExists<UIHintAttribute>())
                return propertyInfo.GetAttribute<UIHintAttribute>().UIHint;

            if (propertyInfo.AttributeExists<DataTypeAttribute>())
                return propertyInfo.GetAttribute<DataTypeAttribute>().DataType.ToString();        
            
            if (propertyInfo.PropertyType.IsEnum)
                return typeof(Enum).Name;

            return propertyInfo.PropertyType.Name ;

        }

    	public virtual string ExampleForPropertyConvention(PropertyInfo propertyInfo)
        {
            
            if (propertyInfo.AttributeExists<ExampleAttribute>())
            {
                return propertyInfo.GetAttribute<ExampleAttribute>().Example;
            }
            return string.Empty;
            
        }

		public virtual string LabelForPropertyConvention(PropertyInfo propertyInfo)
        {
            if (propertyInfo.AttributeExists<LabelAttribute>())
            {
                return propertyInfo.GetAttribute<LabelAttribute>().Label;                
            }
            return propertyInfo.Name.ToSeparatedWords();            
        }

        public virtual string PropertyNameConvention(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

		public virtual bool PropertyIsRequiredConvention(PropertyInfo propertyInfo)
        {
            return propertyInfo.AttributeExists<RequiredAttribute>();
        }

        public string Layout(string partialName)
        {
            if (partialName.ToLower().Equals("guid"))
                return "HiddenField";
            return "Field";
        }

        public virtual Type PropertyTypeConvention(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }

		public virtual bool ModelIsInvalidConvention<T>(PropertyInfo propertyInfo, HtmlHelper<T> htmlHelper) where T : class
        {
            return htmlHelper.ViewData.ModelState.ContainsKey(propertyInfo.Name) &&
                   htmlHelper.ViewData.ModelState[propertyInfo.Name].Errors.Count > 0;
        }

		public virtual object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model)
        {
            var value = propertyInfo.GetValue(model, new object[0]);
            if (value == null)
                return string.Empty;
            return value;
            
        }
    }
}