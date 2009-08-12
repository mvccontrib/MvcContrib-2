using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class InputTester
    {
        [Test]
        public void The_input_should_a_property_specification()
        {
            //arrange
            Model model=new Model(){String = "foo"};
            var input = new MvcContrib.UI.InputBuilder.Input<Model>(InputModelPropertyFactoryTester.CreateHelper(model));
            
            //act
            var result = input.RenderInput(m => m.String);
            
            //assert
            Assert.IsInstanceOfType(typeof(MvcContrib.UI.InputBuilder.InputPropertySpecification),result);
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Input_should_build_a_InputTypeSpecification()
        {
            Model model = new Model() { String = "foo" };
            var input = new MvcContrib.UI.InputBuilder.Input<Model>(InputModelPropertyFactoryTester.CreateHelper(model));

            //act
            var result = input.RenderForm("foo", "bar");

            //assert
            Assert.IsInstanceOfType(typeof(MvcContrib.UI.InputBuilder.InputTypeSpecification<Model>), result);            
        }

    }
}