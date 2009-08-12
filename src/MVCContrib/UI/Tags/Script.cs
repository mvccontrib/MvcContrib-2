using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class Script : Element
	{
		public const string ATTR_LANG = "lang";
		public const string ATTR_DEFER = "defer";
		public const string ATTR_TYPE = "type";
		public const string ATTR_SRC = "src";


		public Script() : this(Hash.Empty)
		{
		}

        public Script(IDictionary attributes)
            : base("script", attributes)
		{
			if (!Attributes.Contains(ATTR_TYPE))
                Attributes[ATTR_TYPE] = "text/javascript";
            EscapeInnerText = true;
		}

        public override bool UseFullCloseTag
        {
            get
            {
                return true;
            }
        }

		public string Src
		{
			get { return NullGet(ATTR_SRC); }
			set { NullSet(ATTR_SRC, value); }
		}

		public string Type 
		{
			get { return NullGet(ATTR_TYPE); }
			set { NullSet(ATTR_TYPE, value);}
		}

		public bool? Defer
		{
			get
			{
				string value = NullGet(ATTR_DEFER);
				if(string.IsNullOrEmpty(value))
				{
					return null;
				}
				return bool.Parse(value);
			}

			set
			{
				if(!value.HasValue)
				{
					NullSet(ATTR_DEFER, null);
				}
				else
				{
					NullSet(ATTR_DEFER, value.Value.ToString().ToLowerInvariant());
				}
			}
		}

		public string Lang 
		{
			get { return NullGet(ATTR_LANG); }
			set { NullSet(ATTR_LANG, value); }
		}

		public string Code
		{
			get { return base.InnerText; }
            set { InnerText = value; }
		}

        //protected virtual string WrapScriptBlock()
        //{
        //    if(string.IsNullOrEmpty(Code))
        //    {
        //        return string.Empty;
        //    }
        //    return "/*<![CDATA[*/ " + Code + " //]]>";
        //}
	}
}
