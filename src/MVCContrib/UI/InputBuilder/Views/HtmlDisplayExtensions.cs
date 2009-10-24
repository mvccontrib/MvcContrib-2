using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.InputSpecification;

namespace MvcContrib.UI.InputBuilder.Views
{
	public static class HtmlDisplayExtensions
	{
		public static IInputSpecification<PropertyViewModel> Display<TModel>(this HtmlHelper<TModel> helper,
																																				 Expression<Func<TModel, object>> expression)
			where TModel : class
		{
			IInputSpecification<PropertyViewModel> specification = helper.Input(expression);
			specification.Model.Layout = "Display";
			specification.Model.PartialName = DisplayPartial.Paragraph;
			return specification;
		}

		public static IInputSpecification<PropertyViewModel> Label<TModel>(this HtmlHelper<TModel> helper,
																			 Expression<Func<TModel, object>> expression)
			where TModel : class
		{
			IInputSpecification<PropertyViewModel> specification = helper.Input(expression);
			specification.Model.Layout = "Display";
			specification.Model.PartialName = DisplayPartial.Label;
			return specification;
		}
	}
}