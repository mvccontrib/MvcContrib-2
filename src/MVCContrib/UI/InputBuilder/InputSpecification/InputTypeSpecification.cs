using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.InputBuilder
{
    public class InputTypeSpecification<T> : IInputSpecification where T : class
    {
        public Type ModelType { get; set; }

        public HtmlHelper<T> HtmlHelper { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }


        public override string ToString()
        {
            using (RenderForm())
            {
                foreach (PropertyInfo propertyInfo in ModelType.GetProperties())
                {
                    InputModelProperty model = new InputModelPropertyFactory<T>(HtmlHelper, InputBuilder.Conventions).Create(propertyInfo);

                    RenderPartial(model);
                }
                RenderSubmitButton();
            }
            return string.Empty;
        }

        protected virtual IDisposable RenderForm()
        {
            return HtmlHelper.BeginForm(Action, Controller);
        }

        protected virtual void RenderSubmitButton()
        {
            HtmlHelper.SubmitButton();
        }

        protected virtual void RenderPartial(InputModelProperty model)
        {
            HtmlHelper.RenderPartial(model.PartialName, model,model.Layout);
        }

        public InputModelProperty Model
        {
            get { throw new InvalidOperationException("As cannot be specified on an InputForm"); }
        }
    }
}