using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.Attributes
{
	/// <summary>
	/// Ensures that an action is not executed unless it is accessed through an HTTP GET.
	/// </summary>
    public class AcceptGetAttribute : ActionMethodSelectorAttribute
	{
		private readonly AcceptVerbsAttribute _innerAttribute = new AcceptVerbsAttribute("GET");

		/// <summary>
		/// Ensures that the action decorated by this attribute is only valid if the HTTP method is a GET.
		/// </summary>
		/// <param name="controllerContext">The current controller context</param>
		/// <param name="methodInfo">The current action method</param>
		/// <returns>True if the action is being accessed via an HTTP GET</returns>
		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			return _innerAttribute.IsValidForRequest(controllerContext, methodInfo);
		}
	}
}