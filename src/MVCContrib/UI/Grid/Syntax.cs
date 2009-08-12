using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MvcContrib.UI.Grid.Syntax
{
	public interface IGrid<T> : IGridWithOptions<T> where T: class 
	{
		/// <summary>
		/// Specifies a custom GridModel to use.
		/// </summary>
		/// <param name="model">The GridModel storing information about this grid</param>
		/// <returns></returns>
		IGridWithOptions<T> WithModel(IGridModel<T> model);
	}

	public interface IGridWithOptions<T> where T : class 
	{
		/// <summary>
		/// The GridModel that holds the internal representation of this grid.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)] //hide from fluent interface
		IGridModel<T> Model { get; }

		/// <summary>
		/// Specifies that the grid should be rendered using a specified renderer.
		/// </summary>
		/// <param name="renderer">Renderer to use</param>
		/// <returns></returns>
		IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer);

		/// <summary>
		/// Specifies the columns to use. 
		/// </summary>
		/// <param name="columnBuilder"></param>
		/// <returns></returns>
		IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder);

		/// <summary>
		/// Text to render when grid is empty.
		/// </summary>
		/// <param name="emptyText">Empty Text</param>
		/// <returns></returns>
		IGridWithOptions<T> Empty(string emptyText);

		/// <summary>
		/// Additional custom attributes
		/// </summary>
		/// <returns></returns>
		IGridWithOptions<T> Attributes(IDictionary<string, object> attributes);

		/// <summary>
		/// Additional custom attributes for each row
		/// </summary>
		/// <param name="attributes">Lambda expression that returns custom attributes for each row</param>
		/// <returns></returns>
		IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

		/// <summary>
		/// Additional custom attributes for the header row.
		/// </summary>
		/// <param name="attributes">Attributes for the header row</param>
		/// <returns></returns>
		IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes);

		/// <summary>
		/// Renders the grid to the TextWriter specified at creation
		/// </summary>
		/// <returns></returns>
		void Render();

	}
}