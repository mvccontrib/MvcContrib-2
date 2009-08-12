using MvcContrib.UI.InputBuilder;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class IInputSpecificationExtensionsTester
    {
        [Test]
        public void testname()
        {
            //arrange
            var inputSpecification = new InputPropertySpecification();
            inputSpecification.Model= new InputModelProperty();

            //act
            inputSpecification
                .Partial("partial")
                .Example("new example")
                .Label("label")
                .Required();

            //assert
            Assert.AreEqual("partial", inputSpecification.Model.PartialName);
            Assert.AreEqual("new example", inputSpecification.Model.Example);
            Assert.AreEqual("label", inputSpecification.Model.Label);
            Assert.IsTrue(inputSpecification.Model.PropertyIsRequired);
        }

        
    }
}