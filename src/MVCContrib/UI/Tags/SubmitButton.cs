using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class SubmitButton : Input
	{
		public SubmitButton(IDictionary attributes) : base("submit", attributes)
		{
		}

		public SubmitButton() : this(Hash.Empty)
		{
		}
	}
}