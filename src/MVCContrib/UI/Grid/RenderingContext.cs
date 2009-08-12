using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Context class used when rendering the grid.
	/// </summary>
	public class RenderingContext
	{
		public TextWriter Writer { get; private set; }
		public ViewContext ViewContext { get; private set; }
		public ViewEngineCollection ViewEngines { get; private set; }

		public RenderingContext(TextWriter writer, ViewContext viewContext, ViewEngineCollection viewEngines)
		{
			Writer = writer;
			ViewContext = viewContext;
			ViewEngines = viewEngines;
		}
	}
}