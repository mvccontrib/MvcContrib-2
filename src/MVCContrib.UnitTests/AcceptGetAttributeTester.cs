using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Attributes;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class AcceptGetAttributeTester
	{
        protected ActionMethodSelectorAttribute _attribute;
		private MockRepository _mocks;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_attribute = new AcceptGetAttribute();
		}

		private ControllerContext CreateContext(string verb)
		{
			var ctx = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(),
			                                _mocks.DynamicMock<ControllerBase>());
			ctx.HttpContext.Request.Expect(x => x.HttpMethod).Return(verb);
			_mocks.ReplayAll();
			return ctx;
		}

		[Test]
		public void Should_return_true_for_http_get()
		{
			Assert.IsTrue(_attribute.IsValidForRequest(CreateContext("GET"), null));
		}

		[Test]
		public void Should_return_false_for_http_post()
		{
			Assert.IsFalse(_attribute.IsValidForRequest(CreateContext("POST"), null));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Should_throw_if_controller_context_is_null()
		{
			_attribute.IsValidForRequest(null, null);
		}
	}
}