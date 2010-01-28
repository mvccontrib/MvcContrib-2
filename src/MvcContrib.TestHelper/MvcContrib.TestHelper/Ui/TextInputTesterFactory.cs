using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.TestHelper.Ui
{
    public class TextInputTesterFactory : IInputTesterFactory
    {
        public bool CanHandle(PropertyInfo info)
        {
            return true;
        }

        public IInputTester Create(LambdaExpression expression, string text)
        {
            return new TextInputTester(expression,text);
        }
    }
}