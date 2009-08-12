using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class HiddenField : Input
	{
		public HiddenField(IDictionary attributes) : base("hidden", attributes)
		{
		}

		public HiddenField() : this(Hash.Empty)
		{
		}
	}
}