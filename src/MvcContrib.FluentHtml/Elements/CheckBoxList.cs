using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// A list of checkboxes buttons.
	/// </summary>
	public class CheckBoxList : CheckBoxListBase<CheckBoxList>
	{
		public CheckBoxList(string name) : base(HtmlTag.Div, name) { }

		public CheckBoxList(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlTag.Div, name, forMember, behaviors) { }
	}
}