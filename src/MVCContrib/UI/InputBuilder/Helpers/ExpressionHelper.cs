using System;
using System.Linq.Expressions;

namespace MvcContrib.UI.InputBuilder.Helpers
{
	public static class ExpressionHelper
	{
		public static object Evaluate(LambdaExpression expression, object target)
		{
			if(target == null)
			{
				return null;
			}

			Delegate func = expression.Compile();

			object result = null;
			try
			{
				result = func.DynamicInvoke(target);
			}
			catch(Exception)
			{
				;
			}

			return result;
		}
	}
}