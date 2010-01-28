namespace MvcContrib.TestHelper.Ui
{
    public abstract class InputTesterBase<TInputValue> : IInputTester
    {
        protected string _inputName;
        protected TInputValue _value;

        protected InputTesterBase(TInputValue value, string name)
        {
            _value = value;
            _inputName = name;
        }

        #region IInputTester Members

        public abstract void AssertInputValueMatches(IBrowserDriver browserDriver);

        public abstract void SetInput(IBrowserDriver browserDriver);

        #endregion
    }
}