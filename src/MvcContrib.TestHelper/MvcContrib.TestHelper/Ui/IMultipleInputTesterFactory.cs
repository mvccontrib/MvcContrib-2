using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.TestHelper.Ui
{
	public interface IMultipleInputTesterFactory
	{
		bool CanHandle(PropertyInfo info);
		IInputTester Create(LambdaExpression expression, string[] text);
	}
}