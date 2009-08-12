using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// A set of radio buttons.
	/// </summary>
	public class RadioSet : RadioSetBase<RadioSet>
	{
		public RadioSet(string name) : base(HtmlTag.Div, name) { }

		public RadioSet(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlTag.Div, name, forMember, behaviors) { }
	}
}