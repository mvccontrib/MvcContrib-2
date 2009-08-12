using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace MvcContrib
{
	/// <summary>
	/// Extension methods for UrlHelper
	/// </summary>
	public static class UrlHelperExtensions
	{
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expression) where TController : Controller
		{
			return LinkBuilder.BuildUrlFromExpression(urlHelper.RequestContext, urlHelper.RouteCollection, expression);	
		}
	}
}