using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcContrib.FluentHtml
{
	public static class StringExtensions
	{
		public static string FormatAsHtmlId(this string name)
		{
			//Replace charactes not valid for ID attribute with underscores.
			//Replace dots with underscores to distinguish from name attribute.
			return string.IsNullOrEmpty(name) 
				? string.Empty 
				: Regex.Replace(name, "[^a-zA-Z0-9-:]", "_");
		}

		public static string FormatAsHtmlName(this string name)
		{
			//Replace charactes not valid for name attribute with underscores.
			return string.IsNullOrEmpty(name)
				? string.Empty
				: Regex.Replace(name, "[^a-zA-Z0-9.-:]", "_");
		}

		/// <summary>
		/// Converts pascal case string into a phrase.  Treats consecutive capital 
		/// letters as a word.  For example, 'HasABCDAcronym' would be tranformed to
		/// 'Has ABCD Acronym'
		/// </summary>
		/// <param name="s">A string containing pascal case words</param>
		public static string PascalCaseToPhrase(this string s)
		{
			var result = new StringBuilder();
			var letters = s.ToCharArray();
			for (var i = 0; i < letters.Length; i++)
			{
				var isFirstLetter = i == 0;
				var isLastLetter = i == letters.Length;
				var isCurrentLetterUpper = Char.IsUpper(letters[i]);
				var isPrevLetterLower = i > 0 && Char.IsLower(letters[i - 1]);
				var isNextLetterLower = i + 1 < letters.Length && Char.IsLower(letters[i + 1]);
				var isPrevCharIsSpace = i > 0 && letters[i - 1] == ' ';
				if (!isFirstLetter &&
					isCurrentLetterUpper &&
					!isPrevCharIsSpace &&
					(isPrevLetterLower || isNextLetterLower || isLastLetter))
				{
					result.Append(" ");
				}
				result.Append(letters[i].ToString());
			}
			return result.ToString();

		}
	}
}