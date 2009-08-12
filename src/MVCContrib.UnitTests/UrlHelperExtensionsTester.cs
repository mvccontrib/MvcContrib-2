using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class UrlHelperExtensionsTester
	{
		private UrlHelper _urlHelper;

		[SetUp]
		public void Setup()
		{
			_urlHelper = new UrlHelper(new RequestContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData()), new RouteCollection());
			_urlHelper.RequestContext.HttpContext.Response.Stub(o => o.ApplyAppPathModifier(null)).IgnoreArguments().Do((Func<string, string>)(s => s));
			_urlHelper.RouteCollection.MapRoute("default", "{controller}/{action}/{id}", new { controller = "home", action = "index", id = "" });
		}

		[Test]
		public void Builds_url_from_strongly_typed_expression()
		{
			string url = _urlHelper.Action<TestController>(c => c.Index());
			url.ShouldEqual("/Test");
		}

		private class TestController : Controller
		{
			public ActionResult Index() { return null; }
		}
	}
}