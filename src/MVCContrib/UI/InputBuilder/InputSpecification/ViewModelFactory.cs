using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.InputSpecification
{
	public class ViewModelFactory<T> where T : class
	{
		private readonly IModelPropertyConventions _conventions;
		private readonly HtmlHelper<T> _htmlHelper;

		public ViewModelFactory(HtmlHelper<T> htmlHelper, IModelPropertyConventions conventions)
		{
			_htmlHelper = htmlHelper;
			_conventions = conventions;
		}

		public PropertyViewModel Create(PropertyInfo propertyInfo)
		{
			object value = _conventions.ValueFromModelPropertyConvention(propertyInfo, _htmlHelper.ViewData.Model);
			PropertyViewModel model = _conventions.ModelPropertyBuilder(propertyInfo, value);

			model.PropertyIsRequired = _conventions.PropertyIsRequiredConvention(propertyInfo);
			model.PartialName = _conventions.PartialNameConvention(propertyInfo);
			model.Name = _conventions.PropertyNameConvention(propertyInfo);
			model.Label = _conventions.LabelForPropertyConvention(propertyInfo);
			model.Example = _conventions.ExampleForPropertyConvention(propertyInfo);
			model.HasValidationMessages = _conventions.ModelIsInvalidConvention(propertyInfo, _htmlHelper);
			model.Type = _conventions.PropertyTypeConvention(propertyInfo);
			model.Layout = _conventions.Layout(model.PartialName);

			return model;
		}

		public TypeViewModel Create()
		{
			TypeViewModel model = new ModelType<T> {Value = _htmlHelper.ViewData.Model};
			model.PartialName = _conventions.PartialNameForTypeConvention(typeof(T));
			model.Label = _conventions.LabelForTypeConvention(typeof(T));
			model.Type = typeof(T); // _conventions.PropertyTypeConvention(propertyInfo);
			model.Layout = _conventions.Layout(model.PartialName);
			return model;
		}
	}
}