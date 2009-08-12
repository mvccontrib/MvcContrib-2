namespace MvcContrib.UnitTests.IoC
{
    public interface INestedDependency
    {
        IDependency Dependency
        {
            get;
            set;
        }

    }
}