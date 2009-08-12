using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Defines a grid to be rendered.
	/// </summary>
	/// <typeparam name="T">Type of datasource for the grid</typeparam>
	public class Grid<T> : IGrid<T> where T : class
	{
		private readonly TextWriter _writer;
		private readonly ViewContext context;
		private IGridModel<T> _gridModel = new GridModel<T>();

		/// <summary>
		/// The GridModel that holds the internal representation of this grid.
		/// </summary>
		public IGridModel<T> Model
		{
			get { return _gridModel; }
		}

		/// <summary>
		/// Creates a new instance of the Grid class.
		/// </summary>
		/// <param name="dataSource">The datasource for the grid</param>
		/// <param name="writer">The TextWriter where the grid should be rendered</param>
		/// <param name="context"></param>
		public Grid(IEnumerable<T> dataSource, TextWriter writer, ViewContext context)
		{
			_writer = writer;
			this.context = context;
			DataSource = dataSource;
		}

		/// <summary>
		/// The datasource for the grid.
		/// </summary>
		public IEnumerable<T> DataSource { get; private set; }

		public IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer)
		{
			_gridModel.Renderer = renderer;
			return this;
		}

		public IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
		{
			var builder = new ColumnBuilder<T>();
			columnBuilder(builder);

			foreach (var column in builder)
			{
				_gridModel.Columns.Add(column);
			}

			return this;
		}

		public IGridWithOptions<T> Empty(string emptyText)
		{
			_gridModel.EmptyText = emptyText;
			return this;
		}

		public IGridWithOptions<T> Attributes(IDictionary<string, object> attributes)
		{
			_gridModel.Attributes = attributes;
			return this;
		}

		public IGridWithOptions<T> WithModel(IGridModel<T> model)
		{
			_gridModel = model;
			return this;
		}

		/// <summary>
		/// Renders to the TextWriter, and returns null. 
		/// This is by design so that it can be used with inline syntax in views.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			Render();
			return null;
		}

		public IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes)
		{
			_gridModel.Sections.HeaderRowAttributes(attributes);
			return this;
		}

		public void Render()
		{
			_gridModel.Renderer.Render(_gridModel, DataSource, _writer, context);
		}

		public IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
		{
			_gridModel.Sections.RowAttributes(attributes);
			return this;
		}
	}
}