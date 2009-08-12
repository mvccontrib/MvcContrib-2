using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.UnitTests.FluentHtml.Fakes
{
	public class FakeViewModelContainer<T> : IViewModelContainer<T> where T : class
	{
		private ViewDataDictionary viewData = new ViewDataDictionary();

	    public FakeViewModelContainer() { }

		public FakeViewModelContainer(string htmlNamePrefix)
		{
			HtmlNamePrefix = htmlNamePrefix;
		}

		public T ViewModel
		{
			get { return viewData.Model as T; }
			set { viewData.Model = value; }
		}

		public IEnumerable<IBehaviorMarker> Behaviors
		{
			get { return new List<IBehaviorMarker>(); }
		}

	    public string HtmlNamePrefix { get; set; }

	    public HtmlHelper Html
	    {
	        get { throw new NotImplementedException(); }
	    }

	    public ViewDataDictionary ViewData
		{
			get { return viewData; }
			set { viewData = value; }
		}
	}
}
