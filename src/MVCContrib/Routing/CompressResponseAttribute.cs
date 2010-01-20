using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.Routing
{
    public class CompressResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (!String.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToUpperInvariant();

                HttpResponseBase response = filterContext.HttpContext.Response;

                if (acceptEncoding.Contains("GZIP"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                }
            }
        }
    }
}