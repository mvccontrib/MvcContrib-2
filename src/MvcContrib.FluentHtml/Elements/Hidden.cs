using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates an HTML input element of type 'hidden.'
	/// </summary>
	public class Hidden : TextInput<Hidden>
	{
		/// <summary>
		/// Generates an HTML input element of type 'hidden.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public Hidden(string name) : base(HtmlInputType.Hidden, name) { }

		/// <summary>
		/// Generates an HTML input element of type 'hidden.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the model member assocaited with the element.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public Hidden(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Hidden, name, forMember, behaviors) { }
	}
}
