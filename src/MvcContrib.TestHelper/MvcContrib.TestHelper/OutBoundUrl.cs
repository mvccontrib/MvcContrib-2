using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace MvcContrib.TestHelper
{
    public static class OutBoundUrl
    {
        public static OutBoundUrlContext OfRouteNamed(string routeName)
        {
            return new OutBoundUrlContext()
            {
                RouteType = new NamedRoute(routeName)
            };
        }

        public static OutBoundUrlContext Of<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
        {
            var controllerName = typeof(TController).Name.Replace("Controller", "");
            var methodCall = ((MethodCallExpression)action.Body);
            var methodName = methodCall.Method.Name;
            var routeValues = new RouteValueDictionary();

            for (int i = 0; i < methodCall.Arguments.Count; i++)
            {
                string name = methodCall.Method.GetParameters()[i].Name;
                object value = null;

                switch (methodCall.Arguments[i].NodeType)
                {
                    case ExpressionType.Constant:
                        value = ((ConstantExpression)methodCall.Arguments[i]).Value;
                        break;

                    case ExpressionType.MemberAccess:
                        value = Expression.Lambda(methodCall.Arguments[i]).Compile().DynamicInvoke();
                        break;

                }

                routeValues.Add(name, value);
            }

            return new OutBoundUrlContext()
            {
                RouteType = new DerivedRoute(controllerName, methodName, routeValues)
            };
        }
    }

    public class DerivedRoute : IRouteType
    {
        private readonly string controller;
        private readonly string action;
        private readonly RouteValueDictionary routeValues;

        public DerivedRoute(string controller, string action, RouteValueDictionary routeValues)
        {
            this.controller = controller;
            this.action = action;
            this.routeValues = routeValues;
        }

        public string GetUrl(UrlHelper helper)
        {
            return helper.Action(action, controller, routeValues);
        }
    }

    public class NamedRoute : IRouteType
    {
        private readonly string name;

        public NamedRoute(string name)
        {
            this.name = name;
        }

        public string GetUrl(UrlHelper helper)
        {
            return helper.RouteUrl(name);
        }
    }

    public class OutBoundUrlContext
    {
        public void ShouldMapToUrl(string url)
        {
            var builder = new TestControllerBuilder();
            var context = new RequestContext(builder.HttpContext, new RouteData());
            context.HttpContext.Response.Stub(x => x.ApplyAppPathModifier(null)).IgnoreArguments().Do(new Func<string, string>(s => s)).Repeat.Any();

            var urlhelper = new UrlHelper(context);

            var generatedUrl = RouteType.GetUrl(urlhelper);
            generatedUrl.AssertSameStringAs(url);
        }

        public IRouteType RouteType { get; set; }
    }

    public interface IRouteType
    {
        string GetUrl(UrlHelper helper);
    }
}