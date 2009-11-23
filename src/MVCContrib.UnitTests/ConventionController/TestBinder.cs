using System;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests.ConventionController
{
	[Obsolete("Consider using System.Web.Mvc.DefaultModelBinder instead.")]
	public class TestBinder : AbstractParameterBinderAttribute
	{
		public TestBinder() : base(null)
		{
		}


	    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
	    {
            ((TestController)controllerContext.Controller).BinderFilterOrdering += "Binder";
            return null;
        }
	}
}