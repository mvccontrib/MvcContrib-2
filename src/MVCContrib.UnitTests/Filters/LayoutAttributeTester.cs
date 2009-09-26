using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class LayoutAttributeTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controllerContext = new ControllerContext(_mocks.DynamicMock<HttpContextBase>(), new RouteData(), _mocks.PartialMock<Controller>());
		}

		[Test]
		public void Should_set_layout_to_view_results()
		{
			var result = new ViewResult();
			var context = new ResultExecutingContext(_controllerContext, result);
			var layoutAttribute = new LayoutAttribute("test");
			layoutAttribute.OnResultExecuting(context);

			Assert.That(result.MasterName, Is.EqualTo("test"));
		}

		[Test]
		public void Should_override_layout_to_view_results_that_explicitly_set_layout()
		{
			var result = new ViewResult {MasterName = "explicit"};
			var context = new ResultExecutingContext(_controllerContext, result);
			var layoutAttribute = new LayoutAttribute("test");
			layoutAttribute.OnResultExecuting(context);

			Assert.That(result.MasterName, Is.EqualTo("test"));
		}
	}
}