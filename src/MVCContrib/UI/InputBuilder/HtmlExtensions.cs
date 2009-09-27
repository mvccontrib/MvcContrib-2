using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcContrib.UI.InputBuilder
{
    public static class HtmlExtensions
    {
        public static Func<HtmlHelper, string, object, string> Render = (a, b, c) =>
                                                                            {
                                                                                a.RenderPartial(b, c);
                                                                                return "";
                                                                            };

        public static IInputPropertySpecification Input<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, object>> expr) where T : class
        {
            return new Input<T>(htmlHelper).RenderInput(expr);
        }

		//public static IInputSpecification InputForm<T>(this HtmlHelper<T> htmlHelper, string controller, string action) where T : class
		//{
		//    return new Input<T>(htmlHelper).RenderForm(controller,action);
		//}
		public static IInputSpecification InputForm<T>(this HtmlHelper<T> htmlHelper) where T : class
		{
			return new Input<T>(htmlHelper).RenderForm();
		}

        public static string SubmitButton(this HtmlHelper htmlHelper) 
        {
            Render(htmlHelper, "Submit", new InputModelProperty
                                             {
                                                 Example = string.Empty,
                                                 Name = string.Empty,
                                                 PropertyIsRequired = false,
                                                 Label = string.Empty,
                                                 HasValidationMessages = false,

                                             });
            return string.Empty;
        }
    }
}