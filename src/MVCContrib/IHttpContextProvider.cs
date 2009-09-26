using System.Web;

namespace MvcContrib
{
	public interface IHttpContextProvider
	{
		HttpContextBase Context { get; }
		HttpRequestBase Request { get; }
	}
}