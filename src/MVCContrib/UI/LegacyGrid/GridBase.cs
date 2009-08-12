using System.Web;
using System.Collections.Generic;
using System.IO;

namespace MvcContrib.UI.LegacyGrid
{

	/// <summary>
	/// Base class for SmartGrid functionality.
	/// </summary>
	/// <typeparam name="T">The type of object for each row in the grid.</typeparam>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]	
	public abstract class GridBase<T> where T : class
	{
		/// <summary>
		/// The items to be displayed in the grid.
		/// </summary>
		protected IEnumerable<T> Items { get; set; }

		/// <summary>
		/// The columns to generate.
		/// </summary>
		protected GridColumnBuilder<T> Columns { get; set; }

		/// <summary>
		/// The writer to output the results to.
		/// </summary>
		protected TextWriter Writer { get; private set; }

		/// <summary>
		/// Message to be displayed if the Items property is empty.
		/// </summary>
		public string EmptyMessageText { get; set; }

		protected GridBase(IEnumerable<T> items, GridColumnBuilder<T> columns, TextWriter writer)
		{
			Items = items;
			Columns = columns;
			Writer = writer;
			EmptyMessageText = "There is no data available.";
		}

		/// <summary>
		/// Renders text to the output stream
		/// </summary>
		/// <param name="text">The text to render</param>
		protected void RenderText(string text)
		{
			Writer.Write(text);
		}

		/// <summary>
		/// Performs the rendering of the grid.
		/// </summary>
		public virtual void Render()
		{
			RenderGridStart();
			bool headerRendered = RenderHeader();

			if(headerRendered)
			{
				RenderItems();
			}
			else
			{
				RenderEmpty();
			}

			RenderGridEnd(! headerRendered);
		}

		/// <summary>
		/// Iterates over the items collection, builds up the markup for each row and outputs the results.
		/// </summary>
		protected virtual void RenderItems()
		{
			bool isAlternate = false;
			foreach(var item in Items)
			{
				RenderRowStart(item, isAlternate);

				foreach(var column in Columns)
				{
					//Column condition has been specified. Continue to the next column if the condition fails.
					if (column.ColumnCondition != null && !column.ColumnCondition()) 
					{
						continue;
					}

					//A custom item section has been specified - render it and continue to the next iteration.
					if(column.Name != null && column.CustomRenderer != null)
					{
						column.CustomRenderer(item);
						continue;
					}

					RenderStartCell(column);
					object value = null;

					bool failedCellCondition = false;

					//Cell condition has been specified. Skip rendering of this cell if the cell condition fails.
					if(column.CellCondition != null && !column.CellCondition(item))
					{
						failedCellCondition = true;
					}

					if(!failedCellCondition)
					{
						//Invoke the delegate to retrieve the value to be displayed in the cell.
						if(column.ColumnDelegate != null)
						{
							value = column.ColumnDelegate(item);
						}
						else //If there isn't a column delegate, attempt to use reflection instead (for anonymous types)
						{
							var property = item.GetType().GetProperty(column.Name);
							if(property != null)
							{
								value = property.GetValue(item, null);
							}
						}


						if(value != null)
						{
							if(column.Format != null) //Use custom output format if specified.
							{
								RenderText(string.Format(column.Format, value));
							}
							else if(column.Encode) //HTML-Encode unless encoding has been explicitly disabled for this cell.
							{
								RenderText(HttpUtility.HtmlEncode(value.ToString()));
							}
							else
							{
								RenderText(value.ToString());
							}
						}
					}


					RenderEndCell();
				}

				RenderRowEnd(item);

				isAlternate = !isAlternate;
			}
		}

		/// <summary>
		/// If there are items to display, iterate over each column and output the results. 
		/// If there are no items to display, return false to indicate execution of RenderItems should not take place.
		/// </summary>
		/// <returns>boolean indication whether or not items should be rendered.</returns>
		protected virtual bool RenderHeader()
		{
			//No items - do not render a header.
			if(Items == null)
			{
				return false;
			}

			IEnumerator<T> enumerator = Items.GetEnumerator();

			//No items - do not render header.
			if(!enumerator.MoveNext())
			{
				return false;
			}

			RenderHeadStart();

			foreach(var column in Columns)
			{
				//Allow for custom header overrides.
				if(column.CustomHeader != null)
				{
					column.CustomHeader();
				}
				else
				{
					//Skip if the custom Column Condition fails.
					if(column.ColumnCondition != null && !column.ColumnCondition())
					{
						continue;
					}

					RenderHeaderCellStart(column);				

					RenderText(column.Name);

					RenderHeaderCellEnd();
				}
			}

			RenderHeadEnd();

			return true;
		}


		/// <summary>
		/// Renders the start of a row to the output stream.
		/// </summary>
		/// <param name="item">The item to be rendered into this row.</param>
		/// <param name="isAlternate">Whether the row is an alternate row</param>
		protected virtual void RenderRowStart(T item, bool isAlternate)
		{
			//If there's a custom delegate for rendering the start of the row, invoke it.
			//Otherwise fall back to the default rendering.
			if(Columns.RowStartBlock != null)
			{
				Columns.RowStartBlock(item);
			}
			else if(Columns.RowStartWithAlternateBlock != null)
			{
				Columns.RowStartWithAlternateBlock(item, isAlternate);
			}
			else
			{
				RenderRowStart(isAlternate);
			}
		}

		/// <summary>
		/// Renders the end of a row to the output stream.
		/// </summary>
		/// <param name="item">The item being rendered in to this row.</param>
		protected virtual void RenderRowEnd(T item)
		{
			//If there's a custom delegate for rendering the end of the row, invoke it.
			//Otherwise fall back to the default rendering.
			if(Columns.RowEndBlock != null)
			{
				Columns.RowEndBlock(item);
			}
			else
			{
				RenderRowEnd();
			}
		}

		protected abstract void RenderHeaderCellEnd();
		protected abstract void RenderHeaderCellStart(GridColumn<T> column);
		protected abstract void RenderRowStart(bool isAlternate);
		protected abstract void RenderRowEnd();
		protected abstract void RenderEndCell();
		protected abstract void RenderStartCell(GridColumn<T> column);
		protected abstract void RenderHeadStart();
		protected abstract void RenderHeadEnd();
		protected abstract void RenderGridStart();
		protected abstract void RenderGridEnd(bool isEmpty);
		protected abstract void RenderEmpty();
	}
}
