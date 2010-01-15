using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Extension methods related to the Grid
	/// </summary>
	public static class GridExtensions
	{
		private const string CouldNotFindView =
			"The view '{0}' or its master could not be found. The following locations were searched:{1}";

		/// <summary>
		/// Creates a grid using the specified datasource.
		/// </summary>
		/// <typeparam name="T">Type of datasouce element</typeparam>
		/// <returns></returns>
		public static IGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource) where T : class
		{
			return new Grid<T>(dataSource, helper.ViewContext.Writer, helper.ViewContext);
		}

		/// <summary>
		/// Creates a grid from an entry in the viewdata.
		/// </summary>
		/// <typeparam name="T">Type of element in the grid datasource.</typeparam>
		/// <returns></returns>
		public static IGrid<T> Grid<T>(this HtmlHelper helper, string viewDataKey) where T : class
		{
			var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IEnumerable<T>;

			if (dataSource == null)
			{
				throw new InvalidOperationException(string.Format(
														"Item in ViewData with key '{0}' is not an IEnumerable of '{1}'.", viewDataKey,
														typeof(T).Name));
			}

			return helper.Grid(dataSource);
		}

		/// <summary>
		/// Defines additional attributes for the column heading.
		/// </summary>
		/// <returns></returns>
		public static IGridColumn<T> HeaderAttributes<T>(this IGridColumn<T> column, params Func<object, object>[] hash)
		{
			return column.HeaderAttributes(new Hash(hash));
		}

		/// <summary>
		/// Defines additional attributes for a grid.
		/// </summary>
		/// <returns></returns>
		public static IGridWithOptions<T> Attributes<T>(this IGridWithOptions<T> grid, params Func<object, object>[] hash)
			where T : class
		{
			return grid.Attributes(new Hash(hash));
		}

		/// <summary>
		/// Defines additional attributes for the cell. 
		/// </summary>
		public static IGridColumn<T> Attributes<T>(this IGridColumn<T> column, params Func<object, object>[] hash)
		{
			return column.Attributes(x => new Hash(hash));
		}

		/// <summary>
		/// Associates custom attributes with every grid row.
		/// </summary>
		public static void RowAttributes<T>(this IGridSections<T> sections,  Func<GridRowViewData<T>, IDictionary<string, object>> attributes) where T : class 
		{
			sections.Row.Attributes = attributes;
		}

		public static IView TryLocatePartial(this ViewEngineCollection engines, ViewContext context, string viewName)
		{
			var viewResult = engines.FindPartialView(context, viewName);

			if (viewResult.View == null)
			{
				var locationsText = new StringBuilder();
				foreach (var location in viewResult.SearchedLocations)
				{
					locationsText.AppendLine();
					locationsText.Append(location);
				}

				throw new InvalidOperationException(string.Format(CouldNotFindView, viewName, locationsText));
			}

			return viewResult.View;
		}

		/// <summary>
		/// Renders the specified text at the start of every row instead of the default output.
		/// </summary>
		public static void RowStart<T>(this IGridSections<T> sections, Func<GridRowViewData<T>, string> rowStart) where T : class
		{
			sections.Row.StartSectionRenderer = (rowData, context) =>
			{
				context.Writer.Write(rowStart(rowData));
				return true;
			};
		}

		/// <summary>
		/// Renders the specified text at the end of every row instead of the default output.
		/// </summary>
		public static void RowEnd<T>(this IGridSections<T> sections, Func<GridRowViewData<T>, string> rowEnd) where T : class 
		{
			sections.Row.EndSectionRenderer = (rowData, context) => 
			{
				context.Writer.Write(rowEnd(rowData));
				return true;
			};
		}

		/// <summary>
		/// Renders the specified text at the start of every row instead of the default output.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="rowStart">Lambda takes an instance of GridRowViewData and returns the string to render</param>
		/// <returns></returns>
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Func<GridRowViewData<T>, string> rowStart) where T : class 
		{
			grid.Model.Sections.RowStart(rowStart);
			return grid;
		}

		/// <summary>
		/// Renders the specified text at the start of every row instead of the default output.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="rowEnd">Lambda takes an instance of GridRowViewData and returns the string to render</param>
		/// <returns></returns>
		public static IGridWithOptions<T> RowEnd<T>(this IGridWithOptions<T> grid, Func<GridRowViewData<T>, string> rowEnd) where T : class 
		{
			grid.Model.Sections.RowEnd(rowEnd);
			return grid;
		}

		/// <summary>
		/// The HTML that should be used to render the header for the column. This should include TD tags. 
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="header">The format to use.</param>
		/// <returns></returns>
		public static IGridColumn<T> Header<T>(this IGridColumn<T> column, string header) where T : class 
		{
			column.CustomHeaderRenderer = c => c.Writer.Write(header);
			return column;
		}

		/// <summary>
		/// Specifies that a partial view should be used to render the contents of this column.
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="partialName">The name of the partial view</param>
		/// <returns></returns>
		public static IGridColumn<T> Partial<T>(this IGridColumn<T> column, string partialName) where T : class 
		{
			column.CustomItemRenderer = (context, item) => {
				var view = context.ViewEngines.TryLocatePartial(context.ViewContext, partialName);
				var newViewData = new ViewDataDictionary<T>(item);
				var newContext = new ViewContext(context.ViewContext, context.ViewContext.View, newViewData, context.ViewContext.TempData, context.ViewContext.Writer);
				view.Render(newContext, context.Writer);
			};
			return column;
		}

		/// <summary>
		/// Specifies custom attributes for the header row.
		/// </summary>
		public static void HeaderRowAttributes<T>(this IGridSections<T> sections, IDictionary<string, object> attributes) where T : class
		{
			sections.HeaderRow.Attributes = x => attributes;
		}

		/// <summary>
		/// Specifies that the grid's columns should be automatically generated from the public properties on the model object.
		/// </summary>
		public static IGridWithOptions<T> AutoGenerateColumns<T>(this IGrid<T> grid) where T : class
		{
			return grid.WithModel(new AutoColumnGridModel<T>(ModelMetadataProviders.Current));
		}
	}
}