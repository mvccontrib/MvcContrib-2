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
		public InputTypeProperty Create()
		{
			InputTypeProperty model = new ModelType<T>(){Value = _htmlHelper.ViewData.Model};
			model.PartialName = _conventions.PartialNameForTypeConvention(typeof(T));
			model.Label = _conventions.LabelForTypeConvention(typeof(T));
			model.Type = typeof(T);// _conventions.PropertyTypeConvention(propertyInfo);
			model.Layout = _conventions.Layout(model.PartialName);		
			return model;
		}
	}

	public class ModelType<T> : InputTypeProperty
	{
		public T Value { get; set; }
	}

	public class InputTypeProperty 
	{
		public InputModelProperty[] Properties { get; set; }
	
		public string PartialName { get; set; }

		public string Label { get; set; }

		public Type Type { get; set; }

		public string Layout { get; set; }
	}
}
