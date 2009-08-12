using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class SubControllerTester
	{
		[Test]
		public void ShouldUseSubControllerActionToViewDataOnIdentification()
		{
			Type type = typeof(SubController<object>);
			object[] attrs = type.GetCustomAttributes(typeof(SubControllerActionToViewDataAttribute), true);

			Assert.That(attrs.Length, Is.EqualTo(1));
		}

		[Test]
		public void ShouldSubclassController()
		{
			Controller controller = new SubController<object>();
			Assert.IsNotNull(controller);
		}

		[Test]
		public void ShouldTrimControllerAndSubcontrollerOffTypeNameToGetControllername()
		{
			var controller = new SubController<object>();
			string name = controller.GetControllerName();

			Assert.AreEqual("", name);

			var controller1 = new FooingSubController();
			string name1 = controller1.GetControllerName();

			Assert.AreEqual("fooing", name1);

			var controller2 = new FooingController();
			string name2 = controller2.GetControllerName();

			Assert.AreEqual("fooing", name2);
		}

		[Test]
		public void ShouldAddControllerAndActionToRouteDataAndContextIsSame()
		{
			var controller = new FooingSubController();
			var parentController = new BaringController();
			parentController.ControllerContext = GetControllerContext();
			RequestContext context = controller.GetNewRequestContextFromController(parentController);

			Assert.That(context.RouteData.Values["controller"], Is.EqualTo("fooing"));
			Assert.That(context.RouteData.Values["action"], Is.EqualTo("fooing"));
			Assert.That(context.HttpContext, Is.SameAs(parentController.HttpContext));
		}

		[Test]
		public void ShouldTakeParentControllerAndReturnAction()
		{
			var controller = new FooingController();
			var parentController = new BaringController();
			parentController.ControllerContext = GetControllerContext();
			Action result = controller.GetResult(parentController);

			Assert.IsNotNull(result);
		}

		private static ControllerContext GetControllerContext()
		{
			var mockRequest = MockRepository.GenerateStub<HttpRequestBase>();
			mockRequest.Stub(r => r.Form).Return(new NameValueCollection()).Repeat.Any();
			mockRequest.Stub(r => r.QueryString).Return(new NameValueCollection()).Repeat.Any();

			var mockHttpContext = MockRepository.GenerateStub<HttpContextBase>();
			mockHttpContext.Stub(c => c.Request).Return(mockRequest).Repeat.Any();

			var routeData = new RouteData();
			return new ControllerContext(mockHttpContext, routeData,
			                             MockRepository.GenerateStub<ControllerBase>());
		}

		private class BaringController : Controller
		{
		}

		private class FooingSubController : SubController<object>
		{
		}

		private class FooingController : SubController<object>
		{
		}
	}
}