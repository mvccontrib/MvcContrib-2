using System;
using System.Web.Mvc;
using MvcContrib.Binders;
using NUnit.Framework;


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
			var bindingContext = new ModelBindingContext { ModelMetadata   =  new ModelMetadata(new  EmptyModelMetadataProvider(), null, null, typeof(FooController), null)};

			object value = binder.BindModel(new ControllerContext(), bindingContext);
			Assert.That(value, Is.InstanceOf<FooController>());
		}

		[Test]
		public void ShouldDeferToDefaultBinderIfNotSubcontroller()
		{
			var binder = new SubControllerBinder();
			var context = new ModelBindingContext
			{
				ModelMetadata = new ModelMetadata(new EmptyModelMetadataProvider(), null, null, typeof(string), null),
				ValueProvider = valueProvider, 
				ModelName = "foo"
		};
			
			object value = binder.BindModel(new ControllerContext(), context);
			Assert.That(value, Is.EqualTo("bar"));
		}
	}
}