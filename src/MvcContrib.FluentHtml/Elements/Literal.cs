using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a literal (text inside a span element).
	/// </summary>
	public class Literal : LiteralBase<Literal>
	{
		/// <summary>
		/// Generates a literal element (span).
		/// </summary>
		/// <param name="name">The name of the element.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public Literal(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) :
            base(name, forMember, behaviors) { }

		/// <summary>
		/// Generates a literal element (span).
		/// </summary>
        /// <param name="name">The name of the element.</param>
        public Literal(string name) : base(name) { }
	}
}
