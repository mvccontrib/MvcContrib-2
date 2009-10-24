using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class DefaultConventionsTester
	{
		private IPropertyViewModelConventions _conventions;

		[SetUp]
		public void Setup()
		{
			this._conventions = new DefaultConventions();
		}


		[Test]
		public void Model_is_invalid_should_return_true()
		{
			Model m = new Model() { String = "foo" };
			var pi = m.GetType().GetProperty("String");
			//arrange
			var helper = InputModelPropertyFactoryTester.CreateHelper(m);
			helper.ViewData.ModelState.AddModelError("String","foo bar");
			//act
			var result = _conventions.ModelIsInvalidConvention(pi, helper);

			//assert
			Assert.IsTrue(result);

		}


		[Test]
		public void Model_property_builder_should_return_a_model_of_datetime()
		{
			Model m = new Model(){timestamp = DateTime.Today};
			var pi = m.GetType().GetProperty("timestamp");
			//arrange

            //act
            var result = _conventions.ModelPropertyBuilder(pi, m.timestamp);

            //assert
            Assert.IsInstanceOf(typeof(PropertyViewModel<DateTime>),result);

            Assert.AreEqual(((PropertyViewModel<DateTime>) result).Value, DateTime.Today);
        }

        [Test]
        public void Model_property_builder_should_return_a_model_of_object()
        {
            Model m = new Model() { String = "foo" };
            var pi = m.GetType().GetProperty("String");
            //arrange

            //act
            var result = _conventions.ModelPropertyBuilder(pi, m.String);

            //assert
            Assert.IsInstanceOf(typeof(PropertyViewModel<object>), result);

            Assert.AreEqual(((PropertyViewModel<object>)result).Value, "foo");
        }


        [Test]
        public void Model_property_builder_should_return_a_model_of_select_items_for_an_enum()
        {
            Model m = new Model() { Enum=Foo.Bar };
            var pi = m.GetType().GetProperty("Enum");
            //arrange

            //act
            var result = _conventions.ModelPropertyBuilder(pi, m.Enum);

            //assert
            Assert.IsInstanceOf(typeof(PropertyViewModel<IEnumerable<SelectListItem>>), result);

            Assert.AreEqual(((PropertyViewModel<IEnumerable<SelectListItem>>)result).Value.Count(), 2);
        }

        [Test]
        public void Partial_Name_should_return_the_property()
        {
            Model m = new Model() { String = "foo" };
            var pi = m.GetType().GetProperty("String");
            //arrange

            //act
            var result = _conventions.PartialNameConvention(pi);

            //assert
            Assert.AreEqual("String", result);
        }

        [Test]
        public void Partial_Name_should_return_the_uihint()
        {
            Model m = new Model() { UiHintProperty = "foo"};
            var pi = m.GetType().GetProperty("UiHintProperty");
            //arrange

            //act
            var result = _conventions.PartialNameConvention(pi);

            //assert
            Assert.AreEqual("theview", result);
        }

        [Test]
        public void Partial_Name_should_return_the_datatype()
        {
            Model m = new Model() { DataType = "foo" };
            var pi = m.GetType().GetProperty("DataType");
            //arrange

            //act
            var result = _conventions.PartialNameConvention(pi);

            //assert
            Assert.AreEqual("EmailAddress", result);
        }

        [Test]
        public void Partial_Name_should_return_the_enum_name()
        {
            Model m = new Model() { Enum = Foo.Bar };
            var pi = m.GetType().GetProperty("Enum");
            //arrange

            //act
            var result = _conventions.PartialNameConvention(pi);

            //assert
            Assert.AreEqual("Enum", result);
        }
        [Test]
        public void Value_from_model_should_return_the_enum_name()
        {
            Model m = new Model() { Enum = Foo.Bar };
            var pi = m.GetType().GetProperty("Enum");
            //arrange

            //act
            var result = _conventions.ValueFromModelPropertyConvention(pi,m);

            //assert
            Assert.AreEqual(Foo.Bar, result);
        }

        [Test]
        public void Example_from_property_should_return_the_example()
        {
            Model m = new Model() { Enum = Foo.Bar };
            var pi = m.GetType().GetProperty("Enum");
            //arrange

            //act
            var result = _conventions.ExampleForPropertyConvention(pi);

            //assert
            Assert.AreEqual("example", result);
        }

        [Test]
        public void Example_from_property_should_return_an_empty_string()
        {
            Model m = new Model() { String = "foo" };
            var pi = m.GetType().GetProperty("String");
            //arrange

            //act
            var result = _conventions.ExampleForPropertyConvention(pi);

            //assert
            Assert.AreEqual("", result);
        }

        [Test]
        public void Label_from_property_should_return_the_example()
        {
            Model m = new Model() { Enum = Foo.Bar };
            var pi = m.GetType().GetProperty("Enum");
            //arrange

            //act
            var result = _conventions.LabelForPropertyConvention(pi);

            //assert
            Assert.AreEqual("label", result);
        }

        [Test]
        public void Label_from_property_should_return_the_property_name()
        {
            Model m = new Model() { String = "foo" };
            var pi = m.GetType().GetProperty("String");
            //arrange

            //act
            var result = _conventions.LabelForPropertyConvention(pi);

            //assert
            Assert.AreEqual("String", result);
        }
    }

}