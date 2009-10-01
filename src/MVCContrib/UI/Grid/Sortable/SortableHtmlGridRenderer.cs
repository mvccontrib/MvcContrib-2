using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Adds the header row with column name links which carry data to enable sorting.
	/// </summary>
	/// <typeparam name="T">The type of the object in the data source list of the grid to be rendered</typeparam>
	public class SortableHtmlTableGridRenderer<T> : HtmlTableGridRenderer<T>, ISortableGridRenderer<T> where T : class
	{
		private string _orderQueryStringName = "SortOrder";

		/// <summary>
		/// The name of the query string parameter which carries order direction. SortOrder by default.
		/// </summary>
		public string OrderQueryStringName
		{
			get { return _orderQueryStringName; }
			set { _orderQueryStringName = value; }
		}

		private string _columnQueryStringName = "SortBy";

		/// <summary>
		/// The name of the query string parameter which carries the name of the column to sort by. SortBy by default.
		/// </summary>
		public string ColumnQueryStringName
		{
			get { return _columnQueryStringName; }
			set { _columnQueryStringName = value; }
		}

		private readonly ViewEngineCollection _engines;

		/// <summary>
		/// Creates an instance of a SortableHtmlTableGridRenderer to render a sortable grid.
		/// </summary>
		/// <param name="engines">The view engines used by the renderer</param>
		public SortableHtmlTableGridRenderer(ViewEngineCollection engines)
			: base(engines)
		{
			_engines = engines;
		}

		/// <summary>
		/// Creates an instance of a SortableHtmlTableGridRenderer to render a sortable grid.
		/// </summary>
		public SortableHtmlTableGridRenderer() {}

		/// <summary>
		/// Creates an instance of a SortableHtmlTableGridRenderer to render a sortable grid.
		/// </summary>
		///<param name="orderByQueryStringName">The name of the query string parameter which carries the name of the column to sort by.</param>
		///<param name="sortOrderQueryStringName">The name of the query string parameter which carries order direction.</param>
		public SortableHtmlTableGridRenderer(string sortOrderQueryStringName, string orderByQueryStringName)
			: this()
		{
			_orderQueryStringName = sortOrderQueryStringName;
			_columnQueryStringName = orderByQueryStringName;
		}

		protected override void RenderHeaderText(GridColumn<T> column)
		{
			EnsureSorting(column);
			if(column.IsSortable)
			{
				RenderText(new SortableLinkRenderer<T>(
				           	column
				           	, new RenderingContext(Writer, Context, _engines)).SortLink());
			}
			else
			{
				base.RenderHeaderText(column);
			}
		}

		private void EnsureSorting(GridColumn<T> column)
		{
			if(column.IsSortable && DataSource is ISortableDataSource<T>)
			{
				column.SortOptions.SortByQueryParameterName = _columnQueryStringName;
				column.SortOptions.SortOrderQueryParameterName = _orderQueryStringName;
				SetColumnSortOrder(column);
			}
		}

		private void SetColumnSortOrder(GridColumn<T> column)
		{
			ISortableDataSource<T> orderableSource = DataSource as ISortableDataSource<T>;
			if(column.SortOptions.IsDefault && String.IsNullOrEmpty(orderableSource.SortBy))
			{
				orderableSource.SortBy = column.Name;
			}

			if(String.IsNullOrEmpty(orderableSource.SortBy) == false &&
			   orderableSource.SortBy.ToLower().Equals(column.Name.ToLower())
			   && orderableSource.SortOrder == SortOrder.Ascending)
			{
				column.SortOptions.SortOrder = SortOrder.Descending;
			}
		}

		public void Render(IGridModel<T> gridModel, ISortableDataSource<T> dataSource, TextWriter output, ViewContext context)
		{
			SetSourceSortOptions(dataSource, context);
			base.Render(gridModel, dataSource as IEnumerable<T>, output, context);
		}

		public new void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output, ViewContext context)
		{
			Render(gridModel, dataSource as ISortableDataSource<T>, output, context);
		}

		private void SetSourceSortOptions(ISortableDataSource<T> dataSource, ViewContext context)
		{
			NameValueCollection qs = context.RequestContext.HttpContext.Request.QueryString;
			dataSource.SortBy = qs[_columnQueryStringName];
			if(String.IsNullOrEmpty(qs[_orderQueryStringName]) == false)
			{
				dataSource.SortOrder = (SortOrder)Enum.Parse(typeof(SortOrder), qs[_orderQueryStringName]);
			}
		}

		/// <summary>
		/// Fluent query string parameter name modifer for renderer instance.
		/// </summary>
		///<param name="sortByParameterName">The name of the query string parameter which carries the name of the column to sort by.</param>
		///<param name="sortOrderParameterName">The name of the query string parameter which carries order direction.</param>
		/// <returns></returns>
		public ISortableGridRenderer<T> WithQueryParameterNames(string sortOrderParameterName, string sortByParameterName)
		{
			_columnQueryStringName = sortByParameterName;
			_orderQueryStringName = sortOrderParameterName;
			return this;
		}
	}
}