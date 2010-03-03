using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;


namespace MvcContrib.ActionResults
{
	public delegate RouteValueDictionary ExpressionToRouteValueConverter<TController>(
		Expression<Action<TController>> expression) where TController : Controller;

	/// <summary>
	/// Represents a result that performs a redirect using a lambda expression to generate the route values.
	/// </summary>
	/// <typeparam name="T">Type of controller</typeparam>
	public class RedirectToRouteResult<T> : RedirectToRouteResult, IControllerExpressionContainer where T : Controller
	{
		MethodCallExpression IControllerExpressionContainer.Expression
		{
			get { return Expression.Body as MethodCallExpression; }
		}

		/// <summary>
		/// The route values.
		/// </summary>
		public Expression<Action<T>> Expression { get; private set; }

		/// <summary>
		/// Creates a new instance of the RedirectToRouteResult class.
		/// </summary>
		/// <param name="expression"></param>
		public RedirectToRouteResult(Expression<Action<T>> expression)
			: this(expression, expr => Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(expr)) {}

		/// <summary>
		/// Creates a new instance of the RedirectToRouteResult class using the specified ExpressionParser
		/// </summary>
		public RedirectToRouteResult(Expression<Action<T>> expression, ExpressionToRouteValueConverter<T> expressionParser)
			: base(expressionParser(expression))
		{
			Expression = expression;
		}
	}

	public interface IControllerExpressionContainer
	{
		MethodCallExpression Expression { get; }
	}
}