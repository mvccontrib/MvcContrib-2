using System.Linq.Expressions;

namespace MvcContrib.FluentHtml.Expressions
{
	public class ExpressionNameVisitorResult
	{
		public Expression NextExpression { get; set; }
		public string Name { get; set; }
	}
}
