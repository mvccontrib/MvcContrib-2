using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class CheckBoxList : InputElementList<CheckBoxField>
	{
		public CheckBoxList(IDictionary attributes) : base(attributes)
		{
		}
		public CheckBoxList() : this(Hash.Empty)
		{
		}
	}
}
