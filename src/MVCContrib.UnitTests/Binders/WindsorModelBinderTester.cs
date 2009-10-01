using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.Binders
{
	[TestFixture]
	public class WindsorModelBinderTester
	{
		private ModelBindingContext _context;

		[SetUp]
		public void Setup()
		{
		    _context = new ModelBindingContext() { ModelMetadata    =  new ModelMetadata(new EmptyModelMetadataProvider(), null, null,typeof(object), null), ModelName = "test" };
		}

		[Test]
		public void ShouldResolveTheCorrectBinder_WhenBinderExists()
		{
			IWindsorContainer container = new WindsorContainer();

			container.AddComponent<IModelBinder, TestModelBinder>("testmodelbinder");

			var binder = new WindsorModelBinder(container);

			var value = binder.BindModel(new ControllerContext(), _context);

			Assert.That(value, Is.EqualTo("TestResult"));
		}

		[Test]
		public void ShouldFallbackToDefaultBinder_WhenBinderDoesNotExist()
		{
			var container = new WindsorContainer();
			var fallbackBinder = MockRepository.GenerateMock<IModelBinder>();
			fallbackBinder.Expect(b => b.BindModel(null,_context))
				.Return("MockedResult");

			var binder = new WindsorModelBinder(container, fallbackBinder);

			var value = binder.BindModel(new ControllerContext(), _context);

			Assert.That(value, Is.EqualTo("MockedResult"));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrow_WhenComponentIsNotIModelBinder()
		{
			var container = new WindsorContainer();
			container.AddComponent<object>("testmodelbinder");

			var binder = new WindsorModelBinder(container);
			binder.BindModel(new ControllerContext(),  _context);
		}

		public class TestModelBinder : IModelBinder
		{
		    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		    {
		        return "TestResult";
		    }
		}
	}
}
