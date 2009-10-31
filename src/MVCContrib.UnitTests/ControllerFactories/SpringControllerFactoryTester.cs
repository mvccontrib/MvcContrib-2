using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Spring;
using NUnit.Framework;
using Spring.Core.IO;
using Spring.Objects.Factory.Xml;

namespace MvcContrib.UnitTests.ControllerFactories
{
	[TestFixture]
	public class SpringControllerFactoryTester
	{
		private IControllerFactory _factory;

		[SetUp]
		public void Setup()
		{
			string objectXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " +
			                   "  <objects xmlns=\"http://www.springframework.net\" " +
			                   "    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
			                   "    xsi:schemaLocation=\"http://www.springframework.net http://www.springframework.net/xsd/spring-objects.xsd\"> " +
			                   "    <object id=\"SimpleController\" singleton=\"false\" type=\"" +
			                   typeof(SpringSimpleController).FullName + "\"/> " +
			                   "    <object id=\"DisposableController\" singleton=\"false\" type=\"" +
			                   typeof(SpringDisposableController).FullName + "\"/> " +
			                   "  </objects>";

			var stream = new MemoryStream(Encoding.Default.GetBytes(objectXml));
			var factory = new XmlObjectFactory(new InputStreamResource(stream, "In memory xml"));

			SpringControllerFactory.Configure(factory);

			_factory = new SpringControllerFactory();
		}

		[Test]
		public void Should_throw_when_not_configured()
		{
			SpringControllerFactory.Configure(null);
			Assert.Throws<ArgumentException>(() => _factory.CreateController(null, "Simple"));
		}

		[Test]
		public void Should_resolve_controller_type_by_name()
		{
			var controller = _factory.CreateController(null, "Simple");
			controller.ShouldBe<SpringSimpleController>();
		}

		[Test]
		public void Dispose_should_call_dispose_on_controller()
		{
			var controller = new SpringDisposableController();
			_factory.ReleaseController(controller);
			controller.IsDisposed.ShouldBeTrue();
		}

		[Test]
		public void Should_throw_for_invalid_controller()
		{
			Assert.Throws<InvalidOperationException>(() => _factory.CreateController(null, "foo"));
		}

		private class SpringSimpleController : IController
		{
			public void Execute(RequestContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		private class SpringDisposableController : IController, IDisposable
		{
			public bool IsDisposed;

			public void Dispose()
			{
				IsDisposed = true;
			}

			public void Execute(RequestContext controllerContext) {}
		}
	}
}