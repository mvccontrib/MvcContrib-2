using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcContrib.UI.LegacyGrid
{

	/// <summary>
	/// Extension methods on the HtmlHelper for creating Grid instances.
	/// </summary>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
	public static class GridExtensions
	{
		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid(helper, viewDataKey, null, columns, null);
		}
		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			Grid(helper, viewDataKey, null, columns, sections);
		}

		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns) where T : class 
		{
			Grid(helper, viewDataKey, htmlAttributes, columns, null);
		}

		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			var grid = new Grid<T>(
				viewDataKey,
				helper.ViewContext,
				CreateColumnBuilder(columns, sections),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output
			);

			grid.Render();
		}

		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid(helper, dataSource, null, columns, null);
		}

		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class 
		{
			Grid(helper, dataSource, null, columns, sections);
		}

		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns) where T : class 
		{
			Grid(helper, dataSource, htmlAttributes, columns, null);
		}

		[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]
		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			var grid = new Grid<T>(
				dataSource,
				CreateColumnBuilder(columns, sections),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output,
				helper.ViewContext.HttpContext
			);

			grid.Render();
		}

		private static GridColumnBuilder<T> CreateColumnBuilder<T>(Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			var builder = new GridColumnBuilder<T>();

			if (columns != null)
			{
				columns(builder);
			}

			if(sections != null)
			{
				sections(builder);	
			}

			return builder;
		}
	}
}
