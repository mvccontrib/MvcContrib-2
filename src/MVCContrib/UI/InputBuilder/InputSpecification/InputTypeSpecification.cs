using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.InputBuilder
{
    public class InputTypeSpecification<T>:IInputSpecification where T : class
    {
		
        public HtmlHelper<T> HtmlHelper { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }


        public override string ToString()
        {
			var factory = new InputModelPropertyFactory<T>(HtmlHelper, InputBuilder.Conventions);
			
			List<InputModelProperty> models = new List<InputModelProperty>();
            foreach (PropertyInfo propertyInfo in Model.Type.GetProperties())
            {
            	models.Add(factory.Create(propertyInfo));
            }
        	HtmlHelper.RenderPartial(Model.PartialName,models.ToArray());
            return string.Empty;
        }

		protected virtual void RenderPartial(InputModelProperty model)
        {
            HtmlHelper.RenderPartial(model.PartialName, model,model.Layout);
        }

        public InputTypeProperty Model{ get; set;}
    }
}