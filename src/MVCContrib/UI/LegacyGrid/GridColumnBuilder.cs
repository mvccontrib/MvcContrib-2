using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcContrib.UI.LegacyGrid
{
	/// <summary>
	/// Constructs GridColumn objects representing the columns to be rendered in a grid.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]	
	public class GridColumnBuilder<T> : IExpressionColumnBuilder<T>, ISimpleColumnBuilder<T>, IRootGridColumnBuilder<T>, IGridSections<T>, IEnumerable<GridColumn<T>> where T : class
	{
		
		private readonly List<GridColumn<T>> columns = new List<GridColumn<T>>(); //Final collection of columns to render
		private GridColumn<T> currentColumn; //The column currently being parsed
		public Action<T> RowStartBlock { get; set; }
		public Action<T, bool> RowStartWithAlternateBlock { get; set; }
		public Action<T> RowEndBlock { get; set; }

		public INestedGridColumnBuilder<T> Formatted(string format)
		{
			currentColumn.Format = format;
			return this;
		}

		public INestedGridColumnBuilder<T> DoNotEncode()
		{
			currentColumn.Encode = false;
			return this;
		}

		public INestedGridColumnBuilder<T> DoNotSplit()
		{
			currentColumn.DoNotSplit = true;
			return this;
		}

		public INestedGridColumnBuilder<T> CellCondition(Func<T, bool> condition)
		{
			currentColumn.CellCondition = condition;
			return this;
		}

		public INestedGridColumnBuilder<T> ColumnCondition(Func<bool> condition)
		{
			currentColumn.ColumnCondition = condition;
			return this;
		}

		public INestedGridColumnBuilder<T> Do(Action<T> block)
		{
			currentColumn.CustomRenderer = block;
			return this;
		}

		public INestedGridColumnBuilder<T> Header(Action block)
		{
			currentColumn.CustomHeader = block;
			return this;
		}

		/// <summary>
		/// Applies the specified attributes to the header of the current column.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public INestedGridColumnBuilder<T> HeaderAttributes(IDictionary attributes)
		{
			currentColumn.HeaderAttributes = attributes;
			return this;
		}

		public IExpressionColumnBuilder<T> For(Expression<Func<T, object>> expression)
		{
			currentColumn = new GridColumn<T>
			                	{
			                		Name = ExpressionToName(expression),
			                		ColumnDelegate = expression.Compile(),
			                	};

			columns.Add(currentColumn);
			return this;
		}

		public INestedGridColumnBuilder<T> For(Func<T, object> func, string name)
		{
			currentColumn = new GridColumn<T>
			                	{
			                		Name = name,
			                		ColumnDelegate = func,
			                		DoNotSplit = true
			                	};
			columns.Add(currentColumn);
			return this;
		}

		public ISimpleColumnBuilder<T> For(string name)
		{
			currentColumn = new GridColumn<T> { Name = name, DoNotSplit = true };
			columns.Add(currentColumn);
			return this;
		}

		public void RowStart(Action<T> block)
		{
			RowStartBlock = block;
		}

		public void RowStart(Action<T, bool> block)
		{
			RowStartWithAlternateBlock = block;
		}

		public void RowEnd(Action<T> block)
		{
			RowEndBlock = block;
		}

		/// <summary>
		/// Grabs the property name from a member expression.
		/// </summary>
		/// <param name="expression">The expression</param>
		/// <returns>The name of the property</returns>
		public static string ExpressionToName(Expression<Func<T, object>> expression)
		{
			var memberExpression = RemoveUnary(expression.Body) as MemberExpression;

			return memberExpression == null ? string.Empty : memberExpression.Member.Name;
		}


		private static Expression RemoveUnary(Expression body)
		{
			var unary = body as UnaryExpression;
			if (unary != null)
			{
				return unary.Operand;
			}
			return body;
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return columns.GetEnumerator();
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return columns.GetEnumerator();
		}

		public GridColumn<T> this[int index]
		{
			get
			{
				return columns[index];
			}
		}
	}
}
