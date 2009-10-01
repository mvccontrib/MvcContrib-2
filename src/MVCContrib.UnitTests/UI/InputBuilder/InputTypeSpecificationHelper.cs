using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;
using MvcContrib.UnitTests.UI.InputBuilder;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{

    [TestFixture]
    public class InputTypeSpecificationHelper
    {
        public void tostring_should_render_the_inputs()
        {
            //arrange
            var spec = new InputTypeSpecTester();
            spec.Model = new TypeViewModel(){Type = typeof (Model)};
        	  
            //act
            var result = spec.ToString();
            //assert
            Assert.AreEqual("",result);
        }

        
	}

    public class InputTypeSpecTester:InputTypeSpecification<Model>
    {
        protected override void RenderPartial(PropertyViewModel model)
        {
            return;
        }
    }
    public class FakeDisposable:IDisposable
    {
        public void Dispose()
        {
           
		}
	}
}