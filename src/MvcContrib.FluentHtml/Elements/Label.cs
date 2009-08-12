using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a label (provides no visual difference, but can be tied to an input element to help with naviagation).
	/// </summary>
	public class Label : LabelBase<Label>
	{
		/// <summary>
		/// Generates a label element.
		/// </summary>
		/// <param name="forElement">Value to be used for the 'for' attribute of the element.  Should be the 'id' of the input element this label is for.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public Label(string forElement, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) : 
			base(forElement, forMember, behaviors) {}

		/// <summary>
		/// Generates a label element.
		/// </summary>
		/// <param name="forElement">Value to be used for the 'for' attribute of the element.  Should be the 'id' of the input element this label is for.</param>
		public Label(string forElement) : base(forElement)
		{
		}

		/// <summary>
		/// Generates a label element.
		/// </summary>
		public Label() {}
	}
}
