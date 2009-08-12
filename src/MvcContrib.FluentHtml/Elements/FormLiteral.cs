using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use this 
	/// if you want to display a value and also have that same value be included in the form post.
	/// </summary>
	public class FormLiteral : FormLiteralBase<FormLiteral>
	{
		/// <summary>
		/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use 
		/// this if you want to display a value and also have that same value be included in the form post.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public FormLiteral(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) 
			: base(name, forMember, behaviors) {}

		/// <summary>
		/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use 
		/// this if you want to display a value and also have that same value be included in the form post.
		/// </summary>
		/// <param name="name">Value used to set the 'name' an 'id' attributes of the element.</param>
		public FormLiteral(string name) : base(name) { }
	}
}
