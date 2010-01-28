using System.Linq.Expressions;
using MvcContrib.UI.InputBuilder.Helpers;

namespace MvcContrib.TestHelper.Ui
{
    public class TextInputTester : InputTesterBase<string>
    {
        public TextInputTester(LambdaExpression property, string value)
            : base(value, ReflectionHelper.BuildNameFrom(property)) {}

        public override void AssertInputValueMatches(IBrowserDriver browserDriver)
        {
            string actualValue = browserDriver.GetValue(_inputName);

            _value.ShouldEqual(actualValue, "Asserting value for input '" + _inputName + "'.");
        }

        public override void SetInput(IBrowserDriver browserDriver)
        {
            browserDriver.SetValue(_inputName, _value);
        }
    }
}