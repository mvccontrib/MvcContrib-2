using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.UI.DataList;

namespace MvcContrib.UI.DataList
{
	public static class DataListExtensions
	{
		/// <summary>
		/// Create a DataList.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="helper">The helper.</param>
		/// <param name="dataSource">The data source.</param>
		/// <returns></returns>
		public static DataList<T> DataList<T>(this HtmlHelper helper, IEnumerable<T> dataSource)
		{
			var list = new DataList<T>(dataSource, helper.ViewContext.HttpContext.Response.Output);
			return list;
		}

		/// <summary>
		/// Create a DataList.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="helper">The helper.</param>
		/// <param name="dataSource">The data source.</param>
		/// <param name="tableAttributes">The table attributes.</param>
		/// <returns></returns>
		public static DataList<T> DataList<T>(this HtmlHelper helper, IEnumerable<T> dataSource,
		                                      params Func<object, object>[] tableAttributes)
		{
			var list = new DataList<T>(dataSource, helper.ViewContext.HttpContext.Response.Output, new Hash(tableAttributes));
			return list;
		}
	}
}