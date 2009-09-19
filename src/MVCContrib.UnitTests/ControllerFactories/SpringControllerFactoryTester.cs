using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Spring;
using NUnit.Framework;

using Spring.Context.Support;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;

namespace MvcContrib.UnitTests.ControllerFactories
{
	public class SpringControllerFactoryTester
	{
		[TestFixture]
		public class WhenConfigureNotCalled
		{
            IControllerFactory factory;
			[SetUp]
			public void Setup()
			{
				//make sure instance variable was not set in another
				//test fixture.  this needs to be done because of the 
				//static field and we need to be sure that test fixtures
				//can be run in any order.
				Type springFactoryType = typeof(SpringControllerFactory);
				FieldInfo factoryField = springFactoryType.GetField("_objectFactory", BindingFlags.NonPublic | BindingFlags.Static);

				
			    factory = new SpringControllerFactory();
			    factoryField.SetValue(factory, null);
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void ShouldThrowExceptionForNoConfig()
			{
				
				IController controller = factory.CreateController(null, "Simple"); 
                //                                                  Type.GetType(
                //                                                    "MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController"));
			}
		}

		[TestFixture]
		public class WhenAValidControllerTypeIsPassed
		{
			[SetUp]
			public void Setup()
			{
				//still investigating configuring objects without using xml for unit test

				string objectXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " +
				                   "  <objects xmlns=\"http://www.springframework.net\" " +
				                   "    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
				                   "    xsi:schemaLocation=\"http://www.springframework.net http://www.springframework.net/xsd/spring-objects.xsd\"> " +
				                   "    <object id=\"SimpleController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SpringSimpleController\"/> " +
				                   "    <object id=\"DependencyController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SpringDependencyController\" > " +
				                   "      <constructor-arg> " +
				                   "        <object type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+StubDependency\" /> " +
				                   "      </constructor-arg> " +
				                   "    </object> " +
				                   "  </objects>";
				Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(objectXml));
				IResource resource = new InputStreamResource(stream, "In memory xml");
				IObjectFactory factory = new XmlObjectFactory(resource);
				SpringControllerFactory.Configure(factory);
			}

			[Test]
			public void ShouldReturnTheController()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null,"Simple");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(SpringSimpleController)));
			}

			[Test]
			public void ShouldReturnControllerWithDependencies()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null, "Dependency");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(SpringDependencyController)));

				var dependencyController = (SpringDependencyController)controller;
				Assert.That(dependencyController._dependency, Is.Not.Null);
				Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
			}

			[Test, ExpectedException(typeof(InvalidOperationException))]
			public void ShouldThrowExceptionForInvalidController()
			{
				IControllerFactory factory = new SpringControllerFactory();
			    IController controller = factory.CreateController(null, "NonValid");//typeof(NonValidController));
			}

            [Test, ExpectedException(typeof(ArgumentNullException))]
			public void ShouldThrowExceptionForNullControllerName()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null, null);
			}

			public class SpringSimpleController : IController
			{
				public void Execute(RequestContext controllerContext)
				{
					throw new NotImplementedException();
				}
			}

			public class SpringDependencyController : IController
			{
				public IDependency _dependency;

				public SpringDependencyController(IDependency dependency)
				{
					_dependency = dependency;
				}

				public void Execute(RequestContext controllerContext)
				{
					throw new NotImplementedException();
				}
			}

			public class NonValidController : IController
			{
				public void Execute(RequestContext controllerContext)
				{
					throw new NotImplementedException();
				}
			}

			public interface IDependency
			{
			}

			public class StubDependency : IDependency
			{
			}
		}

		[TestFixture]
		public class WhenPassedApplicationContext
		{
			[Test]
			public void ShouldConfigureFactory()
			{
				string objectXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " +
				                   "  <objects xmlns=\"http://www.springframework.net\" " +
				                   "    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
				                   "    xsi:schemaLocation=\"http://www.springframework.net http://www.springframework.net/xsd/spring-objects.xsd\"> " +
				                   "    <object id=\"SimpleController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SpringSimpleController\"/> " +
				                   "    <object id=\"DependencyController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SpringDependencyController\" > " +
				                   "      <constructor-arg> " +
				                   "        <object type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+StubDependency\" /> " +
				                   "      </constructor-arg> " +
				                   "    </object> " +
				                   "  </objects>";
				Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(objectXml));
				IResource resource = new InputStreamResource(stream, "In memory xml");
				var ctx = new GenericApplicationContext();
				var reader = new XmlObjectDefinitionReader(ctx);
				reader.LoadObjectDefinitions(resource);
				ctx.Refresh();
				SpringControllerFactory.Configure(ctx);
				IControllerFactory factory = new SpringControllerFactory();
			    IController controller = factory.CreateController(null, "Simple");
				                                                  //Type.GetType("MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController"));

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(WhenAValidControllerTypeIsPassed.SpringSimpleController)));
			}
		}

		[TestFixture]
		public class WhenDisposeControllerInvoked
		{
			[Test]
			public void ControllerShouldBeDisposed()
			{
				var factory = new SpringControllerFactory();
				var controller = new SpringDisposableController();
				factory.ReleaseController(controller);
				Assert.That(controller.IsDisposed);
			
			}

			private class SpringDisposableController : IController, IDisposable
			{
				public bool IsDisposed;

				public void Dispose()
				{
					IsDisposed = true;	
				}

				public void Execute(RequestContext controllerContext)
				{
				}
			}
		}
	}
}
