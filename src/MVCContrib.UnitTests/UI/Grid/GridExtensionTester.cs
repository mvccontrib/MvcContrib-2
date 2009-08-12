using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib.UI.Grid;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridExtensionTester
	{
		private HtmlHelper helper;

		[SetUp]
		public void Setup()
		{
			helper = new HtmlHelper(new ViewContext
			                        	{
			                        		ViewData = new ViewDataDictionary(),
											HttpContext = MvcMockHelpers.DynamicHttpContextBase()
			                        	}, 
										MockRepository.GenerateMock<IViewDataContainer>(), new RouteCollection()
									);
		}

		[Test]
		public void Should_create_grid_with_explict_data()
		{
			var people = new List<Person>();
			var grid = helper.Grid<Person>(people) as Grid<Person>;
			grid.ShouldNotBeNull();
			grid.DataSource.ShouldBeTheSameAs(people);
		}

		[Test]
		public void Should_create_grid_from_viewdata()
		{
			var people = new List<Person>();
			helper.ViewContext.ViewData.Add("people", people);
			var grid = helper.Grid<Person>("people") as Grid<Person>;
			grid.ShouldNotBeNull();
			grid.DataSource.ShouldBeTheSameAs(people);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_if_item_in_viewdata_is_not_IEnumerable_T()
		{
			helper.ViewContext.ViewData.Add("people", new object());
			helper.Grid<Person>("people");
		}
	}
}