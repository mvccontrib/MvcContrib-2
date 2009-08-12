using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class TextBox : Input
	{
		private const string MAXLENGTH = "maxlength";
		private const string SIZE = "size";
		public TextBox(IDictionary attributes) : base("text", attributes)
		{
		}

		public TextBox() : this(Hash.Empty)
		{
		}


		public int MaxLength
		{
			get
			{
				if (NullGet(MAXLENGTH) != null)
				{
					int val;
					if (int.TryParse(NullGet(MAXLENGTH), out val))
					{
						return val;
					}
					else
					{
						NullSet(MAXLENGTH, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(MAXLENGTH, value.ToString());
				}
				else
				{
					NullSet(MAXLENGTH, null);
				}
			}
		}

		public int Size
		{
			get
			{
				if (NullGet(SIZE) != null)
				{
					int val;
					if (int.TryParse(NullGet(SIZE), out val))
					{
						return val;
					}
					else
					{
						NullSet(SIZE, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(SIZE, value.ToString());
				}
				else
				{
					NullSet(SIZE, null);
				}
			}
		}
	}
}