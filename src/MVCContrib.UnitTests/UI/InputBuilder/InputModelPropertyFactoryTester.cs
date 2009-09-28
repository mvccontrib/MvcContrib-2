using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder;
using MvcContrib.UI.InputBuilder.Attributes;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class InputModelPropertyFactoryTester
    {
        [Test]
        public void The_factory_should_call_the_conventions()
        {
            //arrange+            
            var model = new Model() { String = "foo" };            
            var factory = new InputModelFactory<Model>(CreateHelper(model), new FactoryConventions());

            PropertyInfo property = model.GetType().GetProperty("String");

            //act
            var inputModelProperty = factory.Create(property);
            
            //assert
            Assert.AreEqual(inputModelProperty.Type,typeof(String));
            Assert.AreEqual(inputModelProperty.Name, "name");
            Assert.AreEqual(inputModelProperty.PartialName, "String");
            Assert.AreEqual(inputModelProperty.HasExample(), true);
            Assert.AreEqual(inputModelProperty.Example, "example");
            Assert.AreEqual(inputModelProperty.PropertyIsRequired, true);
        }

        public static HtmlHelper<T> CreateHelper<T>(T model) where T : class
        {
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary();
            context.ViewData.Model = model;
            return new HtmlHelper<T>(context, new ViewDataContainer(context.ViewData));
        }

		private class FactoryConventions : DefaultConventions
		{
			public override string ExampleForPropertyConvention(PropertyInfo propertyInfo)
			{
				return "example";
			}

			public override string LabelForPropertyConvention(PropertyInfo propertyInfo)
			{
				return "label";
			}

			public override bool ModelIsInvalidConvention<T>(PropertyInfo propertyInfo, HtmlHelper<T> htmlHelper)
			{
				return false;
			}

			public override string PartialNameConvention(PropertyInfo propertyInfo)
			{
				return "String";
			}

			public override PropertyViewModel ModelPropertyBuilder(PropertyInfo propertyInfo, object value)
			{
				return new PropertyViewModel<string>();
			}

			public override bool PropertyIsRequiredConvention(PropertyInfo propertyInfo)
			{
				return true;
			}

			public override string PropertyNameConvention(PropertyInfo propertyInfo)
			{
				return "name";
			}

			public override object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model)
			{
				return "value";
			}
		}
    }

    public class Model
    {
        public string String { get; set; }

        [Label("label")]
        [Example("example")]
        public Foo Enum { get; set; }

        [PartialView("theview")]
        public string UiHintProperty { get; set; }

        public DateTime timestamp { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string DataType { get; set; }
    }

    public enum Foo
    {
        Bar,
        OhYeah
    }

    public class ViewDataContainer : IViewDataContainer
    {
        public ViewDataContainer(ViewDataDictionary viewData)
        {
            ViewData = viewData;
        }

        public ViewDataDictionary ViewData { get; set; }
    }

}