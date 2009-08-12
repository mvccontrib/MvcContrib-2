using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Components.Binder;
using MvcContrib.Attributes;
using MvcContrib.Castle;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class CastleBinderTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = CreateControllerContext(_mocks.DynamicMock<ControllerBase>());
		}

		private ControllerContext CreateControllerContext(ControllerBase controller)
		{
			var context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
			_mocks.ReplayAll();
			return context;
		}

		[Test]
		public void Should_bind_using_parameter_name()
		{
			_context.HttpContext.Request.Form["cust.Id"] = "5";
			_context.HttpContext.Request.Form["cust.Name"] = "Jeremy";

			var binder = new CastleBindAttribute();
			object value = binder.BindModel(_context, CreateContext("cust", typeof(Customer)));
			var customer = value as Customer;

			Assert.That(customer, Is.Not.Null);
			Assert.That(customer.Name, Is.EqualTo("Jeremy"));
			Assert.That(customer.Id, Is.EqualTo(5));
		}

		[Test]
		public void Should_bind_using_custom_prefix()
		{
			_context.HttpContext.Request.Form["cust.Id"] = "5";
			_context.HttpContext.Request.Form["cust.Name"] = "Jeremy";

			var binder = new CastleBindAttribute("cust");
			object value = binder.BindModel(_context, CreateContext("Foo", typeof(Customer)));
			var customer = value as Customer;

			Assert.That(customer, Is.Not.Null);
			Assert.That(customer.Name, Is.EqualTo("Jeremy"));
			Assert.That(customer.Id, Is.EqualTo(5));
		}

		[Test]
		public void
			When_the_controller_implements_ICastleBindingContainer_then_the_binder_should_be_made_accessible_to_the_controller()
		{
			var controller = new CastleBindableController();
			_context = CreateControllerContext(controller);

			var binder = new CastleBindAttribute();
			binder.BindModel(_context, CreateContext("cust", typeof(Customer)));

			Assert.That(controller.Binder, Is.Not.Null);
		}

		[Test]
		public void When_the_controller_implements_ICastleBindingContainer_and_the_binder_is_already_set_then_it_should_be_used()
		{
			var castleBinder = new DataBinder();
			var controller = new CastleBindableController {Binder = castleBinder};

			_context = CreateControllerContext(controller);
			_context.HttpContext.Request.Form["cust.Id"] = "Fail";

			var binder = new CastleBindAttribute();
			binder.BindModel(_context, CreateContext("cust", typeof(Customer)));

			Assert.That(controller.Binder, Is.SameAs(castleBinder));
			Assert.That(castleBinder.ErrorList["Id"], Is.Not.Null);
		}

		[Test]
		public void Should_only_bind_from_Form_by_default()
		{
			_context.HttpContext.Request.QueryString["cust.Id"] = "5";

			var binder = new CastleBindAttribute();
			var customer = (Customer)binder.BindModel(_context, CreateContext("cust", typeof(Customer)));

			Assert.That(customer.Id, Is.EqualTo(0));
		}

		[Test]
		public void Should_be_configurable_to_bind_from_other_sources()
		{
			_context.HttpContext.Request.QueryString["cust.Id"] = "5";

			var binder = new CastleBindAttribute(RequestStore.QueryString);
			var customer = (Customer)binder.BindModel(_context, CreateContext("cust", typeof(Customer)));

			Assert.That(customer.Id, Is.EqualTo(5));
		}

		private ModelBindingContext CreateContext(string name, Type type)
		{
			var context = new ModelBindingContext();
			context.ModelType = type;
			context.ModelName = name;
			return context;
		}

		public class Customer
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}

		public class CastleBindableController : Controller, ICastleBindingContainer
		{
			public IDataBinder Binder { get; set; }
		}
	}
}