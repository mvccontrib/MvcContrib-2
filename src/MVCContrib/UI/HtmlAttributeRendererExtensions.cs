using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
    public static class HtmlAttributeRendererExtensions
    {
        /// <summary>
        /// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
        /// </summary>
        /// <returns></returns>
        public static string BuildHtmlAttributes(this IDictionary<string, object> attributes)
        {
            return BuildHtmlAttributes(attributes, false);
        }

        /// <summary>
        /// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
        /// </summary>
        /// <param name="startSpace">Should it start with a space if <param name="attributes"/> isn't empty.</param>
        /// <returns></returns>
        public static string BuildHtmlAttributes(this IDictionary<string, object> attributes, bool startSpace)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            const string attributeFormat = "{0}=\"{1}\"";

            string[] strings = attributes.Select(pair => string.Format(attributeFormat, pair.Key, pair.Value)).ToArray();

            return startSpace ? string.Format(" {0}", string.Join(" ", strings)) : string.Join(" ", strings);
        }
    }
}
