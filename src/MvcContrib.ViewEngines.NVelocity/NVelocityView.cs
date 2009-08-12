using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using NVelocity;

namespace MvcContrib.ViewEngines
{
	public class NVelocityView : IViewDataContainer, IView
	{
		private ViewContext _viewContext;
		private readonly Template _masterTemplate;
		private readonly Template _viewTemplate;

		public NVelocityView(Template viewTemplate, Template masterTemplate)
		{
			_viewTemplate = viewTemplate;
			_masterTemplate = masterTemplate;
		}

		public Template ViewTemplate
		{
			get { return _viewTemplate; }
		}

		public Template MasterTemplate
		{
			get { return _masterTemplate; }
		}

		public ViewDataDictionary ViewData
		{
			get { return _viewContext.ViewData; }
			set { throw new NotSupportedException(); }
		}

		public TempDataDictionary TempData
		{
			get { return _viewContext.TempData; }
			set { throw new NotSupportedException(); }
		}

		public void Render(ViewContext viewContext, TextWriter writer)
		{
			_viewContext = viewContext;
			VelocityContext context = CreateContext();


			bool hasLayout = _masterTemplate != null;
			if(hasLayout)
			{
				//Native NVelocity support for master/child template using #parse. No need to buffer the child template to a Stringwriter
				//which bypasses the response output... and will therefore cause any void child helper extension method
				//calls to fail to render in the correct location. #parse($childContent) must appear in the master template
				//See google group discussion thread: 
				//http://groups.google.com/group/mvccontrib-discuss/browse_thread/thread/0fc84d69db708322?hl=en 

				context.Put("childContent", _viewTemplate.Name);
				_masterTemplate.Merge(context, writer);
			}
			else
			{
				_viewTemplate.Merge(context, writer);
			}
		}

		private VelocityContext CreateContext()
		{
			var entries = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
			if(_viewContext.ViewData != null)
			{
				foreach(var pair in _viewContext.ViewData)
				{
					entries[pair.Key] = pair.Value;
				}
			}
			entries["viewdata"] = _viewContext.ViewData;
			if(_viewContext.TempData != null)
			{
				foreach(var pair in _viewContext.TempData)
				{
					entries[pair.Key] = pair.Value;
				}
			}
			entries["tempdata"] = _viewContext.TempData;

			entries["routedata"] = _viewContext.RouteData;
			entries["controller"] = _viewContext.Controller;
			entries["httpcontext"] = _viewContext.HttpContext;

			CreateAndAddHelpers(entries);

			return new VelocityContext(entries);
		}

		private void CreateAndAddHelpers(Hashtable entries)
		{
			entries["html"] = entries["htmlhelper"] = new HtmlExtensionDuck(_viewContext, this);
			entries["url"] = entries["urlhelper"] = new UrlHelper(_viewContext.RequestContext);
		}
	}
}