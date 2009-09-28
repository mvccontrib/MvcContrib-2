using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class SubControllerActionToViewDataAttributeTester
	{
		[Test]
		public void ShouldPushTheActionOfEachSubcontrollerIntoViewdata()
		{
			var c1 = new SubController();
			var c2 = new SubController();

			var attribute = new SubControllerActionToViewDataAttribute();
			var controller = new TestingController();
			ActionExecutingContext context = GetFilterContext(controller);
			context.ActionParameters["c1"] = c1;
			context.ActionParameters["c2"] = c2;

			attribute.OnActionExecuting(context);

			Assert.That(controller.ViewData.Get<Action>("c1"), Is.Not.Null);
			Assert.That(controller.ViewData.Get<Action>("c2"), Is.Not.Null);
		}

		[Test]
		public void ShouldIgnoreActionParametersThatAreNull()
		{
			var attribute = new SubControllerActionToViewDataAttribute();
			var controller = new TestingController();
			ActionExecutingContext context = GetFilterContext(controller);
			context.ActionParameters["c1"] = null;
			context.ActionParameters["c2"] = new SubController();

			attribute.OnActionExecuting(context);

			Assert.That(controller.ViewData.Count, Is.EqualTo(1));
			Assert.That(controller.ViewData.Get<Action>("c2"), Is.Not.Null);
		}


		private static ActionExecutingContext GetFilterContext(ControllerBase controller)
		{
			controller.ControllerContext = new ControllerContext { Controller = controller };
			var actionExecutingContext = new ActionExecutingContext()
			                             	{
			                             		ActionParameters = new Dictionary<string, object>(),
												Controller = controller
			                             	};
			return actionExecutingContext;
		}
	}

	internal class TestingController : Controller
	{
	}
}