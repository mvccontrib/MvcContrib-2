using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class RadioList: InputElementList<RadioField>
	{
		public RadioList(IDictionary attributes) : base(attributes)
		{
		}
		public RadioList() : this(Hash.Empty)
		{
		}
	}
}