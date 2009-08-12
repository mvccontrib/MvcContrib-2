using System.Collections;
namespace MvcContrib.UI.Tags
{
	public class TextArea : ScriptableElement
	{
		private const string ON_FOCUS = "onfocus";
		private const string ON_BLUR = "onblur";
		private const string ON_SELECT = "onselect";
		private const string ON_CHANGE = "onchange";
		private const string READ_ONLY = "readonly";
		private const string DISABLED = "disabled";
		private const string COLS = "cols";
		private const string ROWS = "rows";

		public TextArea(IDictionary attributes) : base("textarea", attributes)
		{
		}

		public TextArea() : this(Hash.Empty)
		{
		}

		public object Value
		{
			get
			{
				return InnerText;
			}
			set
			{
				if (value != null)
					InnerText = value.ToString();
				else
					InnerText = null;
			}
		}

		public string Name
		{
			get { return NullGet("name"); }
			set { Attributes["name"] = value; }
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

		public int Cols
		{
			get
			{
				if (NullGet(COLS) != null)
				{
					int val;
					if (int.TryParse(NullGet(COLS), out val))
					{
						return val;
					}
					else
					{
						NullSet(COLS, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(COLS, value.ToString());
				}
				else
				{
					NullSet(COLS, null);
				}
			}
		}

		public int Rows
		{
			get
			{
				if (NullGet(ROWS) != null)
				{
					int val;
					if (int.TryParse(NullGet(ROWS), out val))
					{
						return val;
					}
					else
					{
						NullSet(ROWS, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(ROWS, value.ToString());
				}
				else
				{
					NullSet(ROWS, null);
				}
			}
		}
		public override bool UseFullCloseTag
		{
			get
			{
				return true;
			}
		}
		public override string ToString()
		{
			InnerText = System.Web.HttpUtility.HtmlEncode(InnerText);
			return base.ToString();
		}
	}
}
