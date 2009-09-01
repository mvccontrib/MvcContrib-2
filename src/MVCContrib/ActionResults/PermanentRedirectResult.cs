using System;
using System.Web.Mvc;

namespace MvcContrib.ActionResults
{
    /// <summary>
    /// Action result that performs an HTTP 301 permanent redirect to a url.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// public PermanentRedirectResult OldActionName() {
    ///		return new PermanentRedirectResult("http://www.asp.net/mvc/");
    /// }
    /// ]]>
    /// </example>
    public class PermanentRedirectResult : ActionResult
    {
        /// <summary>
        /// Creates a new instance of the PermanentRedirectResult class
        /// </summary>
        public PermanentRedirectResult()
        {

        }

        /// <summary>
        /// Creates a new instance of the PermanentRedirectResult class
        /// </summary>
        /// <param name="url"></param>
        public PermanentRedirectResult(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// The url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Sends an HTTP 301 Permanent Redirect to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException("Url cannot be null or empty");
            }

            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = this.Url;
        }
    }
}
