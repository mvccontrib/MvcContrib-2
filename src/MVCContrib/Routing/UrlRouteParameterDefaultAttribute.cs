using System;

namespace MvcContrib.Routing
{
    /// <summary>
    /// Assigns a default value to a route parameter in a UrlRouteAttribute
    /// if not specified in the URL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UrlRouteParameterDefaultAttribute : Attribute
    {
        /// <summary>
        /// Name of the route parameter for which to supply the default value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default value to set on the route parameter if not specified in the URL.
        /// </summary>
        public object Value { get; set; }
    }
}
