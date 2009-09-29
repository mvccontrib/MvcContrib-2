using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
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

		public static IInputSpecification<PropertyViewModel> Input<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, object>> expr)
			where T : class
		{
			return new Input<T>(htmlHelper).RenderInput(expr);
		}

		public static IInputSpecification<TypeViewModel> InputForm<T>(this HtmlHelper<T> htmlHelper) where T : class
		{
			return new Input<T>(htmlHelper).RenderForm();
		}

		public static string SubmitButton(this HtmlHelper htmlHelper)
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
		public static void RenderPartials(this HtmlHelper helper, PropertyViewModel[] model)
		{
			foreach(var viewModel in model)
			{
				helper.RenderPartial((TypeViewModel)viewModel);	
			}			
		}
		public static void RenderPartial(this HtmlHelper helper, TypeViewModel model)
		{
			helper.RenderPartial(model.PartialName,model,model.Layout);
		}
		public static void RenderPartial(this HtmlHelper helper, string partial, object model, string master)
		{
			var ViewContext = helper.ViewContext;
			var viewEngineCollection = ViewEngines.Engines;
			var newViewData = new ViewDataDictionary(helper.ViewData) { Model = model };
			ViewContext newViewContext = new ViewContext(ViewContext, ViewContext.View, newViewData, ViewContext.TempData);
			IView view = FindPartialView(newViewContext, partial, viewEngineCollection, master);

			view.Render(newViewContext, ViewContext.HttpContext.Response.Output);
		}
		private static IView FindPartialView(ViewContext viewContext, string partialViewName, ViewEngineCollection viewEngineCollection, string masterName)
		{
			ViewEngineResult result = viewEngineCollection.FindView(viewContext, partialViewName, masterName);
			if (result.View != null)
			{
				return result.View;
			}

			StringBuilder locationsText = new StringBuilder();
			foreach (string location in result.SearchedLocations)
			{
				locationsText.AppendLine();
				locationsText.Append(location);
			}

			throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
				"could not find view {0} looked in {1}", partialViewName, locationsText));
		}
	}
}