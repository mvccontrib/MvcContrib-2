using System;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;

namespace MvcContrib.UI.Tags.Validators
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class RegularExpressionValidator : BaseValidator
	{
		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text)
			: base(id, referenceId, text)
		{
			ValidationExpression = validationExpression;
		}

		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text, IDictionary attributes)
			: base(id, referenceId, text, attributes)
		{
			ValidationExpression = validationExpression;
		}

		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text, string validationGroup)
			: base(id, referenceId, text, validationGroup)
		{
			ValidationExpression = validationExpression;
		}

		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, validationGroup, attributes)
		{
			ValidationExpression = validationExpression;
		}

		public string ValidationExpression
		{
			get
			{
				return NullExpandoGet("validationexpression");
			}

			set
			{
				NullExpandoSet("validationexpression", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "RegularExpressionValidatorEvaluateIsValid";
			}
		}

		public override bool Validate(HttpRequestBase request)
		{
			var regex = new Regex(ValidationExpression, RegexOptions.Compiled);
			string value = request.Form[ReferenceId];

			if (value != null)
				IsValid = regex.IsMatch(value);
			else
				IsValid = false;

			return IsValid;
		}
	}
}
