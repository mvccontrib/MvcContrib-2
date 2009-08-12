using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class InputImage:Input
	{
		public const string SRC = "src";
		public const string ALT = "alt";

		public InputImage(string src)
			: this(Hash.Empty)
		{
			Src = src;
		}


		public InputImage(string src, IDictionary attributes)
			: this(attributes)
		{
			Src = src;
		}

		public InputImage(IDictionary attributes) : base("image", attributes)
		{
		}

		public InputImage()
			: this(Hash.Empty)
		{
		}

		public string Src
		{
			get { return NullGet(SRC); }
			set { NullSet(SRC, value); }
		}

		public string Alt
		{
			get { return NullGet(ALT); }
			set { NullSet(ALT, value); }
		}
	}
}
