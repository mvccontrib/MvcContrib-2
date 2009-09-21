using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper;
using NUnit.Framework.SyntaxHelpers;
using Assert=NUnit.Framework.Assert;

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class ActionResultHelperTester
	{
		public class SampleObject
		{
			public string Name { get; set; }
		}

		public class SampleController : Controller
		{
			public RedirectToRouteResult TestMethod()
			{
				return
					this.RedirectToAction(c => c.SomeOtherMethod(1, new SampleObject {Name = "name"}));
			}

			public ActionResult SomeOtherMethod(int number, SampleObject obj)
			{
				return View();
			}
		}

		[Test]
		public void Should_convert()
		{
			ActionResult result = new EmptyResult();
			var converted = result.AssertResultIs<EmptyResult>();
			Assert.IsNotNull(converted);
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected result to be of type EmptyResult. It is actually of type RedirectResult.")]
		public void Should_throw_when_conversiontype_is_incorrect()
		{
			ActionResult result = new RedirectResult("http://mvccontrib.org");
			result.AssertResultIs<EmptyResult>();
		}

		[Test]
		public void Should_convert_to_ViewResult()
		{
			ActionResult result = new ViewResult();
			ViewResult converted = result.AssertViewRendered();
			Assert.IsNotNull(converted);
        }

		[Test]
		public void Should_convert_to_RedirectToRouteResult()
		{
			ActionResult result = new RedirectToRouteResult(new RouteValueDictionary());
			RedirectToRouteResult converted = result.AssertActionRedirect();
			Assert.IsNotNull(converted);
		}

		[Test]
		public void Should_convert_to_HttpRedirectResult()
		{
			ActionResult result = new RedirectResult("http://mvccontrib.org");
			RedirectResult converted = result.AssertHttpRedirect();
			Assert.IsNotNull(converted);
		}

		[Test]
		public void WithParameter_should_return_source_result()
		{
			var result = new RedirectToRouteResult(new RouteValueDictionary(new { foo = "bar" }));
			var final = result.WithParameter("foo", "bar");
			Assert.That(final, Is.EqualTo(result));
		}

		[Test]
		public void GetParameter_should_return_value_type_parameters()
		{
			var controller = new SampleController();
			var result = controller.TestMethod();
			var parameter = result.GetStronglyTypedParameter(controller, "number");
			Assert.That(parameter, Is.EqualTo(1));
		}

		[Test]
		public void GetParameter_should_return_reference_type_parameters()
		{
			var controller = new SampleController();
			var result = controller.TestMethod();
			var parameter = (SampleObject)result.GetStronglyTypedParameter(controller, "obj");
			Assert.That(parameter.Name, Is.EqualTo("name"));
		}

        [Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Could not find a parameter named 'foo' in the result's Values collection.")]
		public void WithParameter_should_throw_if_key_not_in_dictionary()
		{
			var result = new RedirectToRouteResult(new RouteValueDictionary());
			result.WithParameter("foo", "bar");
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "When looking for a parameter named 'foo', expected 'bar' but was 'baz'.")]
		public void WithParameter_should_throw_if_values_are_different()
		{
			var result = new RedirectToRouteResult(new RouteValueDictionary(new { foo = "baz" }));
			result.WithParameter("foo", "bar");
		}

		[Test]
		public void ForView_should_return_source_result()
		{
			var result = new ViewResult { ViewName = "Index" };
			var final = result.ForView("Index");
			Assert.That(final, Is.EqualTo(result));
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected view name 'Index', actual was 'About'")]
		public void ForView_should_throw_if_view_names_do_not_match()
		{
			var result = new ViewResult {ViewName = "About"};
			result.ForView("Index");
		}

		[Test]
		public void ForUrl_should_return_source_Result()
		{
			var result = new RedirectResult("http://mvccontrib.org");
			var final = result.ToUrl("http://mvccontrib.org");
			Assert.That(final, Is.EqualTo(result));
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected redirect to 'http://mvccontrib.org', actual was 'http://www.asp.net'")]
		public void ForUrl_should_throw_if_urls_do_not_match()
		{
			var result = new RedirectResult("http://www.asp.net");
			result.ToUrl("http://mvccontrib.org");
		}

		[Test]
		public void Should_chain()
		{
			ActionResult result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Home", action = "Index", id = 1}));
			var final = result.AssertActionRedirect().ToController("Home").ToAction("Index").WithParameter("id", 1);
			Assert.That(final, Is.EqualTo(result));
		}

		[Test]
		public void ToAction_should_support_strongly_typed_controller_and_action()
		{
				ActionResult result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "PageHandler", action = "About" }));
				var final = result.AssertActionRedirect().ToAction<PageHandler>(c => c.About());
				Assert.That(final, Is.EqualTo(result));
		}

		[Test]
		public void ToAction_with_strongly_typed_controller_can_ignore_the_controller_suffix()
		{
			ActionResult result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Fake", action = "About" }));
			var final = result.AssertActionRedirect().ToAction<FakeController>(c => c.About());
			Assert.That(final, Is.EqualTo(result));
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected view data of type 'CustomReferenceTypeViewData', actual was 'String'")]
		public void WithViewData_should_throw_if_view_data_type_does_not_match()
		{
			const string wrongViewDataType = "WrongType";
			var result = new ViewResult { ViewData = new ViewDataDictionary(wrongViewDataType)};

			result.WithViewData<CustomReferenceTypeViewData>();
		}

		[Test]
		public void WithViewData_should_return_view_data_if_view_data_type_matches()
		{
			var expectedData = new CustomReferenceTypeViewData {ID = 2, Name = "Foo"};
			var renderResult = new ViewResult {ViewData = new ViewDataDictionary(expectedData)};

			var result = renderResult.WithViewData<CustomReferenceTypeViewData>();

			Assert.That(result, Is.EqualTo(expectedData));
		}

		[Test]
		public void WithViewData_should_return_view_data_if_view_data_type_is_subclass_of_expected_type()
		{
			var expectedData = new DerivedCustomViewData {ID = 2, Name = "Foo"};
			var renderResult = new ViewResult {ViewData = new ViewDataDictionary(expectedData)};

			var result = renderResult.WithViewData<CustomReferenceTypeViewData>();

			Assert.That(result, Is.EqualTo(expectedData));
		}

		[Test]
		public void WithViewData_should_return_null_if_view_data_is_null_and_expected_type_is_reference_type()
		{
			var renderResult = new ViewResult {ViewData = new ViewDataDictionary<CustomReferenceTypeViewData>() };

			var result = renderResult.WithViewData<CustomReferenceTypeViewData>();

			Assert.That(result, Is.Null);
		}

		[Test]
		[ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected view data of type 'CustomValueTypeViewData', actual was NULL")]
		public void WithViewData_should_throw_exception_if_view_data_is_null_and_expected_type_is_value_type()
		{
			var renderResult = new ViewResult {ViewData = new ViewDataDictionary<CustomReferenceTypeViewData>() };

			renderResult.WithViewData<CustomValueTypeViewData>();
		}

		[Test]
		public void WithViewData_should_return_view_data_if_view_data_type_is_implementation_of_generic_interface()
		{
			var expectedData = new List<string> { "a", "b", "c" };
			var renderResult = new ViewResult { ViewData = new ViewDataDictionary(expectedData) };

			var result = renderResult.WithViewData<IList<string>>();

			Assert.That(result, Is.EqualTo(expectedData));
		}

		[Test]
		public void Should_convert_to_PartialViewResult()
		{
			ActionResult result = new PartialViewResult();
			result.AssertPartialViewRendered().ShouldNotBeNull();
		}

		class DerivedCustomViewData : CustomReferenceTypeViewData
		{
		}

		class CustomReferenceTypeViewData
		{
			public int ID { get; set; }
			public string Name { get; set; }
		}

		struct CustomValueTypeViewData
		{
			public int ID { get; set; }
			public string Name { get; set; }
		}

		class PageHandler : Controller
		{
			public ActionResult About()
			{
				return null;
			}
		}

		class FakeController : Controller
		{
			public ActionResult About()
			{
				return null;
			}
		}

	}
}
