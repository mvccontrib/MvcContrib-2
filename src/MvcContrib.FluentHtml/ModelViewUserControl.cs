using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewUserControl<T> : ViewUserControl<T>, IViewModelContainer<T> where T : class
	{
		protected readonly List<IBehaviorMarker> behaviors = new List<IBehaviorMarker>();
		protected string htmlNamePrefix;

		public ModelViewUserControl()
		{
			behaviors.Add(new ValidationBehavior(() => ViewData.ModelState));
		}

		public ModelViewUserControl(params IBehaviorMarker[] behaviors) : this(null, behaviors) { }

		public ModelViewUserControl(string htmlNamePrefix, params IBehaviorMarker[] memberBehaviors) : this()
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
			get { return ViewData.Model; }
		}

		public IEnumerable<IBehaviorMarker> Behaviors
		{
			get { return behaviors; }
		}

		HtmlHelper IViewModelContainer<T>.Html
		{
			get { return base.Html; }
		}
	}
}
