using System.ComponentModel.DataAnnotations;

namespace MvcContrib.UI.InputBuilder.Attributes
{
	public class ExampleAttribute : ValidationAttribute
	{
		public ExampleAttribute(string example)
		{
			Example = example;
		}

		public string Example { get; set; }

		public override bool IsValid(object value)
		{
			return true;
		}
	}
}