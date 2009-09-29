using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
    [TestFixture]
    public class IInputSpecificationExtensionsTester
    {
        [Test]
        public void testname()
        {
            //arrange
            var inputSpecification = new InputPropertySpecification();
            inputSpecification.Model= new PropertyViewModel();

            //act
        	inputSpecification
        		.Example("new example")
        		.Required()
        		.Partial("partial")
        		.Label("label");


			//assert
			Assert.AreEqual("partial", inputSpecification.Model.PartialName);
			Assert.AreEqual("new example", inputSpecification.Model.Example);
			Assert.AreEqual("label", inputSpecification.Model.Label);
			Assert.IsTrue(inputSpecification.Model.PropertyIsRequired);
		}

        
	}
}