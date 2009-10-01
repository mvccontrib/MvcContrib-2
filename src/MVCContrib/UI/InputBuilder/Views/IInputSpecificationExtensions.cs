using MvcContrib.UI.InputBuilder.InputSpecification;

namespace MvcContrib.UI.InputBuilder.Views
{
	public static class IInputSpecificationExtensions
	{
		public static IInputSpecification<TypeViewModel> Partial<T>(this IInputSpecification<T> inputSpecification, string partialViewName) where T : TypeViewModel
		{
			inputSpecification.Model.PartialName = partialViewName;
			return (IInputSpecification<TypeViewModel>)inputSpecification;
		}

		public static IInputSpecification<PropertyViewModel> Example(this IInputSpecification<PropertyViewModel> inputSpecification, string example)
		{
			inputSpecification.Model.Example = example;
			return inputSpecification;
		}

		public static IInputSpecification<TypeViewModel> Label<T>(this IInputSpecification<T> inputSpecification, string label) where T :TypeViewModel
		{
			inputSpecification.Model.Label = label;
			return (IInputSpecification<TypeViewModel>)inputSpecification;
		}

		public static IInputSpecification<PropertyViewModel> Required(this IInputSpecification<PropertyViewModel> inputSpecification)
		{
			inputSpecification.Model.PropertyIsRequired = true;
			return inputSpecification;
		}
	}
}