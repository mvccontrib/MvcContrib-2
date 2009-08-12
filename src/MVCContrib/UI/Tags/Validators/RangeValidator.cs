using System;
using System.Web.UI.WebControls;
using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public class RangeValidator : BaseCompareValidator
	{
		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text)
			: base(id, referenceId, text, type)
		{
			ValidateInput(minimumValue, maximumValue, type);
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
		}

		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text, IDictionary attributes)
			: base(id, referenceId, text, type, attributes)
		{
			ValidateInput(minimumValue, maximumValue, type);
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
		}

		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text, string validationGroup)
			: base(id, referenceId, text, type, validationGroup)
		{
			ValidateInput(minimumValue, maximumValue, type);
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
		}

		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, type, validationGroup, attributes)
		{
			ValidateInput(minimumValue, maximumValue, type);
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
		}

		public string MinimumValue
		{
			get
			{
				return NullExpandoGet("minimumvalue");
			}

			set
			{
				NullExpandoSet("minimumvalue", value);
			}
		}

		public string MaximumValue
		{
			get
			{
				return NullExpandoGet("maximumvalue");
			}

			set
			{
				NullExpandoSet("maximumvalue", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "RangeValidatorEvaluateIsValid";
			}
		}

		private void ValidateInput(string minimumValue, string maximumValue, ValidationDataType type)
		{
			if (type == ValidationDataType.Date || type == ValidationDataType.Currency)
			{
				throw new ArgumentException("The RangeValidator currently does not support Date or Currency types.");
			}

			if (string.IsNullOrEmpty(minimumValue) || !System.Web.UI.WebControls.BaseCompareValidator.CanConvert(minimumValue, type, true))
			{
				throw new ArgumentException("Cannot convert value to selected validation type.", "minimumValue");
			}

			if (string.IsNullOrEmpty(maximumValue) || !System.Web.UI.WebControls.BaseCompareValidator.CanConvert(maximumValue, type, true))
			{
				throw new ArgumentException("Cannot convert value to selected validation type.", "maximumValue");
			}
		}

		public override bool Validate(System.Web.HttpRequestBase request)
		{
			string formValue = request.Form[ReferenceId];

			if (formValue == null)
			{
				IsValid = false;
				return false;
			}

			IsValid = (CompareValues(formValue, MinimumValue, ValidationCompareOperator.GreaterThanEqual) && CompareValues(formValue, MaximumValue, ValidationCompareOperator.LessThanEqual));
			return IsValid;
		}
	}
}
