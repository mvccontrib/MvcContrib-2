using System;
using System.Linq.Expressions;

namespace MvcContrib.FluentHtml.Expressions
{
	public static class ExpressionExtensions
	{
		public static string GetNameFor<T, TValue>(this Expression<Func<T, TValue>> expression) where T : class
		{
			return new ExpressionNameVisitor().Visit(expression.Body);
		}

		public static string GetNameFor<T, TValue>(this Expression<Func<T, TValue>> expression, IViewModelContainer<T> view) where T : class
		{
			var name = expression.GetNameFor();
			return string.Format("{0}{1}{2}",
				view.HtmlNamePrefix, 
				(string.IsNullOrEmpty(view.HtmlNamePrefix) || string.IsNullOrEmpty(name) || name.StartsWith("[") 
					? null 
					: "."),
				name);
		}

		public static TValue GetValueFrom<T, TValue>(this Expression<Func<T, TValue>> expression, T viewModel) where T : class
		{
			try
			{
				return viewModel == null
					? default(TValue)
					: expression.Compile().Invoke(viewModel);
			}
			catch (Exception)
			{
				return default(TValue);
			}
		}

		public static MemberExpression GetMemberExpression<T, TValue>(this Expression<Func<T, TValue>> expression) where T : class
		{
			if (expression == null)
			{
				return null;
			}
			if (expression.Body is MemberExpression)
			{
				return (MemberExpression)expression.Body;
			}
			if (expression.Body is UnaryExpression)
			{
				var operand = ((UnaryExpression)expression.Body).Operand;
				if (operand is MemberExpression)
				{
					return (MemberExpression)operand;
				}
				if (operand is MethodCallExpression)
				{
					return ((MethodCallExpression)operand).Object as MemberExpression;
				}
			}
			return null;
		}
	}
}