using System;

namespace MvcContrib.UI.Html
{
	/// <summary>
	/// Extension methods to turn quickly turn a value / key pair into an attribute="value" html string
	/// </summary>
	public static class HtmlStringExtensions
	{
		/// <summary>
		/// Returns ' attribute="value"' if value is not null or empty
		/// </summary>
		/// <param name="value">The value to print</param>
		/// <param name="attributeName">The html attribute</param>
		/// <returns>An attribute string</returns>
		public static string AsAttribute(this string value, string attributeName)
		{
			if(string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");
			return string.IsNullOrEmpty(value) ? "" : string.Format(" {0}=\"{1}\"", attributeName, value);
		}

		/// <summary>
		/// Returns ' class="value"' if value is not null or empty
		/// </summary>
		/// <param name="value">The value to print</param>
		/// <returns>An attribute string</returns>
		public static string AsClassAttribute(this string value)
		{
			return value.AsAttribute("class");
		}

	}
}