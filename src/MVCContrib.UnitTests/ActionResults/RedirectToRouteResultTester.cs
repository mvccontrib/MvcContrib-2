using System.Web.Mvc;
using MvcContrib.ActionResults;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class RedirectToRouteResultTester
	{
		[Test]
		public void Should_build_route_values_for_expression()
		{
			var redirect = new RedirectToRouteResult<TestController>(c => c.Index());
			redirect.RouteValues["Controller"].ShouldEqual("Test");
			redirect.RouteValues["Action"].ShouldEqual("Index");
		}

		private class TestController : Controller
		{
			public ActionResult Index() { return null; }
		}
	}
}