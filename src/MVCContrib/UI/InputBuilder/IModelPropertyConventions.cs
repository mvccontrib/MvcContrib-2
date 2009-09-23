using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
    public interface IModelPropertyConventions
    {
        string ExampleForPropertyConvention(PropertyInfo propertyInfo);
        object ValueFromModelPropertyConvention(PropertyInfo propertyInfo,object model);
        string LabelForPropertyConvention(PropertyInfo propertyInfo);
        bool ModelIsInvalidConvention<T>(PropertyInfo propertyInfo,HtmlHelper<T> htmlHelper) where T : class;
        string PropertyNameConvention(PropertyInfo propertyInfo);
        Type PropertyTypeConvention(PropertyInfo propertyInfo);
        string PartialNameConvention(PropertyInfo propertyInfo);
        InputModelProperty ModelPropertyBuilder(PropertyInfo propertyInfo,object model);
        bool PropertyIsRequiredConvention(PropertyInfo propertyInfo);
        string Layout(string partialName);
    }
}