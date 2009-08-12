using System.Linq.Expressions;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Interface for elements that are associated with a model member.
	/// </summary>
	public interface IMemberElement : IElement
	{
		/// <summary>
		/// Expression indicating the view model member associated with the element.</param>
		/// </summary>
		MemberExpression ForMember { get; }
	}
}
