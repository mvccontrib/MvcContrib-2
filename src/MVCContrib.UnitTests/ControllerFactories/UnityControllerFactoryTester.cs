using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MvcContrib.Unity;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.ControllerFactories
{
	[TestFixture, Category("UnityFactory")]
	public class UnityControllerFactoryTester
	{
		private MockRepository _mocks;
		private IUnityContainer _container;
		private RequestContext _context;
		private IControllerFactory _factory;
		private HttpContextBase _mockContext;

		[SetUp]
		public void Setup()
		{
			_container = new UnityContainer();
			_container.RegisterType<UnitySimpleController, UnitySimpleController>();
			_container.RegisterType<IDependency, StubDependency>();
			_container.RegisterType<UnityDependencyController, UnityDependencyController>();

			_mocks = new MockRepository();
			_mockContext = _mocks.PartialMock<HttpContextBase>();
			_mockContext.Stub(c => c.ApplicationInstance).Return(new MockApplication(_container));
			_mocks.ReplayAll();

			_context = new RequestContext(_mockContext, new RouteData());
			_factory = new UnityControllerFactory();
			_factory.InitializeWithControllerTypes(typeof(UnitySimpleController), typeof(UnityDependencyController));

		}

		[Test]
		public void ShouldReturnTheController()
		{
			IController controller = _factory.CreateController(_context, "UnitySimple"); //typeof(SimpleController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(UnitySimpleController)));
		}

		[Test]
		public void ShouldReturnControllerWithDependencies()
		{
			IController controller = _factory.CreateController(_context, "UnityDependency"); //typeof(DependencyController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(UnityDependencyController)));

			var dependencyController = (UnityDependencyController)controller;
			Assert.That(dependencyController._dependency, Is.Not.Null);
			Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenContainerIsNull()
		{
			_mockContext.BackToRecord(BackToRecordOptions.All);
			_mockContext.Stub(c => c.ApplicationInstance).Return(new MockApplication(null));
			_mockContext.Replay();

			_factory.CreateController(_context, "UnitySimple");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenApplicationDoesNotImplementIContainerAccessor()
		{
			_mockContext.BackToRecord(BackToRecordOptions.All);
			_mockContext.Stub(c => c.ApplicationInstance).Return(new HttpApplication());
			_mockContext.Replay();

			_factory.CreateController(_context, "UnitySimple");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenControllerDoesNotExist()
		{
			_factory.CreateController(_context, "ControllerThatDoesNotExist");
		}

		public class MockApplication : HttpApplication, IUnityContainerAccessor
		{
			private readonly IUnityContainer _container;

			public MockApplication(IUnityContainer container)
			{
				_container = container;
			}

			public IUnityContainer Container
			{
				get { return _container; }
			}
		}

		public class UnitySimpleController : IController
		{
			public void Execute(RequestContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public class UnityDependencyController : IController
		{
			public IDependency _dependency;

			public UnityDependencyController(IDependency dependency)
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