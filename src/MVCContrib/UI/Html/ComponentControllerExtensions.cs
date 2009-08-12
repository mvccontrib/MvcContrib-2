using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcContrib.ExtendedComponentController;

namespace MvcContrib.UI.Html
{
	public static class ComponentControllerExtensions
	{
		public static string RenderComponent2<TController>(this HtmlHelper helper,Action<TController> action) where TController:ComponentController
		{
			var type = typeof(TController);
			var controller = ComponentControllerBuilder.Current.GetComponentControllerFactory()
				.CreateComponentController(type) as TController;
			controller.Context = helper.ViewContext;
			action.Invoke(controller);
			return controller.RenderedHtml;
		}
	}
}
