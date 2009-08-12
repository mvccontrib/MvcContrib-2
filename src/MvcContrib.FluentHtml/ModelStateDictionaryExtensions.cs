using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Expressions;

namespace MvcContrib.FluentHtml
{
	public static class ModelStateDictionaryExtensions
	{
		/// <summary>
		/// Adds an error to the current model state.
		/// </summary>
		/// <typeparam name="T">The view model your view is using.</typeparam>
		/// <param name="modelState">The current model state.</param>
		/// <param name="keyExpression">An expression specifying the property on the view model to add an error for.</param>
		/// <param name="errorMessage">The error message you're adding for the specified property.</param>
		public static void AddModelError<T>(this ModelStateDictionary modelState, Expression<Func<T, object>> keyExpression, string errorMessage) where T : class
		{
			modelState.AddModelError(keyExpression.GetNameFor().FormatAsHtmlId(), errorMessage);
		}

		/// <summary>
		/// Adds an error to the current model state.
		/// </summary>
		/// <typeparam name="T">The view model your view is using.</typeparam>
		/// <param name="modelState">The current model state.</param>
		/// <param name="keyExpression">An expression specifying the property on the view model to add an error for.</param>
		/// <param name="exception">The exception you're adding for the specified property.</param>
		public static void AddModelError<T>(this ModelStateDictionary modelState, Expression<Func<T, object>> keyExpression, Exception exception) where T : class
		{
			modelState.AddModelError(keyExpression.GetNameFor().FormatAsHtmlId(), exception);
		}
	}
}
