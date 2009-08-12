using System.Collections;

namespace MvcContrib.UI
{
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
