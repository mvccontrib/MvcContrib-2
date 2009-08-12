using System;
using System.Collections;
namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class Option : ScriptableElement
	{
		private const string DISABLED = "disabled";
		private const string SELECTED = "selected";

		private const string VALUE = "value";

		public Option(IDictionary attributes) : base("option", attributes)
		{
		}

		public Option() : this(Hash.Empty)
		{
		}

		public string Value
		{
			get { return NullGet(VALUE); }
			set { NullSet(VALUE, value); }
		}

		public bool Disabled
		{
			get { return NullGet(DISABLED) == DISABLED; }
			set
			{
				if (value)
					NullSet(DISABLED, DISABLED);
				else
					NullSet(DISABLED, null);
			}
		}

		public bool Selected
		{
			get { return NullGet(SELECTED) == SELECTED; }
			set
			{
				if (value)
					NullSet(SELECTED, SELECTED);
				else
					NullSet(SELECTED, null);
			}
		}
	}
}