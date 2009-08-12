using System;
using System.Web.Mvc;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.ViewFactories
{
	public class BrailViewFactory : IViewEngine
	{
		private readonly BooViewEngine _viewEngine;

		public BrailViewFactory()
			: this(DefaultViewEngine)
		{
		}

		public BrailViewFactory(BooViewEngine viewEngine)
		{
			if (viewEngine == null) throw new ArgumentNullException("viewEngine");

			_viewEngine = viewEngine;
		}

		public BooViewEngine ViewEngine
		{
			get { return _viewEngine; }
		}

		private static BooViewEngine _defaultViewEngine;
		private static BooViewEngine DefaultViewEngine
		{
			get
			{
				if( _defaultViewEngine == null )
				{
					_defaultViewEngine = new BooViewEngine();
					_defaultViewEngine.Initialize();
				}

				return _defaultViewEngine;
			}
		}


	    public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
	    {
			return FindView(controllerContext, partialViewName, null, useCache);
	    }

	    public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
	    {
			var controller = controllerContext.RouteData.Values["controller"] as string;

			string fullViewName = string.Concat(controller, "/", viewName);

			IView view = _viewEngine.Process(fullViewName, masterName);

			return new ViewEngineResult(view, this);
        }

	    public void ReleaseView(ControllerContext controllerContext, IView view)
	    {
	    }
	}
}
