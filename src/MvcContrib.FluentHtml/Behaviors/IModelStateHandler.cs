using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	/// <summary>
	/// Used internally by the ValidationBehavior to handle ModelState values for a particular element type.
	/// </summary>
	public interface IModelStateHandler
	{
		bool Handle(IElement element, ModelState state);
	}
}