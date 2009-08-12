using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Web;

namespace MvcContrib.UI.Tags.Validators
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class CompareValidator : BaseCompareValidator
	{
		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text)
			: base(id, referenceId, text, type)
		{
			OperatorType = operatorType;
			CompareReferenceId = compareReferenceId;
		}

		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text, IDictionary attributes)
			: base(id, referenceId, text, type, attributes)
		{
			OperatorType = operatorType;
			CompareReferenceId = compareReferenceId;
		}

		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text, string validationGroup)
			: base(id, referenceId, text, type, validationGroup)
		{
			OperatorType = operatorType;
			CompareReferenceId = compareReferenceId;
		}

		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, type, validationGroup, attributes)
		{
			OperatorType = operatorType;
			CompareReferenceId = compareReferenceId;
		}

		public ValidationCompareOperator OperatorType
		{
			get
			{
				string value = NullExpandoGet("operator");
				return (ValidationCompareOperator)PropertyConverter.EnumFromString(typeof(ValidationCompareOperator), value);
			}

			set
			{
				NullExpandoSet("operator", PropertyConverter.EnumToString(typeof(ValidationCompareOperator), value));
			}
		}

		public string CompareReferenceId
		{
			get
			{
				return NullExpandoGet("controltocompare");
			}

			set
			{
				NullExpandoSet("controltocompare", value);
				NullExpandoSet("controlhookup", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "CompareValidatorEvaluateIsValid";
			}
		}

		public override bool Validate(HttpRequestBase request)
		{
			string formValue1 = request.Form[ReferenceId];
			string formValue2 = request.Form[CompareReferenceId];

			if (formValue1 == null || (formValue2 == null && OperatorType != ValidationCompareOperator.DataTypeCheck))
			{
				IsValid = false;
				return false;
			}

			IsValid = CompareValues(formValue1, formValue2, OperatorType);

			return IsValid;
		}
	}
}
