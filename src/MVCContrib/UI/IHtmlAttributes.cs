using System;
using System.Collections;
using System.Collections.Generic;
namespace MvcContrib.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public interface IHtmlAttributes: IDictionary<string, string>, IDictionary
	{
		new int Count { get; }
		new IEnumerator<KeyValuePair<string, string>> GetEnumerator();
		int GetEstLength();
	}
}
