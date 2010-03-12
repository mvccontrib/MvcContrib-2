using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcContrib.TestHelper.Ui
{
	public class DisplayTable<T>
	{
		protected List<RowFilter<T>> filters = new List<RowFilter<T>>();
		protected readonly IBrowserDriver _browser;

		public DisplayTable(IBrowserDriver browser)
		{
			_browser = browser;
		}

		public DisplayTable<T> AddRowFilter(Expression<Func<T, object>> expression, string value)
		{
			filters.Add(new RowFilter<T>(expression, value));
			return this;
		}

		public bool VerifyRowExists()
		{
			return _browser.GetRowCount(typeof(T).Name, filters) > 0;
		}

		public void ClickLink(string relId)
		{
			_browser.ClickRowLink(typeof(T).Name, filters, relId);
		}
	}
}