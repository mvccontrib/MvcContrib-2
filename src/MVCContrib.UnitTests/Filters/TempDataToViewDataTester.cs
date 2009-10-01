using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class TempDataToViewDataTester
	{
		private TempDataToViewDataAttribute _filter;
		private ActionExecutedContext _context;

		[SetUp]
		public void Setup()
		{
			_filter = new TempDataToViewDataAttribute();
			_context = new ActionExecutedContext(new TestController().SetupControllerContext().ControllerContext, MockRepository.GenerateStub<ActionDescriptor>(), false,null);
			_context.Controller.TempData.Add("foo", "bar");
			_context.Controller.TempData.Add("baz", "blah");
		}

		[Test]
		public void When_action_result_is_ViewResult_tempdata_should_be_copied_to_viewdata()
		{
			_context.Result = new ViewResult();

			_filter.OnActionExecuted(_context);
            Assert.That(_context.Controller.ViewData.Count, Is.EqualTo(2));
			Assert.That(_context.Controller.ViewData["foo"], Is.EqualTo("bar"));
			Assert.That(_context.Controller.ViewData["baz"], Is.EqualTo("blah"));
		}

		[Test]
		public void When_action_result_is_not_a_ViewResult_tempdata_should_not_be_copied_to_viewdata()
		{
			_context.Result = new RedirectToRouteResult(new RouteValueDictionary());
			_filter.OnActionExecuted(_context);

			Assert.That(_context.Controller.ViewData.Count, Is.EqualTo(0));
		}

		public class TestController : Controller
		{
		}
	}
}