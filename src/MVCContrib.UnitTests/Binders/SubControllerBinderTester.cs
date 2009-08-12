using System.Web.Mvc;
using MvcContrib.Binders;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.Binders
{
	[TestFixture]
	public class SubControllerBinderTester
	{
		private MockValueProvider valueProvider;

		[SetUp]
		public void Setup()
		{
			valueProvider = new MockValueProvider(foo => "bar");
		}

		public class FooController : SubController
		{
		}

		[Test]
		public void ShouldCreateSubcontroller()
		{
			var binder = new SubControllerBinder();
			var bindingContext = new ModelBindingContext {ModelType = typeof(FooController)};

			object value = binder.BindModel(new ControllerContext(), bindingContext);
			Assert.That(value, Is.InstanceOfType(typeof(FooController)));
		}

		[Test]
		public void ShouldDeferToDefaultBinderIfNotSubcontroller()
		{
			var binder = new SubControllerBinder();
			var context = new ModelBindingContext {ValueProvider = valueProvider, ModelName = "foo", ModelType = typeof(string)};
			
			object value = binder.BindModel(new ControllerContext(), context);
			Assert.That(value, Is.EqualTo("bar"));
		}
	}
}