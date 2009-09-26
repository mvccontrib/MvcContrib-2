using System;
using System.Collections.Generic;

namespace MvcContrib
{
	public static class StringExtensions
	{
		public static IList<TEnum> CastToEnum<TEnum>(this IEnumerable<string> strings)
		{
			return strings.CastToEnum<TEnum>(false);
		}

		public static IList<TEnum> CastToEnum<TEnum>(this IEnumerable<string> strings, bool ignoreCase)
		{
			if (typeof (TEnum).BaseType != typeof (Enum))
			{
				throw new ArgumentException("Must be called on Enum type; actual type: " + typeof (TEnum), "strings");
			}
			var enums = new List<TEnum>();
			foreach (var s in strings)
			{
				enums.Add(s.CastToEnum<TEnum>(ignoreCase));
			}
			return enums;
		}

		public static TEnum CastToEnum<TEnum>(this string s)
		{
			return s.CastToEnum<TEnum>(false);
		}

		public static TEnum CastToEnum<TEnum>(this string s, bool ignoreCase)
		{
			if (typeof (TEnum).BaseType != typeof (Enum))
			{
				throw new ArgumentException("Must be called on Enum type; actual type: " + typeof (TEnum), "s");
			}
			var result = (TEnum) Enum.Parse(typeof (TEnum), s, ignoreCase);
			return result;
		}
	}
}