using System.Net;
using System.Web.Mvc;

namespace MvcContrib
{
	///<summary>
	/// HeadResult is a specific <see cref="ActionResult"/> for returning the client an <see cref="HttpStatusCode"/>.
	/// This is particular useful for RESTful applications that return status codes only in some situations eg Not-Modified
	///</summary>
	public class HeadResult : ActionResult
	{
		public HttpStatusCode StatusCode { get; private set; }

		public HeadResult(HttpStatusCode statusCode)
		{
			StatusCode = statusCode;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var response = context.RequestContext.HttpContext.Response;
			response.StatusCode = (int)StatusCode;
		}
	}
}