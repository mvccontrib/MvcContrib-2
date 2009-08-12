using System;
using System.Collections;

namespace MvcContrib.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public interface IElement:IEnumerable
	{
		IHtmlAttributes Attributes { get; }
		string Class { get; set; }
		bool EscapeInnerText { get; set; }
		string Id { get; set; }
		string InnerText { get; set; }
		DomQuery Selector { get; set; }
		string Tag { get; set; }
		string this[string attributeName] { get; set; }
		string ToString();
		bool UseFullCloseTag { get; }
	}
}
