using System.Collections;

namespace MvcContrib.UI
{
	public class ScriptableElement : Element, IScriptableElement
    {
        private const string ON_CLICK = "onclick";
		private const string ON_DBL_CLICK = "ondblclick";
		private const string ON_MOUSE_DOWN = "onmousedown";
		private const string ON_MOUSE_UP = "onmouseup";
		private const string ON_MOUSE_OVER = "onmouseover";
		private const string ON_MOUSE_MOVE = "onmousemove";
		private const string ON_MOUSE_OUT = "onmouseout";
		private const string ON_KEY_PRESS = "onkeypress";
		private const string ON_KEY_DOWN = "onkeydown";
		private const string ON_KEY_UP = "onkeyup";

		public virtual bool UseInlineScripts { get; set; }

		public ScriptableElement()
            : base("Button", Hash.Empty)
        {
        }

		public ScriptableElement(string tag) : base(tag, Hash.Empty)
		{
		}

        public ScriptableElement(string tag, IDictionary attributes):base(tag,attributes)
		{
		}

        public string OnClick
        {
            get { return NullGet(ON_CLICK); }
            set { NullSet(ON_CLICK, value); }
		}
		public string OnDblClick
		{
			get { return NullGet(ON_DBL_CLICK); }
			set { NullSet(ON_DBL_CLICK, value); }
		}
		public string OnMouseDown
		{
			get { return NullGet(ON_MOUSE_DOWN); }
			set { NullSet(ON_MOUSE_DOWN, value); }
		}
		public string OnMouseUp
		{
			get { return NullGet(ON_MOUSE_UP); }
			set { NullSet(ON_MOUSE_UP, value); }
		}
		public string OnMouseOver
		{
			get { return NullGet(ON_MOUSE_OVER); }
			set { NullSet(ON_MOUSE_OVER, value); }
		}
		public string OnMouseMove
		{
			get { return NullGet(ON_MOUSE_MOVE); }
			set { NullSet(ON_MOUSE_MOVE, value); }
		}
		public string OnMouseOut
		{
			get { return NullGet(ON_MOUSE_OUT); }
			set { NullSet(ON_MOUSE_OUT, value); }
		}
		public string OnKeyPress
		{
			get { return NullGet(ON_KEY_PRESS); }
			set { NullSet(ON_KEY_PRESS, value); }
		}
		public string OnKeyDown
		{
			get { return NullGet(ON_KEY_DOWN); }
			set { NullSet(ON_KEY_DOWN, value); }
		}
		public string OnKeyUp
		{
			get { return NullGet(ON_KEY_UP); }
			set { NullSet(ON_KEY_UP, value); }
		}

    }
}
