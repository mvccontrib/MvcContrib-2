using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class InputButton : Input
	{
		public InputButton(IDictionary attributes) : base("button", attributes)
		{
		}

		public InputButton()
			: this(Hash.Empty)
		{
		}

	}
}