using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcContrib.UI.InputBuilder
{
    public class InputPropertySpecification : IInputSpecification
    {
        public Func<HtmlHelper, InputModelProperty, string> Render =
            (helper, model) =>
            {
                //helper.RenderPartial(model.PartialName, model);

                var ViewContext = helper.ViewContext;
                var viewEngineCollection = ViewEngines.Engines;
                var newViewData = new ViewDataDictionary(helper.ViewData) { Model = model };
                ViewContext newViewContext = new ViewContext(ViewContext, ViewContext.View, newViewData, ViewContext.TempData);
                IView view = FindPartialView(newViewContext, model.PartialName, viewEngineCollection, model.Layout );
                
                view.Render(newViewContext, ViewContext.HttpContext.Response.Output);
                

                return "";
            };
       
        internal static IView FindPartialView(ViewContext viewContext, string partialViewName, ViewEngineCollection viewEngineCollection, string masterName) {
            ViewEngineResult result = viewEngineCollection.FindView(viewContext, partialViewName, masterName);
            //ViewEngineResult result = viewEngineCollection.FindPartialView(viewContext, partialViewName);
            if (result.View != null) {
                return result.View;
            }

            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations) {
                locationsText.AppendLine();
                locationsText.Append(location);
            }

            throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                "could not find view {0} looked in {1}", partialViewName, locationsText));
        }
        public InputModelProperty Model { get; set; }

        public HtmlHelper HtmlHelper { get; set; }

        public override string ToString()
        {
            return Render(HtmlHelper, Model);
        }
    }
}