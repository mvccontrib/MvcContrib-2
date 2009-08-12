namespace MvcContrib.UI
{
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
