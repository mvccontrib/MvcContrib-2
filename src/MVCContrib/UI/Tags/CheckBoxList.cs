using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class CheckBoxList : InputElementList<CheckBoxField>
	{
		public CheckBoxList(IDictionary attributes) : base(attributes)
		{
		}
		public CheckBoxList() : this(Hash.Empty)
		{
		}
	}
}
