using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.Routing
{
    public static class RouteAttributeUtility
    {
        const string ControllerSuffix = "Controller";

        public static void RegisterRoutes(RouteCollection routes)
        {
            RegisterRoutes(routes, Assembly.GetCallingAssembly());
        }

        public static void RegisterRoutes(RouteCollection routes, Assembly assembly)
        {
            var routeAttributeMetadata = GetUrlRouteAttributesFromControllersInAssembly(assembly);
            SortRoutesByOrderAttribute(routeAttributeMetadata);

            foreach (var routeDefinition in routeAttributeMetadata)
            {
                AddRoute(routeDefinition, routes);
            }
        }

        public static void RegisterRoutesForController(RouteCollection routes, Type controller)
        {
            var routeAttributeMetadata = new List<RouteAttributeMetadata>();

            GatherRouteDefinitionsFromPublicMethods(controller, routeAttributeMetadata);
            SortRoutesByOrderAttribute(routeAttributeMetadata);

            foreach (var routeDefinition in routeAttributeMetadata)
            {
                AddRoute(routeDefinition, routes);
            }
        }

        private static void AddRoute(RouteAttributeMetadata routeDefinition, RouteCollection routes)
        {
            Trace.TraceInformation(
                "Adding route {{ Priority = {0}, Name = {1}, Path = {2}, Controller = {3}, Action = {4} }}",
                routeDefinition.Order, routeDefinition.RouteName ?? "<null>", routeDefinition.Path,
                routeDefinition.ControllerName, routeDefinition.ActionName);

            routeDefinition.Defaults["controller"] = routeDefinition.ControllerName;
            routeDefinition.Defaults["action"] = routeDefinition.ActionName;

            MapRoute(routes, routeDefinition.RouteName, routeDefinition.Path, routeDefinition.Defaults,
                     routeDefinition.Constraints, new[] {routeDefinition.ControllerNamespace});
        }

        private static void SortRoutesByOrderAttribute(List<RouteAttributeMetadata> routeParams)
        {
            routeParams.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        private static List<RouteAttributeMetadata> GetUrlRouteAttributesFromControllersInAssembly(Assembly callingAssembly)
        {
            var routeParams = new List<RouteAttributeMetadata>();
            var controllerClasses = GetControllerClasses(callingAssembly);

            foreach (var controller in controllerClasses)
            {
                GatherRouteDefinitionsFromPublicMethods(controller, routeParams);
            }

            return routeParams;
        }

        private static IEnumerable<Type> GetControllerClasses(Assembly callingAssembly)
        {
            return from t in callingAssembly.GetTypes()
                   where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller))
                   select t;
        }

        private static void GatherRouteDefinitionsFromPublicMethods(Type controller, ICollection<RouteAttributeMetadata> routeParams)
        {
            foreach (var methodInfo in controller.GetMethods())
            {
                ExtractUrlRouteAttributeMetadata(controller, methodInfo, routeParams);
            }
        }

        private static void ExtractUrlRouteAttributeMetadata(Type controller, MethodInfo methodInfo, ICollection<RouteAttributeMetadata> routeParams)
        {
            foreach (UrlRouteAttribute routeAttrib in methodInfo.GetCustomAttributes(typeof(UrlRouteAttribute), false))
            {
                if(string.IsNullOrEmpty(routeAttrib.Path))
                {
                    throw new InvalidOperationException("Route attribute present which contains no 'Path', cannot map route.  Either remove the attribute or add a path.");
                }

                if (!controller.Name.EndsWith(ControllerSuffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new InvalidOperationException(String.Format("Invalid controller class name {0}: name must end with \"{1}\"", controller.Name, ControllerSuffix));
                }

                if (routeAttrib.Path.StartsWith("/") || routeAttrib.Path.Contains("?"))
                {
                    throw new InvalidOperationException(String.Format("Invalid UrlRoute attribute \"{0}\" on method {1}.{2}: Path cannot start with \"/\" or contain \"?\".", routeAttrib.Path, controller.Name, methodInfo.Name));
                }

                routeParams.Add(new RouteAttributeMetadata
                                    {
                                        RouteName = String.IsNullOrEmpty(routeAttrib.Name) ? null : routeAttrib.Name,
                                        Path = routeAttrib.Path,
                                        ControllerName = SanitiseControllerName(controller),
                                        ActionName = DetermineActionName(methodInfo),
                                        Order = routeAttrib.Order,
                                        Constraints = GetRouteConstrainsFromAttributes(methodInfo),
                                        Defaults = GetDefaults(methodInfo),
                                        ControllerNamespace = controller.Namespace,
                                    });
            }
        }

        private static string SanitiseControllerName(Type controller)
        {
            return controller.Name.Substring(0, controller.Name.Length - ControllerSuffix.Length);
        }

        private static string DetermineActionName(MethodInfo methodInfo)
        {
            var actionName = methodInfo.Name;
            var actionNameAttribute = methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), false).FirstOrDefault() as ActionNameAttribute;
            if (actionNameAttribute != null)
            {
                actionName = actionNameAttribute.Name;
            }
            return actionName;
        }

        /// <summary>
        /// This was copied from System.Web.Mvc.RouteCollectionExtensions and
        /// modified slightly.  The original function declares the defaults
        /// and constraints parameters as object type, which causes the wrong
        /// overload of RouteValueDictionary to be invoked, causing values in
        /// the dictionaries not to be set properly.  The modified version
        /// declares these parameters as Dictionary&lt;string, object&gt;,
        /// fixing the problem.
        /// </summary>
        private static void MapRoute(RouteCollection routes, string name, string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, ICollection<string> namespaces)
        {
            if (routes == null) { throw new ArgumentNullException("routes"); }
            if (url == null) { throw new ArgumentNullException("url"); }
            if (constraints == null) { throw new ArgumentNullException("constraints"); }

            var route = new Route(url, new MvcRouteHandler())
                          {
                              Defaults = new RouteValueDictionary(defaults),
                              Constraints = new RouteValueDictionary(constraints)
                          };

            if ((namespaces != null) && (namespaces.Count > 0))
            {
                route.DataTokens = new RouteValueDictionary();
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return;
        }

        private static Dictionary<string, object> GetRouteConstrainsFromAttributes(MethodInfo method)
        {
            var constraints = new Dictionary<string, object>();

            foreach (UrlRouteParameterConstraintAttribute attrib in method.GetCustomAttributes(typeof(UrlRouteParameterConstraintAttribute), true))
            {
                if (String.IsNullOrEmpty(attrib.Name))
                {
                    throw new InvalidOperationException(String.Format("UrlRouteParameterContraint attribute on {0}.{1} is missing the Name property.", method.DeclaringType.Name, method.Name));
                }

                if (String.IsNullOrEmpty(attrib.Regex))
                {
                    throw new InvalidOperationException(String.Format("UrlRouteParameterContraint attribute on {0}.{1} is missing the RegEx property.", method.DeclaringType.Name, method.Name));
                }
                
                constraints.Add(attrib.Name, attrib.Regex);
            }

            return constraints;
        }

        private static Dictionary<string, object> GetDefaults(MethodInfo methodInfo)
        {
            var defaults = new Dictionary<string, object>();

            foreach (UrlRouteParameterDefaultAttribute urlRouteParameterAttribute in methodInfo.GetCustomAttributes(typeof(UrlRouteParameterDefaultAttribute), true))
            {
                if (string.IsNullOrEmpty(urlRouteParameterAttribute.Name))
                {
                    throw new InvalidOperationException(String.Format("UrlRouteParameterDefault attribute on {0}.{1} is missing the Name property.", methodInfo.DeclaringType.Name, methodInfo.Name));
                }

                if (urlRouteParameterAttribute.Value == null)
                {
                    throw new InvalidOperationException(String.Format("UrlRouteParameterDefault attribute on {0}.{1} is missing the Value property.", methodInfo.DeclaringType.Name, methodInfo.Name));
                }

                defaults.Add(urlRouteParameterAttribute.Name, urlRouteParameterAttribute.Value);
            }

            return defaults;
        }

        class RouteAttributeMetadata
        {
            public int Order { get; set; }
            public string RouteName { get; set; }
            public string Path { get; set; }
            public string ControllerNamespace { get; set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public Dictionary<string, object> Defaults { get; set; }
            public Dictionary<string, object> Constraints { get; set; }
        }
    }
}
