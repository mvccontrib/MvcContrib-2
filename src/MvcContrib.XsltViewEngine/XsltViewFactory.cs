using System;
using System.Web.Hosting;
using System.Web.Mvc;
using MvcContrib.XsltViewEngine;

namespace MvcContrib.ViewFactories
{
	public class XsltViewFactory : VirtualPathProviderViewEngine
	{
		public XsltViewFactory(VirtualPathProvider virtualPathProvider)
		{
			if(virtualPathProvider != null)
			{
				VirtualPathProvider = virtualPathProvider;
			}

			MasterLocationFormats = new string[0];

			ViewLocationFormats = new[]
			                      	{
			                      		"~/Views/{1}/{0}.xslt",
			                      		"~/Views/Shared/{0}.xslt"
			                      	};

			PartialViewLocationFormats = ViewLocationFormats;
		}

		public XsltViewFactory() : this(null)
		{
		}

		protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
		{
			return CreateView(controllerContext, partialPath, null);
		}

		protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
		{
			if(!(controllerContext.Controller.ViewData.Model is XsltViewData))
			{
				throw new ArgumentException("the view data object should be of type XsltViewData");
			}

			var viewTemplate = new XsltTemplate(VirtualPathProvider, viewPath);

			var view = new XsltView(viewTemplate, controllerContext.Controller.ViewData.Model as XsltViewData, string.Empty,
			                        controllerContext.HttpContext);
			return view;
		}
	}
}