using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder;
using MvcContrib.UI.InputBuilder.Attributes;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{

	[TestFixture]
	public class InputModelPropertyFactoryTester
	{
		[Test]
		public void The_factory_should_call_the_conventions()
		{
			//arrange+            
			var model = new Model() { StringProp = "foo" };
			var factory = new ViewModelFactory<Model>(CreateHelper(model), MvcContrib.UI.InputBuilder.InputBuilder.Conventions.ToArray(), new DefaultNameConvention(), null);

			//act
			var inputModelProperty = factory.Create(m=>m.StringProp);

            
			//assert
			Assert.AreEqual(inputModelProperty.Type,typeof(String));
			Assert.AreEqual(inputModelProperty.Name, "StringProp");
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
	}

	public class Model
	{
		[Required]
		[Example("example")]
		public string StringProp { get; set; }

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