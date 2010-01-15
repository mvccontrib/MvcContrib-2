using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using MvcContrib.UI.Grid.Syntax;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class AutoColumnGridModelTester
	{
		private DataAnnotationsModelMetadataProvider _provider;

		[SetUp]
		public void Setup()
		{
			_provider = new DataAnnotationsModelMetadataProvider();
		}

		[Test]
		public void Should_generate_columns()
		{
			var model = new AutoColumnGridModel<Person>(_provider);

			model.Column.Count().ShouldEqual(2);
			model.Column.First().Name.ShouldEqual("Name");
			model.Column.Last().Name.ShouldEqual("Id");
		}

		[Test]
		public void Calling_AutoGenerateColumns_should_set_gridmodel()
		{
			IGrid<Person> grid = new Grid<Person>(new Person[0], new StringWriter(), new ViewContext());
			grid.AutoGenerateColumns();

			((Grid<Person>)grid).Model.ShouldBe<AutoColumnGridModel<Person>>();
		}

		[Test]
		public void Does_not_scaffold_property()
		{
			var model = new AutoColumnGridModel<ScaffoldPerson>(_provider);
			model.Column.Count().ShouldEqual(1);
			model.Column.Single().Name.ShouldEqual("Name");
		}

		[Test]
		public void Uses_custom_displayname()
		{
			var model = new AutoColumnGridModel<DisplayNamePerson>(_provider);
			model.Column.Single().DisplayName.ShouldEqual("Foo");
		}

		[Test]
		public void Uses_custom_displayformat()
		{
			var model = new AutoColumnGridModel<DisplayFormatPerson>(_provider);
			var date = new DateTime(2010, 1, 15);
			var person = new DisplayFormatPerson
			{
				DateOfBirth = date
			};

			model.Column.Single().GetValue(person).ShouldEqual(date.ToString("d"));
		}

		private class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}

		private class ScaffoldPerson
		{
			[ScaffoldColumn(false)]
			public int Id { get; set; }

			public string Name { get; set; }
		}

		private class DisplayNamePerson
		{
			[DisplayName("Foo")]
			public string Name { get; set; }
		}

		private class DisplayFormatPerson
		{
			[DisplayFormat(DataFormatString = "{0:d}")]
			public DateTime DateOfBirth { get; set; }
		}
	}
}