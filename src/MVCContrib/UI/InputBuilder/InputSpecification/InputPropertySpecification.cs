using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.InputBuilder
{
    public class InputPropertySpecification : IInputPropertySpecification
    {
        public Func<HtmlHelper, InputModelProperty, string> Render =
            (helper, model) =>
            {
				helper.RenderPartial(model.PartialName, model,model.Layout);
                return "";
            };
       
        public InputModelProperty Model { get; set; }

        public HtmlHelper HtmlHelper { get; set; }

        public override string ToString()
        {
            return Render(HtmlHelper, Model);
        }

    	InputTypeProperty IInputSpecification.Model
    	{
    		get
    		{
    			return Model;
    		}
    	}
    }
}