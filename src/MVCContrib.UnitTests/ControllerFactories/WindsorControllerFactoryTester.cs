using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using MvcContrib.UnitTests.ControllerFactories;
namespace MvcContrib.UnitTests.Castle
{
	[TestFixture]
	public class WindsorControllerFactoryTester
	{
		private IWindsorContainer _container;
		private IControllerFactory _factory;
		private RequestContext _context;
		[SetUp]
		public void Setup()
		{
			_container = new WindsorContainer();
			_factory = new WindsorControllerFactory(_container);

			_container.AddComponent("simplecontroller", typeof(WindsorSimpleController));
			_container.AddComponent("StubDependency", typeof(IDependency), typeof(StubDependency));
			_container.AddComponent("dependencycontroller", typeof(WindsorDependencyController));

			_factory.InitializeWithControllerTypes(typeof(WindsorSimpleController), typeof(WindsorDependencyController));

			var mocks = new MockRepository();
			_context = new RequestContext(mocks.DynamicHttpContextBase(), new RouteData());
			mocks.ReplayAll();
		}

		[Test]
		public void ShouldReturnTheController()
		{
			IController controller = _factory.CreateController(_context, "WindsorSimple");

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(WindsorSimpleController)));
		}

		[Test, ExpectedException(typeof(HttpException), ExpectedMessage = "The controller for path '' could not be found or it does not implement IController.")]
		public void Should_throw_http_exception_if_controller_type_does_not_exist()
		{
			_factory.CreateController(_context, "DoesNotExist");
		}

		[Test]
		public void ShouldReturnControllerWithDependencies()
		{
			IController controller = _factory.CreateController(_context, "WindsorDependency");

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(WindsorDependencyController)));

			var dependencyController = (WindsorDependencyController)controller;
			Assert.That(dependencyController._dependency, Is.Not.Null);
			Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionWhenContainerIsNull()
		{
			new WindsorControllerFactory(null);
		}

		[Test]
		public void ShouldDisposeOfController()
		{
			var controller = new WindsorDisposableController();
			_factory.ReleaseController(controller);
			Assert.That(controller.IsDisposed);
		}

		[Test]
		public void ShouldReleaseController()
		{
			var mockContainer = MockRepository.GenerateStub<IWindsorContainer>();
			var controller = new WindsorSimpleController();
			var factory = new WindsorControllerFactory(mockContainer);

			factory.ReleaseController(controller);

			mockContainer.AssertWasCalled(c => c.Release(controller));
		}

		public class WindsorDisposableController : IDisposable, IController
		{
			public bool IsDisposed;

			public WindsorDisposableController()
			{
				IsDisposed = false;
			}

			public void Dispose()
			{
				IsDisposed = true;
			}

			public void Execute(RequestContext controllerContext)
			{
			}
		}

		public class MockApplication : HttpApplication, IContainerAccessor
		{
			private readonly IWindsorContainer _container;

			public MockApplication(IWindsorContainer container)
			{
				_container = container;
			}

			public IWindsorContainer Container
			{
				get { return _container; }
			}
		}

		public class WindsorSimpleController : IController
		{
			public void Execute(RequestContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public class WindsorDependencyController : IController
		{
			public IDependency _dependency;

			public WindsorDependencyController(IDependency dependency)
			{
				_dependency = dependency;
			}

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
}