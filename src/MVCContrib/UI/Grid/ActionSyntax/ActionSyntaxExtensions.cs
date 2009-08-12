using System;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid.ActionSyntax
{
	/// <summary>
	/// Extension methods for the Grid that enable the alternative 'Action' syntax for rendering custom sections.
	/// </summary>
	public static class ActionSyntaxExtensions
	{
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="block">Action that renders the HTML.</param>
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Action<T> block) where T : class
		{
			grid.Model.Sections.RowStart(block);
			return grid;
		}


		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="block">Action that renders the HTML.</param>
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Action<T, GridRowViewData<T>> block) where T : class
		{
			grid.Model.Sections.RowStart(block);
			return grid;
		}

		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the end of the row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="block">Action that renders the HTML.</param>
		public static IGridWithOptions<T> RowEnd<T>(this IGridWithOptions<T> grid, Action<T> block) where T : class
		{
			grid.Model.Sections.RowEnd(block);
			return grid;
		}

		public static void RowStart<T>(this IGridSections<T> sections, Action<T> block) where T : class
		{
			sections.Row.StartSectionRenderer = (rowData, context) =>
			{
				block(rowData.Item);
				return true;
			};
		}

		public static void RowStart<T>(this IGridSections<T> sections, Action<T, GridRowViewData<T>> block) where T : class
		{
			sections.Row.StartSectionRenderer = (rowData, context) => 
			{
				block(rowData.Item, rowData);
				return true;
			};
		}

		public static void RowEnd<T>(this IGridSections<T> sections, Action<T> block) where T : class 
		{
			sections.Row.EndSectionRenderer = (rowData, context) =>
			{
				block(rowData.Item);
				return true;
			};
		}

		/// <summary>
		/// Specifies that an action should be used to render the column header.
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="action">The action to render</param>
		/// <returns></returns>
		public static IGridColumn<T> HeaderAction<T>(this IGridColumn<T> column, Action action) {
			column.CustomHeaderRenderer = context => action();
			return column;
		}

		/// <summary>
		/// Specifies that an action should be used to render the contents of this column.
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="action">The action to render</param>
		/// <returns></returns>
		public static IGridColumn<T> Action<T>(this IGridColumn<T> column, Action<T> action) {
			column.CustomItemRenderer = (context, item) => action(item);
			return column;
		}

	}
}