using System.Net;
using System.Web.Mvc;

namespace MvcContrib.FluentController
{
    public class HeadResult : ActionResult
    {
        public HttpStatusCode StatusCode { get; set; }

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