using System;
using System.Web.Mvc;

namespace MvcContrib.ActionResults
{
    /// <summary>
    /// Action Result that performs an HTTP 301 permanent redirect to an action.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// public PermanentRedirectToActionResult OldActionName(int id) {
    ///		return new PermanentRedirectToActionResult("NewActionName", new { id = id });
    /// }
    /// ]]>
    /// </example>
    public class PermanentRedirectToActionResult : ActionResult
    {
        /// <summary>
        /// Creates a new instance of the PermanentRedirectToActionResult class
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        public PermanentRedirectToActionResult(string actionName) : this(actionName, null, null) { }

        /// <summary>
        /// Creates a new instance of the PermanentRedirectToActionResult class
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object containing the parameters for a route. The parameters are retrieved via reflection by examining the properties of the object. Typically created using object initializer syntax.</param>
        public PermanentRedirectToActionResult(string actionName, object routeValues) : this(actionName, null, routeValues) { }

        /// <summary>
        /// Creates a new instance of the PermanentRedirectToActionResult class
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object containing the parameters for a route. The parameters are retrieved via reflection by examining the properties of the object. Typically created using object initializer syntax.</param>
        public PermanentRedirectToActionResult(string actionName, string controllerName, object routeValues)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
            this.RouteValues = routeValues;
        }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// The name of the action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// An object containing the parameters for a route. The parameters are retrieved via reflection by examining the properties of the object. Typically created using object initializer syntax.
        /// </summary>
        public object RouteValues { get; set; }

        /// <summary>
        /// Calculates the url based on the values passed, and sends an HTTP 301 Permanent Redirect to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.Action(this.ActionName, this.ControllerName, this.RouteValues);
            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = url;
        }
    }
}
