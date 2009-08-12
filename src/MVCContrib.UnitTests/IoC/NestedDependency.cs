namespace MvcContrib.UnitTests.IoC
{
    public class NestedDependency : INestedDependency, IDependency
    {
    	public NestedDependency(IDependency dependency)
        {
            Dependency = dependency;
        }

    	public IDependency Dependency { get; set; }
    }
}
