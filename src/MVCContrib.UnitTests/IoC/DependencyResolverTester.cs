using System;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.IoC
{
	public abstract class WhenAValidTypeIsPassedBase
	{
		protected IDependencyResolver _dependencyResolver;

		[SetUp]
		public abstract void Setup();

		[TearDown]
		public abstract void TearDown();

		[Test]
		public void Static_Dependency_Resolver_Wraps_Specific_Resolver()
		{
			DependencyResolver.InitializeWith(_dependencyResolver);
			Assert.AreEqual(_dependencyResolver, DependencyResolver.Resolver);

			IDependency depedency = DependencyResolver.GetImplementationOf<SimpleDependency>();

			Assert.That(depedency, Is.Not.Null);
			Assert.That(depedency, Is.AssignableFrom(typeof(SimpleDependency)));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Static_Dependency_Resolver_Throws_When_Not_Found_In_Resolver_And_Cannot_CreateInstance()
		{
			DependencyResolver.GetImplementationOf<Array>();
		}

		[Test]
		public void ShouldReturnTheSimpleDependencyAndCastToAnInterface()
		{
			var depedency = _dependencyResolver.GetImplementationOf<IDependency>(typeof(SimpleDependency));

			Assert.That(depedency, Is.Not.Null);
			Assert.That(depedency, Is.AssignableFrom(typeof(SimpleDependency)));
		}

		[Test]
		public void ShouldReturnNestedDependencyAndCastToAnInterface()
		{
			var dependency = _dependencyResolver.GetImplementationOf<IDependency>(typeof(NestedDependency));

			Assert.That(dependency, Is.Not.Null);
			Assert.That(dependency, Is.AssignableFrom(typeof(NestedDependency)));

			var nestedDependency = (INestedDependency)dependency;
			Assert.That(nestedDependency.Dependency, Is.Not.Null);
			Assert.That(nestedDependency.Dependency, Is.AssignableFrom(typeof(SimpleDependency)));
		}

		[Test]
		public void ShouldReturnTheSimpleDependency()
		{
			IDependency depedency = _dependencyResolver.GetImplementationOf<SimpleDependency>();

			Assert.That(depedency, Is.Not.Null);
			Assert.That(depedency, Is.AssignableFrom(typeof(SimpleDependency)));
		}

		[Test]
		public void ShouldReturnNestedDependency()
		{
			IDependency dependency = _dependencyResolver.GetImplementationOf<NestedDependency>();

			Assert.That(dependency, Is.Not.Null);
			Assert.That(dependency, Is.AssignableFrom(typeof(NestedDependency)));

			var nestedDependency = (INestedDependency)dependency;
			Assert.That(nestedDependency.Dependency, Is.Not.Null);
			Assert.That(nestedDependency.Dependency, Is.AssignableFrom(typeof(SimpleDependency)));
		}

		[Test]
		public void Should_Return_Null_If_Not_Found_In_Container()
		{
			var dependency = _dependencyResolver.GetImplementationOf<IAppDomainSetup>();
			Assert.That(dependency, Is.Null);
		}

		[Test]
		public void For_Coverage()
		{
			//TODO: Check whether the other IoC containers need to release instances like Windsor's container.Release().
			_dependencyResolver.DisposeImplementation(new object());
		}
	}
}
