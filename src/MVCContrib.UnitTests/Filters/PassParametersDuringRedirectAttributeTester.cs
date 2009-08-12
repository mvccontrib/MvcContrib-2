using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Filters;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class When_an_action_is_executing
	{
		private SampleController _controller;
		private ActionExecutingContext _filterContext;
		private SomeObject _someObject;
		private PassParametersDuringRedirectAttribute _attribute;

		[SetUp]
		public void Establish_context()
		{
			_attribute = new PassParametersDuringRedirectAttribute();
			_controller = new SampleController();
			_filterContext = new ActionExecutingContext();
			_filterContext.ActionParameters = new Dictionary<string, object>();
			_filterContext.Controller = _controller;
			_someObject = new SomeObject {One = 1, Two = "two"};

			_controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "param1"] = _someObject;
			_controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "param2"] = 5;
		}

		[Test]
		public void Should_load_the_parameter_values_out_of_TempData()
		{
			_attribute.OnActionExecuting(_filterContext);
			_filterContext.ActionParameters["param1"].ShouldEqual(_someObject);
			_filterContext.ActionParameters["param2"].ShouldEqual(5);
		}
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