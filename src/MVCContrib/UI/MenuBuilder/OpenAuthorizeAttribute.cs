using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.UI.MenuBuilder
{
	/// <summary>
	/// This class opens up the AuthorizeCore method which is protected in the MVC framework. 
	/// If this method is made public, this entire file could be eliminated.
	/// </summary>
	public class OpenAuthorizeAttribute : AuthorizeAttribute
	{
		public OpenAuthorizeAttribute(AuthorizeAttribute attribute)
		{
			Order = attribute.Order;
			Roles = attribute.Roles;
			Users = attribute.Users;
		}

		public bool Authorized(RequestContext requestContext)
		{
			return AuthorizeCore(requestContext.HttpContext);
		}
	}

	public static class AuthorizeAttributeExtensions
	{
		public static bool Authorized(this AuthorizeAttribute attribute, ControllerContext context)
		{
			var securityActionFilterAttribute = new OpenAuthorizeAttribute(attribute);
			return securityActionFilterAttribute.Authorized(context.RequestContext);
		}
	}
}