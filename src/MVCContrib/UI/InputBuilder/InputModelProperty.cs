using System;

namespace MvcContrib.UI.InputBuilder
{
    public class InputModelProperty
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string PartialName { get; set; }

        public bool PropertyIsRequired { get; set; }

        public string Example { get; set; }

        public bool HasValidationMessages { get; set; }

        public bool HasExample()
        {
            return !string.IsNullOrEmpty(Example);
        }
    }
}