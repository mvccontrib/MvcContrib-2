using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MvcContrib.UI.LegacyGrid
{
	/// <summary>
	/// A column to be rendered as part of a grid.
	/// </summary>
	/// <typeparam name="T">Type of object to be rendered in the grid.</typeparam>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]	
	public class GridColumn<T>
	{
		/// <summary>
		/// Creates a new instance of the <see cref="GridColumn{T}"/>
		/// </summary>
		public GridColumn()
		{
			Name = string.Empty;
			Encode = true;
		}

		/// <summary>
		/// Delegate that will be invoked on each item in the in the datasource in order to obtain the current item's value.
		/// </summary>
		public Func<T, object> ColumnDelegate {get; set; }

		private string _name;

		/// <summary>
		/// Name of the column
		/// </summary>
		public string Name
		{
			get
			{
				//By default, PascalCased property names should be split and separated by a space (eg "Pascal Cased")
				if (!DoNotSplit)
				{
					return SplitPascalCase(_name);
				}
				return _name;
			}
			set { _name = value; }
		}

		/// <summary>
		/// Custom format for the cell output.
		/// </summary>
		public string Format { get; set; }
		/// <summary>
		/// Whether or not PascalCased names should be split.
		/// </summary>
		public bool DoNotSplit { get; set; }
		/// <summary>
		/// Delegate used to hide the contents of the cells in a column.
		/// </summary>
		public Func<T, bool> CellCondition { get; set; }
		/// <summary>
		/// Delegate used to hide the entire column
		/// </summary>
		public Func<bool> ColumnCondition { get; set; }
		/// <summary>
		/// Delegate that can be used to perform custom rendering actions.
		/// </summary>
		public Action<T> CustomRenderer { get; set; }
		/// <summary>
		/// Delegate used to specify a custom heading.
		/// </summary>
		public Action CustomHeader { get; set; }
		/// <summary>
		/// Whether to HTML-Encode the output (default is true).
		/// </summary>
		public bool Encode { get; set; }

		/// <summary>
		/// The attributs to apply to the header of the column.
		/// </summary>
		public IDictionary HeaderAttributes { get; set; }

		/// <summary>
		/// Replaces pascal casing with spaces. For example "CustomerId" would become "Customer Id".
		/// Strings that already contain spaces are ignored.
		/// </summary>
		/// <param name="input">String to split</param>
		/// <returns>The string after being split</returns>
		protected virtual string SplitPascalCase(string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

	}
}
