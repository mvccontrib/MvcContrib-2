using System;
using System.Web.Mvc;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ViewRender
	{
		/// <summary>
		/// Asserts that a RenderViewResult is rendering the specified view.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="viewName">The name of the view.</param>
		/// <returns></returns>
		public static T ForViewOrItself<T>(this T result, string viewName) where T : ViewResultBase
		{
			if(!String.IsNullOrEmpty(result.ViewName) && result.ViewName != viewName)
			{
				throw new ActionResultAssertionException(String.Format("Expected view name '{0}', actual was '{1}'", viewName,
				                                                       result.ViewName));
			}
			return result;
		}

		/// <summary>
		/// Asserts that a RenderViewResult is rendering the specified view.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="viewName">The name of the view.</param>
		/// <returns></returns>
		public static T ForView<T>(this T result, string viewName) where T : ViewResultBase
		{
			if(result.ViewName != viewName)
			{
				throw new ActionResultAssertionException(String.Format("Expected view name '{0}', actual was '{1}'", viewName,
				                                                       result.ViewName));
			}
			return result;
		}
	}
}