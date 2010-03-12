using System;
using System.Linq.Expressions;

namespace MvcContrib.TestHelper.Ui
{
	public class RowFilter<T>
	{
		public readonly Expression<Func<T, object>> Expression;
		public readonly string Value;

		public RowFilter(Expression<Func<T, object>> expression, string value)
		{
			Expression = expression;
			Value = value;
		}
	}
}