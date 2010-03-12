using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace MvcContrib.TestHelper.Ui
{
	public static class BaseClassLibraryExtensions
	{
		public static bool IsEmpty(this IEnumerable enumerable)
		{
			foreach (object o in enumerable)
			{
				return false;
			}
			return true;
		}

		public static string ConvertToNullSafeString(this object value)
		{
			return value == null ? String.Empty : value.ToString();
		}

		public static bool IsNullOrEmpty(this object value)
		{
			if (value is string)
				return string.IsNullOrEmpty(value as string);
			return value == null;
		}

		public static string ConvertToLowerCamelCase(this string value)
		{
			return value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);
		}

		public static string ConvertToUpperCamelCase(this string value)
		{
			return value.Substring(0, 1).ToUpperInvariant() + value.Substring(1);
		}

		public static string SplitToSeparatedWords(this string value)
		{
			return Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
		}
	}
}