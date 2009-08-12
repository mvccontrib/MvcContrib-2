using MvcContrib.StructureMap;
using NUnit.Framework;
using StructureMap;

namespace MvcContrib.UnitTests.IoC
{
    [TestFixture]
    public class StructureMapDependencyResolverTester
    {
        [TestFixture]
        public  class WhenAValidDependencyTypeIsPassed : WhenAValidTypeIsPassedBase
        {
            public override void TearDown()
            {                
                
            }

            public override void Setup()
            {
                ObjectFactory.Initialize(x =>
                {
                	x.UseDefaultStructureMapConfigFile = false;
					x.BuildInstancesOf<IDependency>().TheDefaultIsConcreteType<SimpleDependency>();
					x.BuildInstancesOf<SimpleDependency>().TheDefaultIsConcreteType<SimpleDependency>();
					x.BuildInstancesOf<NestedDependency>().TheDefaultIsConcreteType<NestedDependency>();
                });
                _dependencyResolver = new StructureMapDependencyResolver();
            }
        }
    }
}
