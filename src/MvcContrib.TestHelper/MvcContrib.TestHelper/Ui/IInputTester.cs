namespace MvcContrib.TestHelper.Ui
{
    public interface IInputTester
    {
        void SetInput(IBrowserDriver browserDriver);
        void AssertInputValueMatches(IBrowserDriver browserDriver);
    }
}