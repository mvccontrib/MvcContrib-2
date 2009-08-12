using System.Web.Mvc;
using MvcContrib.FluentHtml;

namespace MvcContrib.UnitTests.FluentHtml.Views
{
	public class TestableViewPage<T> : ModelViewPage<T> where T : class
	{
		public TestableViewPage(ViewContext viewContext)
		{
			ViewContext = viewContext;
		}

		public new T ViewModel
		{
			get { return ViewData.Model; }
			set { ViewData.Model = value; }
		}
	}
}
