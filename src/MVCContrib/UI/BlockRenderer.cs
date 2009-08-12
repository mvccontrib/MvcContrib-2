using System;
using System.IO;
using System.Web;

namespace MvcContrib.UI
{
	/// <summary>Renders an Action delegate and captures all output to a string. </summary>
	public class BlockRenderer
	{
		private readonly HttpContextBase _httpContext;

        public BlockRenderer(HttpContextBase httpContext)
		{
			_httpContext = httpContext;
		}

		/// <summary>Renders the action and returns a string.</summary>
		/// <param name="viewRenderer">The delegate to render.</param>
		/// <returns>The rendered text.</returns>
		public string Capture(Action viewRenderer)
		{
			HttpResponseBase resp = _httpContext.Response;
			Stream originalFilter = null;
			CapturingResponseFilter innerFilter;
			string capturedHtml = "";

			if (viewRenderer != null)
			{
				try
				{
					resp.Flush();
					originalFilter = resp.Filter;
					innerFilter = new CapturingResponseFilter(resp.Filter);
					resp.Filter = innerFilter;
					viewRenderer();
					resp.Flush();
					capturedHtml = innerFilter.GetContents(resp.ContentEncoding);
				}
				finally
				{
					if (originalFilter != null)
					{
						resp.Filter = originalFilter;
					}
				}
			}
			return capturedHtml;
		}
	}
}
