using System;
using System.Web;
using HttpSessionStateBase = System.Web.HttpSessionStateBase;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	public class TestHttpContext : HttpContextBase
	{
	}

	public class TestHttpHandler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}

		public bool IsReusable { get; set; }
	}

	public class TestHttpRequest : HttpRequestBase
	{
	}

	public class TestHttpResponse : HttpResponseBase{

	}

	public class TestHttpServerUtility : HttpServerUtilityBase
	{
	}

	public class TestHttpSessionState :  HttpSessionStateBase
	{
	}
}
