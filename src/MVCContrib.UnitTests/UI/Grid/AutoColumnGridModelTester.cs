using System.IO;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using MvcContrib.UI.Grid.Syntax;
using NUnit.Framework;
using System.Linq;
namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class AutoColumnGridModelTester
	{
		private AutoColumnGridModel<Person> _gridModel;

		[SetUp]
		public void Setup()
		{
			_gridModel = new AutoColumnGridModel<Person>(); 
		}

		[Test]
		public void Should_generate_columns()
		{
			_gridModel.Column.Count().ShouldEqual(2);
			_gridModel.Column.First().Name.ShouldEqual("Name");
			_gridModel.Column.Last().Name.ShouldEqual("Id");
		}

		[Test]
		public void Calling_AutoGenerateColumns_should_set_gridmodel()
		{
			IGrid<Person> grid = new Grid<Person>(new Person[0], new StringWriter(), new ViewContext());
			grid.AutoGenerateColumns();

			((Grid<Person>)grid).Model.ShouldBe<AutoColumnGridModel<Person>>();
		}

		private class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}