using System.Web.Mvc;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class ConventionControllerTester
	{
		private TestController _controller;

		[SetUp]
		public void SetUp()
		{
			_controller = new TestController();
		}

		[Test]
		public void RenderXml_should_return_XmlResult_object()
		{
			var result = _controller.XmlResult() as XmlResult;
			Assert.That(result, Is.Not.Null);
			Assert.That(result.ObjectToSerialize, Is.EqualTo("Test 1 2 3"));
		}

		[Test]
		public void Expression_based_redirect_to_action_should_redirect_correctly_to_same_controller()
		{
			var redirectToRouteResult = _controller.RedirectActionOnSameController();

			Assert.That(redirectToRouteResult.RouteValues["Controller"], Is.EqualTo("Test"));
			Assert.That(redirectToRouteResult.RouteValues["Action"], Is.EqualTo("BasicAction"));
			Assert.That(redirectToRouteResult.RouteValues["Id"], Is.EqualTo(1));
		}

		[Test]
		public void Expression_based_redirect_to_action_should_redirect_correctly_to_another_controller()
		{
			var redirectToRouteResult = _controller.RedirectActionOnAnotherController();

			Assert.That(redirectToRouteResult.RouteValues["Controller"], Is.EqualTo("AnotherTest"));
			Assert.That(redirectToRouteResult.RouteValues["Action"], Is.EqualTo("SomeAction"));
			Assert.That(redirectToRouteResult.RouteValues["Id"], Is.EqualTo(2));
		}

		[Test]
		public void When_a_custom_actioninvoker_is_specified_in_the_constructor_then_the_ActionInvoker_property_should_be_set()
		{
			var invoker = MockRepository.GenerateStub<IActionInvoker>();
			_controller = new TestController(invoker);
			Assert.That(_controller.ActionInvoker, Is.SameAs(invoker));
		}
	}
}