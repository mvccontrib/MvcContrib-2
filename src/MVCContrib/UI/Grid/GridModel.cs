using System;
using System.Collections.Generic;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Default model for grid
	/// </summary>
	public class GridModel<T>  : IGridModel<T> where T : class
	{
		private readonly ColumnBuilder<T> _columnBuilder = new ColumnBuilder<T>();
		private readonly GridSections<T> _sections = new GridSections<T>();
		private IGridRenderer<T> _renderer = new HtmlTableGridRenderer<T>();
		private string _emptyText;
		private IDictionary<string, object> _attributes = new Dictionary<string, object>();

		ICollection<GridColumn<T>> IGridModel<T>.Columns
		{
			get { return _columnBuilder; }
		}

		IGridRenderer<T> IGridModel<T>.Renderer
		{
			get { return _renderer; }
			set { _renderer = value; }
		}

		string IGridModel<T>.EmptyText
		{
			get { return _emptyText; }
			set { _emptyText = value; }
		}

		IDictionary<string, object> IGridModel<T>.Attributes
		{
			get { return _attributes; }
			set { _attributes = value; }
		}

		/// <summary>
		/// Creates a new instance of the GridModel class
		/// </summary>
		public GridModel()
		{
			_emptyText = "There is no data available.";
		}

		/// <summary>
		/// Column builder for this grid model
		/// </summary>
		public ColumnBuilder<T> Column
		{
			get { return _columnBuilder; }
		}

		/// <summary>
		/// Section overrides for this grid model.
		/// </summary>
		IGridSections<T> IGridModel<T>.Sections
		{
			get { return _sections; }
		}

		/// <summary>
		/// Section overrides for this grid model.
		/// </summary>
		public GridSections<T> Sections
		{
			get { return _sections; }
		}

		/// <summary>
		/// Text that will be displayed when the grid has no data.
		/// </summary>
		/// <param name="emptyText">Text to display</param>
		public void Empty(string emptyText)
		{
			_emptyText = emptyText;
		}

		/// <summary>
		/// Defines additional attributes for the grid.
		/// </summary>
		/// <param name="hash"></param>
		public void Attributes(params Func<object, object>[] hash)
		{
			Attributes(new Hash(hash));
		}

		/// <summary>
		/// Defines additional attributes for the grid
		/// </summary>
		/// <param name="attributes"></param>
		public void Attributes(IDictionary<string, object> attributes)
		{
			_attributes = attributes;
		}

		/// <summary>
		/// Specifies the Renderer to use with this grid. If omitted, the HtmlTableGridRenderer will be used. 
		/// </summary>
		/// <param name="renderer">The Renderer to use</param>
		public void RenderUsing(IGridRenderer<T> renderer)
		{
			_renderer = renderer;
		}
	}
}
