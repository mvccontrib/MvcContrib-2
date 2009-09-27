namespace MvcContrib.UI.InputBuilder
{
    public static class IInputSpecificationExtensions
    {
        public static IInputSpecification Partial(this IInputSpecification inputSpecification, string partialViewName)
        {
            inputSpecification.Model.PartialName = partialViewName;
            return inputSpecification;
        }

		public static IInputPropertySpecification Example(this IInputPropertySpecification inputSpecification, string example)
        {
            inputSpecification.Model.Example = example;
            return inputSpecification;
        }

        public static IInputSpecification Label(this IInputSpecification inputSpecification, string label)
        {
            inputSpecification.Model.Label = label;
            return inputSpecification;
        }

		public static IInputPropertySpecification Required(this IInputPropertySpecification inputSpecification)
        {
            inputSpecification.Model.PropertyIsRequired = true;
            return inputSpecification;
        }
    
    }
}