using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class HiddenField : Input
	{
		public HiddenField(IDictionary attributes) : base("hidden", attributes)
		{
		}

		public HiddenField() : this(Hash.Empty)
		{
		}
	}
}