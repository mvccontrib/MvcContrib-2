using System.Web;
using System.Web.Routing;
using MvcContrib.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Routing
{
	/// <summary>
	/// Contains tests for the <see cref="RegexRoute"/> class.
	/// </summary>
	[TestFixture]
	public class RegexRouteTests
	{
		private IRouteHandler handler;
		private MockRepository mr;

		/// <summary>
		/// Before each test we can consolidate a couple of lines.
		/// handler actually shouldn't be used; it is only needed 
		/// to pass through to the <see cref="RouteData"/> datatype.
		/// </summary>
		[SetUp]
		public void beforeEachTest()
		{
			mr = new MockRepository();
			handler = mr.DynamicMock<IRouteHandler>();
		}

		/// <summary>
		/// Apparently routes are supposed to return null if they don't match
		/// (if the url is not for this route)
		/// </summary>
		[Test]
		public void ShouldReturnNullIfRouteIsNotMatched()
		{
			var route = new RegexRoute("asdf", handler);
			RouteData routeData = route.GetRouteData("fdsa");
			Assert.IsNull(routeData, "routedata should be null if the path didn't match");
		}

		/// <summary>
		/// The simplest use case would be if the route matched 
		/// exactly and there is no data in a <see cref="RouteValueDictionary"/>
		/// </summary>
		[Test]
		public void ShouldNotBeNullIfRouteIsMatched()
		{
			var route = new RegexRoute("asdf", handler);
			RouteData routeData = route.GetRouteData("asdf");
			Assert.AreSame(handler, routeData.RouteHandler);
			Assert.IsNotNull(routeData, "routeData should not be null if the path matched the regex");
		}

		/// <summary>
		/// A slightly more complex use case is if a <see cref="RouteValueDictionary"/>
		/// associated with the route has data in it, but the route has no way to 
		/// override the values in the dictionary.
		/// </summary>
		[Test]
		public void ShouldHaveDefaultValuesIfPairFoundButDoesNotMatchAnything()
		{
			var defaults = new RouteValueDictionary(new {a = "A"});
			var route = new RegexRoute("asdf", defaults, handler);
			RouteData routeData = route.GetRouteData("asdf");
			Assert.AreSame(handler, routeData.RouteHandler);
			Assert.IsNotNull(routeData, "routeData should not be null if the path matched the regex");
			Assert.IsTrue(routeData.Values.ContainsKey("a"));
			Assert.AreEqual("A", routeData.Values["a"], "if the match failed, the value should be default");
		}

		/// <summary>
		/// Here is a potential real world use case where a route 
		/// describes how to figure out the controller, action and id.
		/// </summary>
		[Test]
		public void ShouldHaveMatchedPairIfValueFound()
		{
			var route = new RegexRoute(@"(?<Controller>[a-zA-Z]+)_((?<Action>[a-zA-Z]+)_)?(?<Id>\d+)?", handler);
			RouteData routeData = route.GetRouteData("Products_View_0");
			Assert.AreSame(handler, routeData.RouteHandler);
			Assert.AreEqual("Products", routeData.Values["Controller"]);
			Assert.AreEqual("View", routeData.Values["Action"]);
			Assert.AreEqual("0", routeData.Values["Id"]);
		}

		/// <summary>
		/// The route generator should generate a route using the url generator parameter.
		/// In this test I am generating a virtual path for a "Sales" controller.
		/// One wierd thing about the MVC engine is that the GetVirtualPath method is coupled 
		/// to a <see cref="RequestContext"/>. In order to generate a virtual path from a route you
		/// need to be in an active request context(even though a route should be the same no 
		/// matter where the context it is coming from is). So here I am faking this context.
		/// 
		/// Unless I am wrong and a route is supposed to override the defaults with 
		/// the values from the last request and override those values with the values sent in.
		/// Since there is no documentation available for this and I haven't actually tested this 
		/// idea I am leaving it as just a thought at the moment.
		/// </summary>
		[Test]
		public void RouteGeneratorGeneratesValidRoute()
		{
			var defaults = new RouteValueDictionary(new {Action = "View", Id = 0});
			var route = new RegexRoute(@"(?<Controller>[a-zA-Z]+)(_(?<Action>[a-zA-Z]+))?(_?<Id>\d+)?",
			                           "{Controller}_{Action}_{Id}", defaults, handler);
			RouteData routeData = route.GetRouteData("Products");
			VirtualPathData pathData = route.GetVirtualPath(new RequestContext(mr.PartialMock<HttpContextBase>(), routeData),
			                                                new RouteValueDictionary(new {Controller = "Sales"}));
			Assert.AreEqual("Sales_View_0", pathData.VirtualPath);
		}

		/// <summary>
		/// When you have a route and are trying to generate a virtual path but haven't 
		/// set any defaults to the route, you have to set all the values.
		/// </summary>
		[Test]
		public void RouteGeneratorGeneratesValidRouteWithoutDefaults()
		{
			var route = new RegexRoute(@"(?<Controller>[a-zA-Z]+)(_(?<Action>[a-zA-Z]+))?(_?<Id>\d+)?",
			                           "{Controller}_{Action}_{Id}", handler);
			RouteData routeData = route.GetRouteData("Products_View_0");
			VirtualPathData pathData = route.GetVirtualPath(new RequestContext(mr.PartialMock<HttpContextBase>(), routeData),
			                                                new RouteValueDictionary(
			                                                	new {Controller = "Accounts", Action = "Delete", Id = 0}));
			Assert.AreEqual("Accounts_Delete_0", pathData.VirtualPath);
		}

		/// <summary>
		/// It seems that the route generator is supposed to return null if not all the parameters are available. 
		/// This appears to be how it figures out if this is the correct route for a given controller.
		/// </summary>
		[Test]
		public void RouteGeneratorReturnsNullWhenNotAllParametersFilledIn()
		{
			var route = new RegexRoute(@"(?<Controller>[a-zA-Z]+)(_(?<Action>[a-zA-Z]+))?(_?<Id>\d+)?",
			                           "{Controller}_{Action}_{Id}", handler);
			RouteData routeData = route.GetRouteData("Products_View_0");
			VirtualPathData pathData = route.GetVirtualPath(new RequestContext(mr.PartialMock<HttpContextBase>(), routeData),
			                                                new RouteValueDictionary(
			                                                	new {Controller = "Accounts", Action = "Delete"}));
			Assert.IsNull(pathData);
		}

		/// <summary>
		/// One thing noticable about a regular expression is that it isn't always easy to provide a generator string.
		/// To compensate for this <see cref="RegexRoute"/> allows the user to provide a delegate to generate the
		/// virtual path instead of a string for the class itself to generate the path.
		/// </summary>
		[Test]
		public void RouteGeneratorCallsCustomFunctionIfProvided()
		{
			bool called = false;
			RequestContext innerContext = null;
			RouteValueDictionary innerRouteValues = null;
			RegexRoute innerRoute = null;
			VirtualPathData innerPathData = null;
			var route = new RegexRoute(
				@"(?<Controller>[a-zA-Z]+)(_(?<Action>[a-zA-Z]+))?(_?<Id>\d+)?",
				delegate(RequestContext context, RouteValueDictionary routeValues, RegexRoute thisRoute)
					{
						called = true;
						innerContext = context;
						innerRouteValues = routeValues;
						innerRoute = thisRoute;
						innerPathData = new VirtualPathData(thisRoute, "");
						return innerPathData;
					},
				handler);
			RouteData routeData = route.GetRouteData("Products_View_0");
			var values = new RouteValueDictionary(new {Controller = "Accounts", Action = "Delete", Id = 0});
			var requestContext = new RequestContext(mr.PartialMock<HttpContextBase>(), routeData);
			VirtualPathData pathData = route.GetVirtualPath(
				requestContext,
				values);
			Assert.IsTrue(called);
			Assert.IsNotNull(innerContext);
			Assert.AreSame(requestContext, innerContext);
			Assert.IsNotNull(innerRouteValues);
			Assert.AreSame(values, innerRouteValues);
			Assert.IsNotNull(innerRoute);
			Assert.AreSame(route, innerRoute);
			Assert.IsNotNull(pathData);
			Assert.AreSame(pathData, innerPathData);
		}

		/// <summary>
		/// In order for the route engine to actually work, I have to override the real 
		/// GetRouteData method. So here I am mocking the expected contents of the method
		/// so it calls the actual implementation of GetRouteData.
		/// </summary>
		[Test]
		public void GetRouteDataHttpContextCallsGetRouteDataString()
		{
			var httpContext = mr.PartialMock<HttpContextBase>();
			var httpRequest = mr.PartialMock<HttpRequestBase>();
			var route = mr.PartialMock<RegexRoute>("asdf", handler);
			var routeData = mr.PartialMock<RouteData>();
			using(mr.Record())
			{
				Expect.Call(httpContext.Request).Return(httpRequest);
				Expect.Call(httpRequest.AppRelativeCurrentExecutionFilePath).Return("~/Products_View_0");
				Expect.Call(httpContext.Request).Return(httpRequest);
				Expect.Call(httpRequest.PathInfo).Return("/pathinfo");
				Expect.Call(route.GetRouteData("Products_View_0/pathinfo")).Return(routeData);
			}
			using(mr.Playback())
			{
				RouteData returnedRouteData = route.GetRouteData(httpContext);
				Assert.AreSame(routeData, returnedRouteData);
			}
			mr.VerifyAll();
		}
	}
}