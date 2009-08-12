using System;

namespace MvcContrib.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public interface IScriptableElement:IElement
	{
		string OnClick { get; set; }
		string OnDblClick { get; set; }
		string OnKeyDown { get; set; }
		string OnKeyPress { get; set; }
		string OnKeyUp { get; set; }
		string OnMouseDown { get; set; }
		string OnMouseMove { get; set; }
		string OnMouseOut { get; set; }
		string OnMouseOver { get; set; }
		string OnMouseUp { get; set; }
		bool UseInlineScripts { get; set; }
	}
}
