using System;

namespace MvcContrib.Routing
{
    /// <summary>
    /// Assigns a URL route to an MVC Controller class method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UrlRouteAttribute : Attribute
    {
        /// <summary>
        /// Optional name of the route.  Route names must be unique per route.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Optional order in which to add the route (default is 0).  Routes
        /// with lower order values will be added before those with higher.
        /// Routes that have the same order value will be added in undefined
        /// order with respect to each other.
        /// </summary>
        public int Order { get; set; }

        private string _path;
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (value.StartsWith("/"))
                {
                    throw new ArgumentException("Paths should not start with '/'", "value");
                }
                
                if (value.EndsWith("/"))
                {
                    throw new ArgumentException("Paths should not end in '/'", "value");
                }

                _path = value;
            }
        }
    }
}
