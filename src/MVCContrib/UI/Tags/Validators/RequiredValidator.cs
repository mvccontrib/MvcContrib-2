using System;
using System.Collections;
using System.Web;

namespace MvcContrib.UI.Tags.Validators
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class RequiredValidator : BaseValidator
	{
		public RequiredValidator(string id, string referenceId, string text)
			: base(id, referenceId, text)
		{
			InitialValue = string.Empty;
		}

		public RequiredValidator(string id, string referenceId, string text, IDictionary attributes)
			: base(id, referenceId, text, string.Empty, attributes)
		{
			InitialValue = string.Empty;
		}

		public RequiredValidator(string id, string referenceId, string text, string validationGroup)
			: base(id, referenceId, text, validationGroup)
		{
			InitialValue = string.Empty;
		}

		public RequiredValidator(string id, string referenceId, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, validationGroup, attributes)
		{
			InitialValue = string.Empty;
		}

		public override string ValidationFunction
		{
			get
			{
				return "RequiredFieldValidatorEvaluateIsValid";
			}
		}

		public string InitialValue
		{
			get
			{
				return NullExpandoGet("initialvalue");
			}

			set
			{
				NullExpandoSet("initialvalue", value);
			}
		}

		public override bool Validate(HttpRequestBase request)
		{
			IsValid = !string.IsNullOrEmpty(request.Form[ReferenceId]);

			return IsValid;
		}
	}
}
