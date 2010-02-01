using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.ControllerFactories.IoCControllerFactoryTester
{
	public class IoCControllerFactoryTester
	{
		[TestFixture]
		public class WhenCreatedWithoutADependencyResolverContainer : SpecBase
		{
			private IDependencyResolver _dependencyResolver;

			protected override void BeforeEachSpec()
			{
				_dependencyResolver = _mocks.StrictMock<IDependencyResolver>();

				_dependencyResolver.Expect(r => r.GetImplementationOf(typeof(IocTestController)))
					.Return(new IocTestController());

				_dependencyResolver.Replay();

				DependencyResolver.InitializeWith(_dependencyResolver);
			}

			[Test]
			public void Should_call_into_the_static_resolver_to_create_a_controller()
			{
				var requestContext = new RequestContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData());

				IControllerFactory controllerFactory = new IoCControllerFactory();
				controllerFactory.InitializeWithControllerTypes(typeof(IocTestController));

				IController controller = controllerFactory.CreateController(requestContext, "IocTest");

				Assert.That(controller, Is.TypeOf(typeof(IocTestController)));
			}

			protected override void AfterEachSpec()
			{
				_dependencyResolver.VerifyAllExpectations();
				_dependencyResolver = null;
				DependencyResolver.InitializeWith(null);
			}
		}

		[TestFixture]
		public class WhenCreatedWithADependencyResolverCcontainer : SpecBase
		{
			private IDependencyResolver _dependencyResolver;

			protected override void BeforeEachSpec()
			{
				_dependencyResolver = _mocks.StrictMock<IDependencyResolver>();
				DependencyResolver.InitializeWith(_dependencyResolver);
			}

			[Test]
			public void Should_call_into_the_resolver_to_create_a_controller()
			{
				var requestContext = new RequestContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData());
				_dependencyResolver.Expect(r => r.GetImplementationOf<IController>(typeof(IocTestController)))
					.Return(new IocTestController());

				_dependencyResolver.Replay();

				IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
				controllerFactory.InitializeWithControllerTypes(typeof(IocTestController));

				IController controller = controllerFactory.CreateController(requestContext, "IocTest");

				Assert.That(controller, Is.TypeOf(typeof(IocTestController)));
				_dependencyResolver.VerifyAllExpectations();
			}

			[Test]
			[ExpectedException(typeof(ArgumentNullException))]
			public void Should_throw_an_argument_null_exception_when_the_resolver_is_null()
			{
				IControllerFactory controllerFactory = new IoCControllerFactory(null);
			}

			[Test, ExpectedException(typeof(ArgumentNullException))]
			public void Should_throw_if_controllerName_is_null()
			{
				IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
				controllerFactory.CreateController(null, null);
			}


			[Test,
             ExpectedException(typeof(System.Web.HttpException), 
			 	ExpectedMessage = "Could not find a type for the controller name 'DoesNotExist'")]
			public void Should_throw_if_controller_type_cannot_be_resolved()
			{
				var requestContext = new RequestContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData());

                //HttpException(0x194,
				IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
				controllerFactory.InitializeWithControllerTypes(typeof(IocTestController));

				controllerFactory.CreateController(requestContext, "DoesNotExist");
			}

			protected override void AfterEachSpec()
			{
				_dependencyResolver = null;
				DependencyResolver.InitializeWith(null);
			}
		}

		[TestFixture]
		public class WhenDisposeControllerIsCalled : SpecBase
		{
			private IDependencyResolver _dependencyResolver;

			protected override void BeforeEachSpec()
			{
				_dependencyResolver = MockRepository.GenerateMock<IDependencyResolver>();
			}

			[Test]
			public void Then_ReleaseImplementation_should_be_called_on_the_specified_resolver()
			{
				var controller = new IocTestController();

				_dependencyResolver.Expect(r => r.DisposeImplementation(controller));

				IControllerFactory factory = new IoCControllerFactory(_dependencyResolver);
				factory.ReleaseController(controller);
			}

			[Test]
			public void Then_ReleaseImplementation_should_be_called_on_the_default_resolver()
			{
				DependencyResolver.InitializeWith(_dependencyResolver);
				var controller = new IocTestController();

				_dependencyResolver.Expect(r => r.DisposeImplementation(controller));

				IControllerFactory factory = new IoCControllerFactory();
				factory.ReleaseController(controller);
			}

            [Test]
            public void And_default_resolver_is_null_then_ReleaseImplementation_should_not_be_called_on_the_default_resolver()
            {
                DependencyResolver.InitializeWith(null);
                var controller = new IocTestController();

                IControllerFactory factory = new IoCControllerFactory();
                factory.ReleaseController(controller);
            }

            [Test]
			public void And_controller_implements_IDisposable_then_dispose_should_be_called()
			{
				var controller = new DisposableIocTestController();

				IControllerFactory factory = new IoCControllerFactory(_dependencyResolver);
				factory.ReleaseController(controller);

				Assert.IsTrue(controller.IsDisposed);
			}

			protected override void AfterEachSpec()
			{
				_dependencyResolver.VerifyAllExpectations();
			}
		}
	}

	public class IocTestController : IController
	{
		public void Execute(RequestContext controllerContext)
		{
			throw new NotImplementedException();
		}
	}

	public class DisposableIocTestController : IController, IDisposable
	{
		public bool IsDisposed;

		public void Execute(RequestContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			IsDisposed = true;
		}
	}
}