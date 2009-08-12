using System.Web.Routing;
using System.Web;

namespace MvcContrib.Routing
{
	/// <summary>
	/// Route handler that ensures all requestes are routed to the DebugHttpHandler.
	/// </summary>
	public class DebugRouteHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new DebugHttpHandler { RequestContext = requestContext };
		}
	}
}
