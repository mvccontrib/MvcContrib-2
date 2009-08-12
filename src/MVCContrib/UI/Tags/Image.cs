using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class Image : ScriptableElement
	{
		private const string SRC = "src";
		private const string ALT = "alt";
		private const string HEIGHT = "height";
		private const string WIDTH = "width";
		public Image()
			: base("img",Hash.Empty)
		{
		}

		public Image(string src)
			: base("img",Hash.Empty)
		{
			Src = src;
		}

		public Image(string src, IDictionary attributes)
			: base("img",attributes)
		{
			Src = src;
		}

		public virtual string Src
		{
			get { return NullGet(SRC); }
			set { NullSet(SRC, value); }
		}
		public virtual string Alt
		{
			get { return NullGet(ALT); }
			set { NullSet(ALT, value); }
		}

		public int Width
		{
			get
			{
				if (NullGet(WIDTH) != null)
				{
					int val;
					if (int.TryParse(NullGet(WIDTH), out val))
					{
						return val;
					}
					else
					{
						NullSet(WIDTH, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(WIDTH, value.ToString());
				}
				else
				{
					NullSet(WIDTH, null);
				}
			}
		}

		public int Height
		{
			get
			{
				if (NullGet(HEIGHT) != null)
				{
					int val;
					if (int.TryParse(NullGet(HEIGHT), out val))
					{
						return val;
					}
					else
					{
						NullSet(HEIGHT, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(HEIGHT, value.ToString());
				}
				else
				{
					NullSet(HEIGHT, null);
				}
			}
		}
	}
}
