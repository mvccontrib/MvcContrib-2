using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.ActionResults;
using MvcContrib.Filters;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class PassParametersDuringRedirectAttributeTester
	{
		private PassParametersDuringRedirectAttribute _filter;
		private SomeObject _someObject;

		[SetUp]
		public void Setup()
		{
			_filter = new PassParametersDuringRedirectAttribute();
			_someObject = new SomeObject {One = 1, Two = "two"};
		}

		[Test]
		public void OnActionExecuting_should_load_the_parameter_values_out_of_TempData()
		{
			var context = new ActionExecutingContext()
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>()
			};


			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "param1"] = _someObject;
			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "param2"] = 5;

			_filter.OnActionExecuting(context);

			context.ActionParameters["param1"].ShouldEqual(_someObject);
			context.ActionParameters["param2"].ShouldEqual(5);
		}

		[Test]
		public void OnActionExecuted_should_store_parameters_in_tempdata_when_result_is_generic_RedirectToRouteResult()
		{
			var context = new ActionExecutedContext()
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Index(_someObject, 5)),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);
			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"].ShouldEqual(_someObject);
			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "id"].ShouldEqual(5);
		}

		[Test]
		public void Should_not_remove_null_parameters_from_the_route_values() 
		{
			var context = new ActionExecutedContext
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Index(null, 5)),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"].ShouldBeNull();
		}

		public class SomeObject
		{
			public int One { get; set; }
			public string Two { get; set; }
		}

		public class SampleController : Controller
		{
			public ActionResult Index(SomeObject viewModel, int id)
			{
				return View(viewModel);
			}

			public ActionResult Save(SomeObject updateModel, int someId)
			{
				return this.RedirectToAction(c => c.Index(updateModel, someId));
			}
		}
	}
}