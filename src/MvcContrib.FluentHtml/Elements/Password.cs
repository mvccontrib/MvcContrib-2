using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML input element of type 'password.'
	/// </summary>
	public class Password : TextInput<Password>
	{
		/// <summary>
		/// Generate an HTML input element of type 'password.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public Password(string name) : base(HtmlInputType.Password, name) { }

		/// <summary>
		/// Generate an HTML input element of type 'password.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the model member assocaited with the element.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public Password(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Password, name, forMember, behaviors) { }

		protected override void ApplyModelState(System.Web.Mvc.ModelState state) {
			//no-op - passwords should not be restored.
		}
	}
}
