using System;

namespace MvcContrib.Routing
{
    /// <summary>
    /// Assigns a constraint to a route parameter in a UrlRouteAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UrlRouteParameterConstraintAttribute : Attribute
    {
        /// <summary>
        /// Name of the route parameter on which to apply the constraint.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Regular expression constraint to test on the route parameter value
        /// in the URL.
        /// </summary>
        public string Regex { get; set; }
    }
}
