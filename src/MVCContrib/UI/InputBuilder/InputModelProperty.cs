using System;

namespace MvcContrib.UI.InputBuilder
{
    public class InputModelProperty:InputTypeProperty
    {
        public string Name { get; set; }

        public bool PropertyIsRequired { get; set; }

        public string Example { get; set; }

        public bool HasValidationMessages { get; set; }

        public bool HasExample()
        {
            return !string.IsNullOrEmpty(Example);
        }
		public virtual object Value { get; set; }
    }
}