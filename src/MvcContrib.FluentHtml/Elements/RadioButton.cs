using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// A radio button.
	/// </summary>
	public class RadioButton : RadioButtonBase<RadioButton>
	{
		public RadioButton(string name) : base(name) { }

		public RadioButton(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(name, forMember, behaviors) { }
	}
}