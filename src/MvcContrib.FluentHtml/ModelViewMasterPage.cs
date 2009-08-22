using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewMasterPage<T> : ViewMasterPage<T>, IViewModelContainer<T> where T : class
	{
		protected readonly List<IBehaviorMarker> behaviors = new List<IBehaviorMarker>();
		protected string htmlNamePrefix;

		public ModelViewMasterPage()
		{
			behaviors.Add(new ValidationBehavior(() => ViewData.ModelState));
		}

		public ModelViewMasterPage(params IBehaviorMarker[] behaviors) : this(null, behaviors) { }

		public ModelViewMasterPage(string htmlNamePrefix, params IBehaviorMarker[] memberBehaviors) : this()
		{
			this.htmlNamePrefix = htmlNamePrefix;
			if (memberBehaviors != null)
			{
				behaviors.AddRange(memberBehaviors);
			}
		}

		public string HtmlNamePrefix
		{
			get { return htmlNamePrefix; }
			set { htmlNamePrefix = value; }
		}

		public T ViewModel
		{
			get { return ViewData.Model as T; }
		}

		public IEnumerable<IBehaviorMarker> Behaviors
		{
			get { return behaviors; }
		}

		public new ViewDataDictionary ViewData
		{
			get { return base.ViewData; }
			set { throw new NotImplementedException("ViewData from base class ViewMasterPage<T> is read-only."); }
		}

		HtmlHelper IViewModelContainer<T>.Html 
		{
			get { return base.Html; }
		}
	}
}
