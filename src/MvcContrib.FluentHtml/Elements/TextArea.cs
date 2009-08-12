using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	public class TextArea : TextAreaBase<TextArea>
	{
		/// <summary>
		/// Generate an HTML textarea element.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public TextArea(string name) : base(name) { }

		/// <summary>
		/// Generate an HTML textarea element.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public TextArea(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) 
			: base(name, forMember, behaviors) { }
	}
}
