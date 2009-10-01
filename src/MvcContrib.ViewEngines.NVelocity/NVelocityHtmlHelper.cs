using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcContrib.ViewEngines
{
	/// <summary>
	/// Extension methods for use with NVelocity 
	/// </summary>
	public static class NVelocityHtmlHelper
	{
		public static string TextBox(this HtmlHelper helper, string htmlName, IDictionary htmlAttributes)
		{
			return TextBox(helper, htmlName, string.Empty, htmlAttributes);
		}

		public static string TextBox(this HtmlHelper helper, string htmlName, string value, IDictionary htmlAttributes)
		{
			return helper.TextBox(htmlName, value, MakeGeneric(htmlAttributes)).ToHtmlString();
		}

		private static IDictionary<string, object> MakeGeneric(IDictionary source)
		{
			var toReturn = new Dictionary<string, object>();
			foreach(DictionaryEntry entry in source)
			{
				toReturn.Add(entry.Key.ToString(), entry.Value);
			}
			return toReturn;
		}
	}
}