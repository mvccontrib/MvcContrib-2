using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcContrib.UI.InputBuilder
{
    public class InputPropertySpecification : IInputSpecification
    {
        public Func<HtmlHelper, InputModelProperty, string> Render =
            (helper, model) =>
            {
                helper.RenderPartial(model.PartialName, model);
                return "";
            };
       
        public InputModelProperty Model { get; set; }

        public HtmlHelper HtmlHelper { get; set; }

        public override string ToString()
        {
            return Render(HtmlHelper, Model);
        }
    }
}