using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class InputButton : Input
	{
		public InputButton(IDictionary attributes) : base("button", attributes)
		{
		}

		public InputButton()
			: this(Hash.Empty)
		{
		}

	}
}