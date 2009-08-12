using System.Collections;
namespace MvcContrib.UI.Tags
{
	public class Input : ScriptableElement
	{
		private const string ON_FOCUS = "onfocus";
		private const string ON_BLUR = "onblur";
		private const string ON_SELECT = "onselect";
		private const string ON_CHANGE = "onchange";
		private const string READ_ONLY = "readonly";
		private const string DISABLED = "disabled";
		private const string VALUE = "value";
		private const string NAME = "name";
		private const string LABEL = "label";

		private string _label;

		public Input(string type, IDictionary attributes) : base("input", attributes)
		{
			Type = type;
			//Label is not an HTML attribute, so remove it and store separately.
			if (Attributes.Contains(LABEL))
			{
				_label = attributes[LABEL].ToString();
				Attributes.Remove(LABEL);
			}
		}

		public string Label
		{
			get { return _label; }
			set { _label = value; }
		}

		public string Type
		{
			get { return NullGet("type"); }
			set { this["type"] = value; }
		}

		public object Value
		{
			get { return NullGet(VALUE); }
			set {
				if (value != null)
					NullSet(VALUE, value.ToString());
				else
					NullSet(VALUE, null);
			}
		}

		public string Name
		{
			get { return NullGet(NAME); }
			set { NullSet(NAME,value); }
		}

		public string OnFocus
		{
			get { return NullGet(ON_FOCUS); }
			set { NullSet(ON_FOCUS, value); }
		}

		public string OnBlur
		{
			get { return NullGet(ON_BLUR); }
			set { NullSet(ON_BLUR, value); }
		}

		public string OnSelect
		{
			get { return NullGet(ON_SELECT); }
			set { NullSet(ON_SELECT, value); }
		}

		public string OnChange
		{
			get { return NullGet(ON_CHANGE); }
			set { NullSet(ON_CHANGE, value); }
		}

		public bool Disabled
		{
			get { return NullGet(DISABLED) == DISABLED; }
			set
			{
				if (value)
					NullSet(DISABLED, DISABLED);
				else
					NullSet(DISABLED, null);
			}
		}
		public bool ReadOnly
		{
			get { return NullGet(READ_ONLY) == READ_ONLY; }
			set
			{
				if (value)
					NullSet(READ_ONLY, READ_ONLY);
				else
					NullSet(READ_ONLY, null);
			}
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(_label) && !string.IsNullOrEmpty(Id))
			{
				var label = new Element(LABEL, new Hash(@for => Id)) {InnerText = _label};
				return base.ToString() + label;
			}
			return base.ToString();
		}
	}
}
