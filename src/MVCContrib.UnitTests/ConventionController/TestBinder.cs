using System;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests.ConventionController
{
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