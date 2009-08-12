using System;

namespace MvcContrib.UI.LegacyGrid
{
	/// <summary>
	/// Used to provide custon sections to the grid.
	/// </summary>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]	
	public interface IGridSections<T>
	{
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowStart(Action<T> block);
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row with alternate row as additional parameter.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowStart(Action<T, bool> block);
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the end of the row.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowEnd(Action<T> block);
	}
}