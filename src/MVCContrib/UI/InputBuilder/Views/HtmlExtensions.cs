using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.InputSpecification;

namespace MvcContrib.UI.InputBuilder.Views
{
	public static class HtmlExtensions
	{
		public static Func<HtmlHelper, PropertyViewModel, string> Render = (a, b) =>
		{
			a.RenderPartial(b);
			return "";
		};

		public static IInputSpecification<PropertyViewModel> Input<T>(this HtmlHelper<T> htmlHelper,
		                                                              Expression<Func<T, object>> expr)
			where T : class
		{
			return new Input<T>(htmlHelper).RenderInput(expr);
		}

		public static IInputSpecification<TypeViewModel> InputForm<T>(this HtmlHelper<T> htmlHelper) where T : class
		{
			return new Input<T>(htmlHelper).RenderForm();
		}

		public static string InputButtons(this HtmlHelper htmlHelper)
		{
			Render(htmlHelper, new PropertyViewModel
			{
				PartialName = "Submit",
				Layout = "Field",
				Example = string.Empty,
				Name = string.Empty,
				PropertyIsRequired = false,
				Label = string.Empty,
				HasValidationMessages = false,
			});
			return string.Empty;
		}

		public static void RenderPartial(this HtmlHelper helper, PropertyViewModel model)
		{
			helper.RenderPartial((TypeViewModel)model);
		}

		public static void InputFields(this HtmlHelper helper, PropertyViewModel[] model)
		{
			foreach(PropertyViewModel viewModel in model)
			{
				helper.RenderPartial((TypeViewModel)viewModel);
			}
		}
		public static void InputFields(this HtmlHelper helper, IEnumerable<TypeViewModel> model)
		{
			foreach (TypeViewModel viewModel in model)
			{
				helper.RenderPartial(viewModel);
			}
		}

		public static void RenderPartial(this HtmlHelper helper, TypeViewModel model)
		{
			helper.RenderPartial(model.PartialName, model, model.Layout);
		}

		public static void RenderPartial(this HtmlHelper helper, string partial, object model, string master)
		{
			try
			{
				ViewContext ViewContext = helper.ViewContext;
				ViewEngineCollection viewEngineCollection = ViewEngines.Engines;
				var newViewData = new ViewDataDictionary(helper.ViewData) {Model = model};
				var newViewContext = new ViewContext(ViewContext, ViewContext.View, newViewData, ViewContext.TempData, ViewContext.Writer);
				IView view = FindPartialView(newViewContext, partial, viewEngineCollection, master);
				view.Render(newViewContext, ViewContext.HttpContext.Response.Output);
			}
			catch (Exception ex)
			{
				string message = string.Format("Error trying to render the partial:{0} with master:{1} for model type:{2}"
					, partial, master, model.GetType().Name);

				throw new RenderInputBuilderException(message, ex);
			}
		}

		private static IView FindPartialView(ViewContext viewContext, string partialViewName,
		                                     ViewEngineCollection viewEngineCollection, string masterName)
		{
			ViewEngineResult result = viewEngineCollection.FindView(viewContext, partialViewName, masterName);
			if(result.View != null)
			{
				return result.View;
			}

			var locationsText = new StringBuilder();
			foreach(string location in result.SearchedLocations)
			{
				locationsText.AppendLine();
				locationsText.Append(location);
			}

			throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
			                                                  "could not find view {0} looked in {1}", partialViewName,
			                                                  locationsText));
		}
	}
}