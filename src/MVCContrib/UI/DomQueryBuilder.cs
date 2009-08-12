using System.Collections.Generic;
using System.Web.UI;

namespace MvcContrib.UI
{
	public class DomQueryBuilder
	{
		private readonly List<string> _queryList = new List<string>();
		private readonly List<string> _ids = new List<string>();
		private bool _hasOnlyIds = true;
		private string _currentQuery;


		public DomQueryBuilder()
		{
		}


		public DomQueryBuilder(string id)
		{
			AddId(id);
		}

		private DomQueryBuilder AddId(string id)
		{
			_ids.Add(id);
			_currentQuery += "#" + id;
			return this;
		}

		public DomQueryBuilder Id(string id)
		{
			return AddId(id);
		}

		public DomQueryBuilder Class(string @class)
		{
			_hasOnlyIds = false;
			_currentQuery += "." + @class;
			return this;
		}

		public DomQueryBuilder Tag(string tag)
		{
			_hasOnlyIds = false;
			_currentQuery += tag;
			return this;
		}

		public DomQueryBuilder Tag(HtmlTextWriterTag tag)
		{
			return Tag(tag.ToString().ToLowerInvariant());
		}

		public DomQueryBuilder And
		{
			get
			{
				AddToList();
				return this;
			}
		}

		public DomQueryBuilder Descendant
		{
			get
			{
				_currentQuery += " ";
				return this;
			}
		}

		private void AddToList()
		{
			if (string.IsNullOrEmpty(_currentQuery))
			{
				return;
			}

			if (_queryList.Count > 0)
			{
				_currentQuery = "," + _currentQuery.Trim();
			}

			_queryList.Add(_currentQuery.Trim());
			_currentQuery = "";
		}

		public DomQuery ToDomQuery()
		{
			AddToList();
			return new DomQuery(string.Join("", _queryList.ToArray()), _hasOnlyIds, _ids);
		}

		public static implicit operator DomQuery(DomQueryBuilder builder)
		{
			return builder.ToDomQuery();
		}

		public static implicit operator string(DomQueryBuilder builder)
		{
			return builder.ToDomQuery().ToString();
		}
	}
}