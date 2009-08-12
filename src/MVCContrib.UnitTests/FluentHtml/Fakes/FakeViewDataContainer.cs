using System.Web.Mvc;

namespace MvcContrib.UnitTests.FluentHtml.Fakes
{
	public class FakeViewDataContainer : IViewDataContainer
	{
		private ViewDataDictionary viewData = new ViewDataDictionary();

		public ViewDataDictionary ViewData
		{
			get { return viewData; }
			set { viewData = value; }
		}
	}
}
