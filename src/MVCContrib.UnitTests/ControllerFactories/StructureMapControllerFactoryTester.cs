using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.StructureMap;
using NUnit.Framework;

using StructureMap;

namespace MvcContrib.UnitTests.ControllerFactories
{
	public class StructureMapControllerFactoryTester
	{
		[TestFixture]
		public class WhenAValidControllerTypeIsPassed
		{
			[SetUp]
			public void Setup()
			{
				ObjectFactory.Initialize(x =>
				{
					x.UseDefaultStructureMapConfigFile = false;
					x.BuildInstancesOf<IController>();
					x.BuildInstancesOf<StructureMapSimpleController>().TheDefaultIsConcreteType<StructureMapSimpleController>();
					x.BuildInstancesOf<IDependency>().TheDefaultIsConcreteType<StubDependency>();
					x.BuildInstancesOf<StructureMapDependencyController>().TheDefaultIsConcreteType<StructureMapDependencyController>();
				});
			}

			[Test]
			public void ShouldReturnTheController()
			{
				var requestContext = new RequestContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData());

				IControllerFactory factory = new StructureMapControllerFactory();
				factory.InitializeWithControllerTypes(typeof(StructureMapSimpleController), typeof(StructureMapDependencyController));
				IController controller = factory.CreateController(requestContext, "StructureMapSimple");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(StructureMapSimpleController)));
			}

			[Test]
			public void ShouldReturnControllerWithDependencies()
			{
				IControllerFactory factory = new StructureMapControllerFactory();
				factory.InitializeWithControllerTypes(typeof(StructureMapSimpleController), typeof(StructureMapDependencyController));

				var requestContext = new RequestContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData());

				IController controller = factory.CreateController(requestContext, "StructureMapDependency");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(StructureMapDependencyController)));

				var dependencyController = (StructureMapDependencyController)controller;
				Assert.That(dependencyController._dependency, Is.Not.Null);
				Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
			}

		    public class StructureMapDependencyController : IController
			{
				public IDependency _dependency;

				public StructureMapDependencyController(IDependency dependency)
				{
					_dependency = dependency;
				}

				public void Execute(RequestContext controllerContext)
				{
					throw new NotImplementedException();
				}
			}

			public class TestRegistry
			{
				
			}

			public interface IDependency
			{
			}

			public class StubDependency : IDependency
			{
			}
		}

	    public class StructureMapSimpleController : IController
	    {
	        public void Execute(RequestContext controllerContext)
	        {
	            throw new NotImplementedException();
	        }
	    }
	}
}
