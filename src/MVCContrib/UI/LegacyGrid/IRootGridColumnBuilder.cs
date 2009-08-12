using System;
using System.Linq.Expressions;

namespace MvcContrib.UI.LegacyGrid
{
	/// <summary>
	/// Used in the construction of grid columns.
	/// </summary>
	/// <typeparam name="T">Type of object to generate grid rows for.</typeparam>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]	
	public interface IRootGridColumnBuilder<T> where T : class
	{
		/// <summary>
		/// Uses a lambda expression to specify which property the column should be rendered for.
		/// </summary>
		/// <param name="expression">Lambda expression for the property.</param>
		/// <returns>A Column builder.</returns>
		IExpressionColumnBuilder<T> For(Expression<Func<T, object>> expression);
		/// <summary>
		/// Uses a lambda expression to specify which property the column should be rendered for.
		/// </summary>
		/// <param name="func">Lambda expression for the property.</param>
		/// <param name="name">Custom column heading.</param>
		/// <returns>A Column builder.</returns>
		INestedGridColumnBuilder<T> For(Func<T, object> func, string name);
		/// <summary>
		/// Specifies a string representation of a property name for which a column should be created. 
		/// Using this approach will resort to using reflection to obtain the property value.
		/// </summary>
		/// <param name="name">Property name to generate the column for.</param>
		/// <returns>A Column builder</returns>
		ISimpleColumnBuilder<T> For(string name);
	}
}