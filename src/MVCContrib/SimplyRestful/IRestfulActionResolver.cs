using System.Web.Routing;

namespace MvcContrib.SimplyRestful
{
	public interface IRestfulActionResolver
	{
		RestfulAction ResolveAction(RequestContext context);
	}
}
