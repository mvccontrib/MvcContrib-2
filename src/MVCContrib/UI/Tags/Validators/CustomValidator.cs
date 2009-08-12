using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public class CustomValidator : BaseValidator
	{
		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text)
			: base(id, referenceId, text)
		{
			ClientValidationFunction = clientValidationFunction;
		}

		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text, IDictionary attributes)
			: base(id, referenceId, text, attributes)
		{
			ClientValidationFunction = clientValidationFunction;
		}

		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text, string validationGroup)
			: base(id, referenceId, text, validationGroup)
		{
			ClientValidationFunction = clientValidationFunction;
		}

		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, validationGroup, attributes)
		{
			ClientValidationFunction = clientValidationFunction;
		}

		public string ClientValidationFunction
		{
			get
			{
				return NullExpandoGet("clientvalidationfunction");
			}

			set
			{
				NullExpandoSet("clientvalidationfunction", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "CustomValidatorEvaluateIsValid";
			}
		}
	}
}
