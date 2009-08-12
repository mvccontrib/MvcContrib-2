using Microsoft.Practices.Unity;
using MvcContrib.Unity;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IoC
{
	public class UnityDependencyResolverTester
	{
		[TestFixture]
		public class WhenAValidTypeIsPassed : WhenAValidTypeIsPassedBase
		{
			public override void Setup()
			{
				IUnityContainer container = new UnityContainer()
					.RegisterType<SimpleDependency, SimpleDependency>()
					.RegisterType<IDependency, SimpleDependency>()
					.RegisterType<NestedDependency, NestedDependency>();

				_dependencyResolver = new UnityDependencyResolver(container);
			}

			[Test]
			public void ForCoverage()
			{
				IUnityContainer container = new UnityDependencyResolver().Container;
			}

			public override void TearDown()
			{
			}
		}
	}
}