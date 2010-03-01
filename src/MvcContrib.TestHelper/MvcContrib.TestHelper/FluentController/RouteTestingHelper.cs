using System;
using System.Web.Routing;
using System.Web;
using System.Collections.Specialized;
using System.Web.Mvc;
using Moq;
using System.Linq.Expressions;
using MvcContrib.TestHelper.FluentController.Fakes;

namespace MvcContrib.TestHelper.FluentController
{
    /// <summary>
    /// Used to simplify Restful testing routes and can be used on conjunction with standard router testing extensions.
    /// <example>
    /// This tests that incoming PUT on resource is redirected to Update
    ///             "~/banner/1"
    ///               .GivenIncomingAs(HttpVerbs.Put)
    ///               .ShouldMapTo&lt;BannerController>(action => action.Update(1));
    ///
    /// This tests that incoming PUT on resource is redirected to Update
    ///             "~/banner/1"
    ///               .GivenIncomingAs(HttpVerbs.Put)
    ///               .ShouldMapTo&lt;BannerController>(action => action.Update(1));
    /// </example>
    /// </summary>
    public static class Routes
    {
        /// <summary>
        /// Returns the corresponding route for the URL.  Returns null if no route was found.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="formMethod">The form method.</param>
        /// <returns></returns>
        public static RouteData Route(this string url, HttpVerbs httpMethod, HttpVerbs formMethod)
        {
            var context = FakeHttpContext(url, httpMethod, formMethod);
            var route = RouteTable.Routes.GetRouteData(context);
            
            // cater for SimplyRestful methods and others
            // adding values during the GetHttpHandler method
            route.ReadValue(x => x.RouteHandler).ReadValue(x => x.GetHttpHandler(new RequestContext(context, route)));
            return route;
        }

        /// <summary>
        /// Returns the corresponding route for the URL.  Returns null if no route was found.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        public static RouteData Route(this string url, HttpVerbs httpMethod)
        {
            var context = FakeHttpContext(url, httpMethod);
            var route = RouteTable.Routes.GetRouteData(context);

            // cater for SimplyRestful methods and others
            // adding values during the GetHttpHandler method
            route.ReadValue(x => x.RouteHandler).ReadValue(x => x.GetHttpHandler(new RequestContext(context, route)));
            return route;
        }

        /// <summary>
        /// Asserts that the route matches the expression specified based on the incoming HttpMethod and FormMethod for Simply Restful routing.  Checks controller, action, and any method arguments
        /// into the action as route values.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="formMethod">The form method.</param>
        /// <returns></returns>
        public static RouteData GivenIncomingAs(this string relativeUrl, HttpVerbs httpMethod, HttpVerbs formMethod)
        {
            return relativeUrl.Route(httpMethod, formMethod);
        }

        /// <summary>
        /// Asserts that the route matches the expression specified based on the incoming HttpMethod for Simply Restful routing.  Checks controller, action, and any method arguments
        /// into the action as route values.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        public static RouteData GivenIncomingAs(this string relativeUrl, HttpVerbs httpMethod)
        {
            return relativeUrl.Route(httpMethod);
        }

        #region Copied from MvcContrib.TestHelper
        /// <summary>
        /// Returns the corresponding route for the URL.  Returns null if no route was found.
        /// <example>
        /// "~/user"
        ///     .GivenIncomingAs(HttpVerbs.Post)
        ///     .ShouldMapTo&lt;UserController>(action => action.Create(new UserModel()));
        /// </example>
        /// </summary>
        /// <param name="url">The app relative url to test.</param>
        /// <returns>A matching <see cref="RouteData" />, or null.</returns>
        public static RouteData Route(this string url)
        {
            var context = FakeHttpContext(url);
            return RouteTable.Routes.GetRouteData(context);
        }

        /// <summary>
        /// Asserts that the route matches the expression specified.  Checks controller, action, and any method arguments
        /// into the action as route values.
        /// </summary>
        /// <typeparam name="TController">The controller.</typeparam>
        /// <param name="routeData">The routeData to check</param>
        /// <param name="action">The action to call on TController.</param>
        public static RouteData ShouldMapTo<TController>(this RouteData routeData, Expression<Func<TController, ActionResult>> action)
            where TController : Controller
        {
            routeData.ShouldNotBeNull("The URL did not match any route");

            //check controller
            routeData.ShouldMapTo<TController>();

            //check action
            var methodCall = (MethodCallExpression)action.Body;
            string actualAction = routeData.Values.GetValue("action").ToString();
            string expectedAction = methodCall.Method.Name;
            actualAction.AssertSameStringAs(expectedAction);

            //check parameters
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

                value = (value == null ? value : value.ToString());
                routeData.Values.GetValue(name).ShouldEqual(value, "Value for parameter did not match");
            }

            return routeData;
        }

        /// <summary>
        /// Converts the URL to matching RouteData and verifies that it will match a route with the values specified by the expression.
        /// </summary>
        /// <typeparam name="TController">The type of controller</typeparam>
        /// <param name="relativeUrl">The ~/ based url</param>
        /// <param name="action">The expression that defines what action gets called (and with which parameters)</param>
        /// <returns></returns>
        public static RouteData ShouldMapTo<TController>(this string relativeUrl, Expression<Func<TController, ActionResult>> action) where TController : Controller
        {
            return relativeUrl.Route().ShouldMapTo(action);
        }



        /// <summary>
        /// Verifies the <see cref="RouteData">routeData</see> maps to the controller type specified.
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static RouteData ShouldMapTo<TController>(this RouteData routeData) where TController : Controller
        {
            //strip out the word 'Controller' from the type
            string expected = typeof(TController).Name.Replace("Controller", "");

            //get the key (case insensitive)
            string actual = routeData.Values.GetValue("controller").ToString();


            expected.AssertSameStringAs(actual);
            return routeData;
        }


        /// <summary>
        /// Verifies that the route data does not map.
        /// </summary>
        /// <returns></returns>
        public static RouteData ShouldNotBeMapped(this string relativeUrl)
        {
            return relativeUrl.Route().ShouldNotBeMapped();
        }

        /// <summary>
        /// Verifies that the route data does not map.
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static RouteData ShouldNotBeMapped(this RouteData routeData)
        {
            routeData.ShouldNotBeNull("The URL matched a route when it was not expected to.  Matched route: " + (string) routeData.Values["controller"] + ", " + (string) routeData.Values["action"]);
            return routeData;
        }

        /// <summary>
        /// Verifies the <see cref="RouteData">routeData</see> will instruct the routing engine to ignore the route.
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static RouteData ShouldBeIgnored(this string relativeUrl)
        {
            RouteData routeData = relativeUrl.Route();

            routeData.RouteHandler.ShouldBe<StopRoutingHandler>("Expected StopRoutingHandler, but wasn't");
            return routeData;
        }

        /// <summary>
        /// Gets a value from the <see cref="RouteValueDictionary" /> by key.  Does a
        /// case-insensitive search on the keys.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(this RouteValueDictionary routeValues, string key)
        {
            foreach (var routeValueKey in routeValues.Keys)
            {
                if (string.Equals(routeValueKey, key, StringComparison.InvariantCultureIgnoreCase))
                    return routeValues[routeValueKey].ToString();
            }

            return null;
        }
        #endregion

        private static HttpContextBase FakeHttpContext(string url, HttpVerbs? httpMethod, HttpVerbs? formMethod)
        {
            NameValueCollection form = null;

            if (formMethod.HasValue)
                form = new NameValueCollection { { "_method", formMethod.Value.ToString().ToUpper() } };

            if (!httpMethod.HasValue)
                httpMethod = HttpVerbs.Get;

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);
            request.SetupGet(x => x.PathInfo).Returns(string.Empty);
            request.SetupGet(x => x.Form).Returns(form);
            request.SetupGet(x => x.HttpMethod).Returns(httpMethod.Value.ToString().ToUpper());

            var context = new FakeHttpContext(url);
            context.SetRequest(request.Object);

            return context;
        }

        private static HttpContextBase FakeHttpContext(string url, HttpVerbs? httpMethod)
        {
            return FakeHttpContext(url, httpMethod, null);
        }


        private static HttpContextBase FakeHttpContext(string url)
        {
            return FakeHttpContext(url, null, null);
        }

    }
}