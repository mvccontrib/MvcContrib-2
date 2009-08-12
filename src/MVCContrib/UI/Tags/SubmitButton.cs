using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class SubmitButton : Input
	{
		public SubmitButton(IDictionary attributes) : base("submit", attributes)
		{
		}

		public SubmitButton() : this(Hash.Empty)
		{
		}
	}
}