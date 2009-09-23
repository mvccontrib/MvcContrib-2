using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
    public class InputModelPropertyFactory<T> where T : class
    {
        private readonly HtmlHelper<T> _htmlHelper;
    	private readonly IModelPropertyConventions _conventions;

    	public InputModelPropertyFactory(HtmlHelper<T> htmlHelper, IModelPropertyConventions conventions)
        {
            _htmlHelper = htmlHelper;
			_conventions = conventions;
        }

		public InputModelProperty Create(PropertyInfo propertyInfo)
        {
            object value = _conventions.ValueFromModelPropertyConvention(propertyInfo, _htmlHelper.ViewData.Model);
            InputModelProperty model = _conventions.ModelPropertyBuilder(propertyInfo, value);
            
            model.PropertyIsRequired = _conventions.PropertyIsRequiredConvention(propertyInfo);
            model.PartialName = _conventions.PartialNameConvention(propertyInfo);
            model.Name = _conventions.PropertyNameConvention(propertyInfo);
            model.Label = _conventions.LabelForPropertyConvention(propertyInfo);
            model.Example = _conventions.ExampleForPropertyConvention(propertyInfo);
            model.HasValidationMessages = _conventions.ModelIsInvalidConvention(propertyInfo, _htmlHelper);
            model.Type = _conventions.PropertyTypeConvention(propertyInfo);
            model.Layout= _conventions.Layout(model.PartialName);

            return model;
        }
    }
}
