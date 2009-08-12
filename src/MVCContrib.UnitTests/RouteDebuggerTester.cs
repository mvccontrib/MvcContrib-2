using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Routing;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class RouteDebuggerTester
	{
		[Test]
		public void Should_rewrite_routes()
		{
			var routes = new RouteCollection
			                 {
			                     new Route("{controller}/{action}/{id}", null, new MvcRouteHandler()),
			                     new Route("{controller}/{action}", null, new MvcRouteHandler())
			                 };

		    RouteDebugger.RewriteRoutesForTesting(routes);

			foreach (Route route in routes)
			{
				Assert.That(route.RouteHandler, Is.InstanceOfType(typeof(DebugRouteHandler)));
			}

		}

		[Test]
		public void Should_add_debug_route()
		{
			var routes = new RouteCollection();
			RouteDebugger.RewriteRoutesForTesting(routes);
			Assert.That(routes[0], Is.EqualTo(DebugRoute.Singleton));
		}

		[Test]
		public void Should_not_add_debug_route_if_already_added()
		{
			var routes = new RouteCollection {DebugRoute.Singleton};

		    RouteDebugger.RewriteRoutesForTesting(routes);
			Assert.That(routes.Count, Is.EqualTo(1));
		}

		[Test]
		public void DebugRouteHandler_should_create_DebugHttpHandler()
		{
			var mocks = new MockRepository();
			var handler = new DebugRouteHandler();
			var context = new RequestContext(mocks.DynamicHttpContextBase(), new RouteData());

			var httpHandler = handler.GetHttpHandler(context);
			Assert.That(httpHandler, Is.InstanceOfType(typeof(DebugHttpHandler)));
			Assert.That(((DebugHttpHandler)httpHandler).RequestContext, Is.SameAs(context));
		}

		[Test]
		public void DebugRoute_should_act_as_singleton()
		{
			var first = DebugRoute.Singleton;
			var second = DebugRoute.Singleton;

			Assert.That(first, Is.SameAs(second));
		}

		[Test]
		public void DebugRoute_should_be_a_catchall_route_using_the_DebugRouteHandler()
		{
			var route = DebugRoute.Singleton;
			Assert.That(route.Url, Is.EqualTo("{*catchall}"));
			Assert.That(route.RouteHandler, Is.InstanceOfType(typeof(DebugRouteHandler)));
		}

		[Test]
		public void Handler_should_not_match_route()
		{
			var handler = new DebugHttpHandler();
			var mocks = new MockRepository();
			var context = mocks.DynamicHttpContextBase();

			SetupResult.For(context.Request.AppRelativeCurrentExecutionFilePath).Return("~/");
			SetupResult.For(context.Request.PathInfo).Return(string.Empty);

			var routes = new RouteCollection();
			RouteDebugger.RewriteRoutesForTesting(routes);

			mocks.ReplayAll();

			var routeData = new RouteData(DebugRoute.Singleton, new DebugRouteHandler());

			handler.RequestContext = new RequestContext(context, routeData);
			handler.ProcessRequest(context, routes);

			Assert.That(context.Response.Output.ToString().Contains("<strong class=\"false\">NO MATCH!</strong>"));
		}

		[Test]
		public void Handler_should_match_route()
		{
			var handler = new DebugHttpHandler();
			var mocks = new MockRepository();
			var context = mocks.DynamicHttpContextBase();

			SetupResult.For(context.Request.AppRelativeCurrentExecutionFilePath).Return("~/Home/");
			SetupResult.For(context.Request.PathInfo).Return("Index");

			var routes = new RouteCollection
			                 {
			                     new Route("{controller}/{action}/{id}", null, new MvcRouteHandler()),
			                     new Route("{controller}/{action}", null, new MvcRouteHandler()),
			                     new Route("{controller}/{action}", new RouteValueDictionary(new Hash(Controller => "Home")),
			                               new MvcRouteHandler())
			                 };
		    RouteDebugger.RewriteRoutesForTesting(routes);

			mocks.ReplayAll();
			var routeData = new RouteData(routes[0], new DebugRouteHandler());
			routeData.Values.Add("Controller", "Home");
			routeData.Values.Add("Action", "Index");

			handler.RequestContext = new RequestContext(context, routeData);
			handler.ProcessRequest(context, routes);

			Assert.That(context.Response.Output.ToString().Contains("<tr><td>Controller</td><td>Home&nbsp;</td></tr>"));
		}

		[Test]
		public void For_Coverage()
		{
			var handler = new DebugHttpHandler();
			Assert.That(handler.IsReusable, Is.True);
		}
	}
}
