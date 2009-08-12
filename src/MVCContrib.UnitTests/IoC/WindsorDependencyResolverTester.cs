using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IoC
{
	[TestFixture]
	public class WindsorDependencyResolverTester
	{
		[TestFixture]
		public class WhenAValidTypeIsPassed : WhenAValidTypeIsPassedBase
		{
			public override void Setup()
			{
				IWindsorContainer container = new WindsorContainer();
				container.AddComponent("SimpleDependency", typeof(SimpleDependency));
				container.AddComponent("IDependency", typeof(IDependency), typeof(SimpleDependency));
				container.AddComponent("NestedDependency",typeof(NestedDependency));

				_dependencyResolver = new WindsorDependencyResolver(container);
			}

			[Test]
			public void ForCoverage()
			{
				IWindsorContainer container = new WindsorDependencyResolver().Container;
			}

			public override void TearDown()
			{
			}
		}

		[TestFixture]
		public class WhenDisposeImplementationIsCalled
		{
			private WindsorDependencyResolver _resolver;
			private IWindsorContainer _container;

			[SetUp]
			public void Setup()
			{
				_container = MockRepository.GenerateMock<IWindsorContainer>();
				_resolver = new WindsorDependencyResolver(_container);
			}

			[Test]
			public void ThenReleaseShouldBeCalled()
			{
				var obj = new object();

				_container.Expect(c => c.Release(obj));

				_resolver.DisposeImplementation(obj);

				_container.VerifyAllExpectations();
			}
		}
	}
}