using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
    [TestFixture]
    public class IInputSpecificationExtensionsTester
    {
        [Test]
        public void methods_should_modify_the_underlying_model()
        {
            //arrange
            var inputSpecification = new InputPropertySpecification();
            inputSpecification.Model= new PropertyViewModel();

            //act
        	inputSpecification
				.MaxLength(5)				
        		.Example("new example")
        		.Required()
        		.Partial("partial")
        		.Label("label");



			//assert
			Assert.AreEqual(5, inputSpecification.Model.AdditionalValues["maxlength"]);
			Assert.AreEqual("partial", inputSpecification.Model.PartialName);
			Assert.AreEqual("new example", inputSpecification.Model.Example);
			Assert.AreEqual("label", inputSpecification.Model.Label);
			Assert.IsTrue(inputSpecification.Model.PropertyIsRequired);
		}

	}
	public static class SetUserExtensions
	{
		public static IInputSpecification<PropertyViewModel> MaxLength(this IInputSpecification<PropertyViewModel> inputSpecification, int length)
		{
			inputSpecification.Model.AdditionalValues.Add("maxlength", length);
			return inputSpecification;
		}
	}
}