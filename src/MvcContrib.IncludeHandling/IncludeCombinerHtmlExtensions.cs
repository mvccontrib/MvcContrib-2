using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Filters;
using MvcContrib.Services;

namespace MvcContrib.IncludeHandling
{
	public static class IncludeCombinerHtmlExtensions
	{
		public static string RenderIncludes(this HtmlHelper helper, IncludeType type)
		{
			return helper.RenderIncludes(type, helper.IsInDebugMode());
		}
		
		public static string RenderIncludes(this HtmlHelper helper, IEnumerable<string> includes, IncludeType type)
		{
			return helper.RenderIncludes(includes, type, helper.IsInDebugMode());
		}

		public static string RenderIncludes(this HtmlHelper helper, IEnumerable<string> includes, IncludeType type, bool isInDebugMode) 
		{
			var combiner = DependencyResolver.GetImplementationOf<IIncludeCombiner>();
			var toRender = combiner.RenderIncludes(includes, type, isInDebugMode);
			return toRender;
		}

		public static string RenderIncludes(this HtmlHelper helper, IncludeType type, bool isInDebugMode)
		{
			var sources = helper.ViewData[getViewDataKey(type)] as IList<string> ?? new List<string>();
			var toRender = helper.RenderIncludes(sources, type, isInDebugMode);
			helper.ViewData[getViewDataKey(type)] = new List<string>();
			return toRender;
		}

		public static string RenderCss(this HtmlHelper helper)
		{
			return helper.RenderCss(helper.IsInDebugMode());
		}

		public static string RenderCss(this HtmlHelper helper, bool isInDebugMode)
		{
			return helper.RenderIncludes(IncludeType.Css, isInDebugMode);
		}

		public static string RenderCss(this HtmlHelper helper, IEnumerable<string> includes)
		{
			return helper.RenderIncludes(includes, IncludeType.Css, helper.IsInDebugMode());
		}

		public static string RenderCss(this HtmlHelper helper, IEnumerable<string> includes, bool isInDebugMode)
		{
			return helper.RenderIncludes(includes, IncludeType.Css, isInDebugMode);
		}

		public static string RenderJs(this HtmlHelper helper)
		{
			return helper.RenderJs(helper.IsInDebugMode());
		}

		public static string RenderJs(this HtmlHelper helper, bool isInDebugMode)
		{
			return helper.RenderIncludes(IncludeType.Js, isInDebugMode);
		}

		public static string RenderJs(this HtmlHelper helper, IEnumerable<string> includes)
		{
			return helper.RenderIncludes(includes, IncludeType.Js, helper.IsInDebugMode());
		}

		public static string RenderJs(this HtmlHelper helper, IEnumerable<string> includes, bool isInDebugMode)
		{
			return helper.RenderIncludes(includes, IncludeType.Js, isInDebugMode);
		}

		public static void Include(this HtmlHelper helper, IncludeType type, string source)
		{
			var sourcesToInclude =
				helper.ViewData[getViewDataKey(type)] as IList<string> ?? new List<string>();
			if (!sourcesToInclude.Contains(source))
			{
				sourcesToInclude.Add(source);
			}
			helper.ViewData[getViewDataKey(type)] = sourcesToInclude;
		}

		public static void Include(this HtmlHelper helper, IncludeType type, params string[] sources)
		{
			foreach (var path in sources)
			{
				helper.Include(type, path);
			}
		}

		public static void IncludeCss(this HtmlHelper helper, string source)
		{
			helper.Include(IncludeType.Css, source);
		}

		public static void IncludeCss(this HtmlHelper helper, params string[] sources)
		{
			helper.Include(IncludeType.Css, sources);
		}

		public static void IncludeJs(this HtmlHelper helper, string source)
		{
			helper.Include(IncludeType.Js, source);
		}

		public static void IncludeJs(this HtmlHelper helper, params string[] sources)
		{
			helper.Include(IncludeType.Js, sources);
		}

		private static string getViewDataKey(IncludeType type)
		{
			return typeof (IncludeCombinerHtmlExtensions).FullName + "_" + type;
		}
	}
}