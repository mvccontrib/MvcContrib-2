using System;
using System.Web.Routing;

namespace MvcContrib.SimplyRestful
{
	public class RestfulActionResolver : IRestfulActionResolver
	{
		public RestfulAction ResolveAction(RequestContext context)
		{
			if(context.HttpContext.Request == null)
			{
				throw new NullReferenceException("Request in RequestContext.HttpContext cannot be null.");
			}

			if(string.IsNullOrEmpty(context.HttpContext.Request.HttpMethod))
			{
				return RestfulAction.None;
			}

			string requestMethod = context.HttpContext.Request.HttpMethod.ToUpperInvariant();
			if(string.Equals(requestMethod, "POST", StringComparison.Ordinal))
			{
				return ResolvePostAction(context);
			}

			return RestfulAction.None;
		}

		private static RestfulAction ResolvePostAction(RequestContext context)
		{
			if(context.HttpContext.Request.Form == null)
			{
				return RestfulAction.None;
			}

			string formMethod = context.HttpContext.Request.Form["_method"];
			if(string.IsNullOrEmpty(formMethod))
			{
				return RestfulAction.None;
			}

			formMethod = formMethod.Trim().ToUpperInvariant();
			if(string.Equals("PUT", formMethod))
			{
				return RestfulAction.Update;
			}
			else if(string.Equals("DELETE", formMethod, StringComparison.Ordinal))
			{
				return RestfulAction.Destroy;
			}

			return RestfulAction.None;
		}
	}
}
