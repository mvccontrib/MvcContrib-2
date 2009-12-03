using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Attributes;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.InputSpecification;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
    [TestFixture]
    public class InputTester
    {
        [Test]
        public void The_input_should_a_property_specification()
        {
            //arrange
            Model model = new Model() { StringProp = "foo" };
            var input = new Input<Model>(InputModelPropertyFactoryTester.CreateHelper(model));

            //act
            var result = input.RenderInput(m => m.StringProp);

            //assert
            Assert.IsInstanceOf<InputPropertySpecification>(result);
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Input_should_build_a_InputTypeSpecification()
        {
            Model model = new Model() { StringProp = "foo" };
            var input = new Input<Model>(InputModelPropertyFactoryTester.CreateHelper(model));

            //act
            var result = input.RenderForm("foo", "bar");

            //assert
            Assert.IsInstanceOf<InputTypeSpecification<Model>>(result);
        }

    }


}