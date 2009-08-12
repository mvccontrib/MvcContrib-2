using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace MvcContrib.UI
{
	/// <summary>A representation of a query to find Elements in the Dom.</summary>
	public class DomQuery
	{
		private readonly string _query;
		private readonly bool _hasOnlyIds;
		private readonly IEnumerable<string> _ids;
		private readonly bool _isSimple;

		public DomQuery(string query, string id)
		{
			_query = query;
			_hasOnlyIds = true;
			_isSimple = true;
			_ids = new[] {id};
			IsSingle = true;
		}
		public DomQuery(string query, bool hasOnlyIds, IEnumerable<string> ids)
		{
			_query = query;
			_hasOnlyIds = hasOnlyIds;
			_ids = ids;

			if(hasOnlyIds && ids.Count() == 1)
			{
				_isSimple = true;
				IsSingle = true;
			}
				
		}

		public string Id
		{
			get
			{
				if(!IsSimple)
				{
					throw new InvalidOperationException("Id can only be gotten if the DomQuery.IsSimple == true");
				}
				return Ids.First();
			}
		}
		/// <summary>True if the query is only a single element id</summary>
		public bool IsSimple
		{
			get { return _isSimple; }
		}

		/// <summary>True if the query string is only a selection of html id's.</summary>
		/// <remarks>IJSGenerator implementors can use this to develop more efficient
		/// javascript than always calling a css selector function.</remarks>
		public bool HasOnlyIds
		{
			get { return _hasOnlyIds; }
		}

		/// <summary>The list of raw id's that are included in the query string.</summary>
		public IEnumerable<string> Ids
		{
			get { return _ids; }
		}

		public bool IsSingle { get; set; }

		/// <summary>The query string.</summary>
		/// <returns>The CSS Selector query string.</returns>
		public override string ToString()
		{
			return _query;
		}

		public static DomQuery Parse(string query)
		{
			Match match = IdOnly.Match(query);
			if(match.Success)
			{
				return new DomQueryBuilder().Id(match.Groups["id"].Value);
			}
			return new DomQuery(query, false, Enumerable.Empty<string>());
		}

		private static Regex IdOnly =
			new Regex(@"^\s*#(?<id>[a-zA-Z](?:[\w-])+)\s*$", RegexOptions.Compiled);

		/// <summary>Converts a regular string into a DomQuery.</summary>
		/// <param name="query">The CSS Selector query.</param>
		/// <returns>The DomQuery</returns>
		/// <remarks>The query will always have <see cref="DomQuery.HasOnlyIds"/> <c>== false</c>
		/// and will always return an empty collection of <see cref="DomQuery.Ids"/>.
		/// </remarks>
		public static implicit operator DomQuery(string query)
		{
			return Parse(query);
		}

		public static implicit operator string(DomQuery query)
		{
			if(query != null)
				return query.ToString();
			return null;
		}
	}
}
