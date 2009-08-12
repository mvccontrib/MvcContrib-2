using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class Form: ScriptableElement
	{
		public const string METHOD = "method";
		public const string ACTION = "action";
		public const string ENCTYPE = "enctype";
		public const string ON_SUBMIT = "onsubmit";
		public const string ON_RESET = "onreset";
		public const string MULTIPART_ENCODE = "multipart/form-data";

		public enum FORM_METHOD
		{
			GET,
			POST
		}

		public Form():base("form")
		{
		}
		public Form(string action)
			: base("form")
		{
			NullSet(ACTION, action);
		}
		public Form(string action, FORM_METHOD method)
			: base("form")
		{
			NullSet(ACTION, action);
			NullSet(METHOD, method.ToString().ToLower());
		}
		public Form(string action, FORM_METHOD method, IDictionary attributes)
			: base("form", attributes)
		{
			NullSet(METHOD, method.ToString().ToLower());
			NullSet(ACTION, action);
		}

		public bool IsMultiPart
		{
			get { return NullGet(ENCTYPE) == MULTIPART_ENCODE; }
			set
			{
				if (value)
				{
					NullSet(ENCTYPE, MULTIPART_ENCODE);
				}
				else
				{
					NullSet(ENCTYPE, null);
				}
			}
		}

		public string OnSubmit
		{
			get { return NullGet(ON_SUBMIT); }
			set { NullSet(ON_SUBMIT, value); }
		}
		public string OnReset
		{
			get { return NullGet(ON_RESET); }
			set { NullSet(ON_RESET, value); }
		}
		public string Action
		{
			get { return NullGet(ACTION); }
			set { NullSet(ACTION, value); }
		}
		public FORM_METHOD Method
		{
			get { 
				switch (NullGet(METHOD))
				{
					case "get":
						return FORM_METHOD.GET;
					case "post":
						return FORM_METHOD.POST;
				}
				return FORM_METHOD.GET;
			}
			set
			{
				NullSet(METHOD, value.ToString().ToLower());
			}
		}

		public override bool UseFullCloseTag
		{
			get
			{
				return true;
			}
		}
	}
}
