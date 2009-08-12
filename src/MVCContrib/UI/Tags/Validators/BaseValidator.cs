using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

namespace MvcContrib.UI.Tags.Validators
{
	public abstract class BaseValidator : Element, IValidator
	{
		private Hash<string> _expandoAttributes = new Hash<string>();
		private bool isValid = true;

		public BaseValidator(string id, string referenceId, string text)
			: base("span")
		{
			Id = id;
			ReferenceId = referenceId;
			InnerText = text;
			NullSet("style", "display:none;color:red;");
		}

		public BaseValidator(string id, string referenceId, string text, IDictionary attributes)
			: base("span", attributes)
		{
			Id = id;
			ReferenceId = referenceId;
			InnerText = text;
			NullSet("style", "display:none;color:red;");
		}

		public BaseValidator(string id, string referenceId, string text, string validationGroup)
			: base("span")
		{
			Id = id;
			ReferenceId = referenceId;
			ValidationGroup = validationGroup;
			InnerText = text;
			NullSet("style", "display:none;color:red;");
		}

		public BaseValidator(string id, string referenceId, string text, string validationGroup, IDictionary attributes)
			: base("span", attributes)
		{
			Id = id;
			ReferenceId = referenceId;
			ValidationGroup = validationGroup;
			InnerText = text;
			NullSet("style", "display:none;color:red;");
		}

		public string ReferenceId
		{
			get
			{
				return NullExpandoGet("controltovalidate");
			}

			set
			{
				NullExpandoSet("controltovalidate", value);
			}
		}

		public string ErrorMessage
		{
			get
			{
				return NullExpandoGet("errormessage");
			}

			set
			{
				NullExpandoSet("errormessage", value);
			}
		}

		public string ValidationGroup
		{
			get
			{
				return NullExpandoGet("validationGroup");
			}

			set
			{
				NullExpandoSet("validationGroup", value);
			}
		}

		public abstract string ValidationFunction
		{
			get;
		}

		public bool IsValid
		{
			get
			{
				return isValid;
			}

			protected set
			{
				isValid = value;
				if (!value)
					NullSet("style", "color:red;");
			}
		}

		public virtual void RenderClientHookup(StringBuilder output)
		{
			string name = Id.Replace('.', '_');

			NullExpandoSet("evaluationfunction", ValidationFunction);
			NullExpandoSet("display", "Dynamic");

			// field declaration
			output.Append("var ");
			output.Append(name);
			output.Append(" = document.all ? document.all[\"");
			output.Append(Id);
			output.Append("\"] : document.getElementById(\"");
			output.Append(Id);
			output.Append("\");");
			output.AppendLine();

			Dictionary<string, string>.Enumerator en = _expandoAttributes.GetEnumerator();
			while (en.MoveNext())
			{
				output.Append(name);
				output.Append(".");
				output.Append(en.Current.Key);
				output.Append(" = \"");
				output.Append(en.Current.Value.Replace("\\", "\\\\"));
				output.Append("\";");
				output.AppendLine();
			}
		}

		public virtual bool Validate(HttpRequestBase request)
		{
			throw new NotImplementedException();
		}

		public static bool Validate(HttpRequestBase request, ICollection<IValidator> validators)
		{
			bool valid = true;
			foreach (var validator in validators)
			{
				bool tempValid = validator.Validate(request);
				if (valid && !tempValid)
					valid = false;
			}

			return valid;
		}

		protected string NullExpandoGet(string key)
		{
			if (_expandoAttributes.ContainsKey(key))
			{
				return _expandoAttributes[key];
			}

			return null;
		}

		protected void NullExpandoSet(string key, string value)
		{
			_expandoAttributes[key] = value;
		}
	}
}
