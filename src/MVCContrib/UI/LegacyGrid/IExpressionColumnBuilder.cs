
namespace MvcContrib.UI.LegacyGrid
{
	/// <summary>
	/// Used in the construction of columns that have been specified using a lambda expression.
	/// </summary>
	///<typeparam name="T">Type of object to generate grid rows for.</typeparam>
	[System.Obsolete("The old version of the grid has been deprecated. Please switch to the version located in MvcContrib.UI.Grid")]	
	public interface IExpressionColumnBuilder<T> : INestedGridColumnBuilder<T> where T : class
	{
		/// <summary>
		/// By default, a property such as "CustomerName" will be be parsed in order to produce a column heading of "Customer Name".
		/// Calling DoNotSplit will disable the parsing of the property names for this column heading.
		/// </summary>
		/// <returns></returns>
		INestedGridColumnBuilder<T> DoNotSplit();
	}
}