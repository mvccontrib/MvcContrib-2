using Microsoft.Practices.Unity;
namespace MvcContrib.Unity
{
    public interface IUnityContainerAccessor
    {
        IUnityContainer Container { get; }
    }
}
