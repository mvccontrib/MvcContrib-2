using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class RadioList: InputElementList<RadioField>
	{
		public RadioList(IDictionary attributes) : base(attributes)
		{
		}
		public RadioList() : this(Hash.Empty)
		{
		}
	}
}