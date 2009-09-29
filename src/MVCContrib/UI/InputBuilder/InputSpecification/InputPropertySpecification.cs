using System;
using System.Web.Mvc;
using MvcContrib.UI.Html;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.InputSpecification
{
	public class InputPropertySpecification : IInputSpecification<PropertyViewModel>,IInputSpecification<TypeViewModel>
	{
		public Func<HtmlHelper, PropertyViewModel, string> Render =
			(helper, model) =>
			{
				helper.RenderPartial(model.PartialName, model, model.Layout);
				return "";
			};

		public HtmlHelper HtmlHelper { get; set; }


		public PropertyViewModel Model { get; set; }


		public override string ToString()
		{
			return Render(HtmlHelper, Model);
		}

		TypeViewModel IInputSpecification<TypeViewModel>.Model
		{
			get
			{
				return this.Model;
			}
		}
	}
}