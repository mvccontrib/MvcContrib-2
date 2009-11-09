using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
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
		public void The_factory_should_handle_an_array_of_complex_objects()
		{
			//arrange+            
			var model = new Model {ChildArray = new[] {new Child(){Name = "1"}, new Child(){Name = "2"}, new Child(), new Child(), new Child(){Name = "5"},}};
			var factory = new ViewModelFactory<Model>(CreateHelper(model),
			                                          MvcContrib.UI.InputBuilder.InputBuilder.Conventions.ToArray(),
													  new DefaultNameConvention(), MvcContrib.UI.InputBuilder.InputBuilder.TypeConventions.ToArray());

			//act
			PropertyViewModel inputModelProperty = factory.Create(m => m.ChildArray);


			//assert
			Assert.AreEqual(inputModelProperty.Type, typeof(Child[]));
			Assert.AreEqual(inputModelProperty.Name, "ChildArray");
			Assert.AreEqual(inputModelProperty.HasExample(), false);
			Assert.AreEqual(inputModelProperty.PropertyIsRequired, false);
			Assert.AreEqual(inputModelProperty.PartialName, "Array");
			Assert.AreEqual(inputModelProperty.Layout, "Array");
			Assert.IsInstanceOf<IEnumerable<TypeViewModel>>(inputModelProperty.Value);
		}

		[Test]
		public void The_factory_should_handle_an_array_of_value_objects()
		{
			//arrange+            
			var model = new Model {StringArray = new[] {"foo", "bar", "wow", "this", "is"}};
			var factory = new ViewModelFactory<Model>(CreateHelper(model),
			                                          MvcContrib.UI.InputBuilder.InputBuilder.Conventions.ToArray(),
													  new DefaultNameConvention(), MvcContrib.UI.InputBuilder.InputBuilder.TypeConventions.ToArray());

			//act
			PropertyViewModel inputModelProperty = factory.Create(m => m.StringArray);


			//assert
			Assert.AreEqual(inputModelProperty.Type, typeof(String[]));
			Assert.AreEqual(inputModelProperty.Name, "StringArray");
			Assert.AreEqual(inputModelProperty.HasExample(), false);
			Assert.AreEqual(inputModelProperty.PropertyIsRequired, false);
			Assert.AreEqual(inputModelProperty.PartialName, "Array");
			Assert.AreEqual(inputModelProperty.Layout, "Array");
		}

		[Test,Ignore("not supported")]
		public void The_factory_should_handle_an_array_indexer()
		{
			//arrange+            
			var model = new Model {StringArray = new string[]{"asdf","fddfdf"}};
			var factory = new ViewModelFactory<Model>(CreateHelper(model),
			                                          MvcContrib.UI.InputBuilder.InputBuilder.Conventions.ToArray(),
													  new DefaultNameConvention(), MvcContrib.UI.InputBuilder.InputBuilder.TypeConventions.ToArray());

			//act
			PropertyViewModel inputModelProperty = factory.Create(m => m.StringArray[0]);


			//assert
			Assert.AreEqual(typeof(String), inputModelProperty.Type);
			Assert.AreEqual(inputModelProperty.Name, "StringArray[0]");
			Assert.AreEqual(inputModelProperty.HasExample(), false);
			Assert.AreEqual(inputModelProperty.PropertyIsRequired, false);
			Assert.AreEqual(inputModelProperty.PartialName, "String");
			Assert.AreEqual(inputModelProperty.Layout, "Field");
			//Assert.AreEqual(inputModelProperty., "Array");
		}

		[Test]
		public void The_factory_should_call_the_conventions()
		{
			//arrange+            
			var model = new Model {StringProp = "foo"};
			var factory = new ViewModelFactory<Model>(CreateHelper(model),
			                                          MvcContrib.UI.InputBuilder.InputBuilder.Conventions.ToArray(),
			                                          new DefaultNameConvention(), null);

			//act
			PropertyViewModel inputModelProperty = factory.Create(m => m.StringProp);


			//assert
			Assert.AreEqual(inputModelProperty.Type, typeof(String));
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
		public string[] StringArray { get; set; }

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

		public Child[] ChildArray { get; set; }
	}

	public class Child
	{
		public string Name { get; set; }
		public Child Sibling { get; set; }
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