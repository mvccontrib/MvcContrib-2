using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.LegacyGrid;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Collections.Generic;
using MvcContrib.Pagination;
namespace MvcContrib.UnitTests.UI.Html
{
#pragma warning disable 618,612

	[TestFixture]
	public class HtmlGridTester
	{
		private class Person
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public DateTime DateOfBirth { get; set; }
		}

		private MockRepository _mocks;
		private HttpContextBase _context;
		private HtmlHelper _helper;
		private ViewContext _viewContext;
		private List<Person> _people;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicHttpContextBase();
			SetupResult.For(_context.Request.FilePath).Return("Test.mvc");
		    var view = _mocks.DynamicMock<IView>();
			_viewContext = new ViewContext(new ControllerContext(_context, new RouteData(), _mocks.DynamicMock<ControllerBase>()), view, new ViewDataDictionary(), new TempDataDictionary());
			_helper = new HtmlHelper(_viewContext, new ViewPage());
			_people = new List<Person>
			              	{
			              		new Person { Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19)}
			              	};
			AddToViewData("people", _people);
			_mocks.ReplayAll();

		}
		private TextWriter Writer
		{
			get { return _context.Response.Output; }
		}


		private void AddToViewData(string key, Object value)
		{
			_viewContext.ViewData.Add(key, value);
		}

		[Test]
		public void Should_render_empty_table()
		{
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			var grid = new Grid<Person>(null, null, null, Writer, null);
			grid.Render();

			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_empty_table_when_collection_is_empty()
		{
			_people.Clear();
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); });

			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_empty_table_with_custom_message()
		{
			string expected = "<table class=\"grid\"><tr><td>Test</td></tr></table>";
			_helper.Grid<Person>((string)null, new Hash(empty => "Test"), column => { column.For(p => p.Name); column.For(p => p.Id); });
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Custom_html_attrs()
		{
			string expected = "<table class=\"sortable grid\"><tr><td>There is no data available.</td></tr></table>";
			var grid = new Grid<Person>(null, null, new Hash(@class => "sortable grid"), Writer, null);
			grid.Render();

			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void Should_render()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_custom_Header_section()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name).Header(() => Writer.Write("<td>TEST</td>")); column.For(p => p.Id); });
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void Header_should_be_split_pascal_case()
		{
			_helper.Grid<Person>("people", column => column.For(p => p.DateOfBirth).Formatted("{0:dd}"));

			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>19</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}


		[Test]
		public void With_format()
		{
			_helper.Grid<Person>("people", column => column.For(p => p.DateOfBirth).Formatted("{0:ddd}"));

		    var dayString = string.Format("{0:ddd}", _people[0].DateOfBirth);

			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>" + dayString + "</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Complicated_column()
		{
			_helper.Grid<Person>("people", column => column.For(p => p.Id + "-" + p.Name, "Test"));
			string expected = "<table class=\"grid\"><thead><tr><th>Test</th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Column_heading_should_be_empty()
		{
			_helper.Grid<Person>("people", column => column.For(p => p.Id + "-" + p.Name));
			string expected = "<table class=\"grid\"><thead><tr><th></th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Custom_item_section()
		{
			_helper.Grid<Person>("people", column => column.For("Name").Do(s => Writer.Write("<td>Test</td>")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Test</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_anonymous_type()
		{
			AddToViewData("test", new ArrayList { new { Name = "Testing" } });
			_helper.Grid<object>("test", column => column.For("Name"));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Testing</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_cell_condition()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id).CellCondition(p => false); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td></td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_col_condition()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id).ColumnCondition(() => false); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void When_a_custom_renderer_is_specified_then_column_condition_should_still_be_checked()
		{
			_helper.Grid<Person>("people", column => column.For("Custom").Do(x => Writer.Write("<td>Foo</td>")).ColumnCondition(() => false));
			string expected = "<table class=\"grid\"><thead><tr></tr></thead><tr class=\"gridrow\"></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_encode()
		{
			AddToViewData("people2", new List<Person> { new Person { Name = "Jeremy&" } });
			_helper.Grid<Person>("people2", column => column.For(p => p.Name));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy&amp;</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_not_encode()
		{
			AddToViewData("people2", new List<Person> { new Person { Name = "Jeremy&" } });
			_helper.Grid<Person>("people2", column => column.For(p => p.Name).DoNotEncode());
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy&</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_strongly_typed_data()
		{
			_helper.Grid(new List<Person> { new Person { Id = 1 } }, column => column.For(p => p.Id));
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr class=\"gridrow\"><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected)); 
		}

		[Test]
		public void Should_render_with_strongly_typed_data_and_custom_sections() 
		{
			_helper.Grid(new List<Person> { new Person { Id = 1 } }, column => column.For(p => p.Id), sections => sections.RowStart(p => Writer.Write("<tr foo=\"bar\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr foo=\"bar\"><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_strongly_typed_data_and_custom_attributes() 
		{
			_helper.Grid(new List<Person> { new Person { Id = 1 } }, new Hash(style => "width: 100%"), column => column.For(p => p.Id));
			string expected = "<table style=\"width: 100%\" class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr class=\"gridrow\"><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}


		[Test]
		public void Should_render_custom_row_end()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id);  }, sections => sections.RowEnd(person => Writer.Write("</tr>TEST")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr>TEST</table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_custom_row_start()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); }, sections => sections.RowStart(p => Writer.Write("<tr class=\"row\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"row\"><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_pagination_last_and_next()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=2\">last</a></span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_with_pagination_first_and_previous()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(2, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1\">first</a> | <a href=\"Test.mvc?page=1\">prev</a> | next | last</span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_querystring()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			_context.Request.QueryString.Add("a", "b");
			AddToViewData("pagedPeople", _people.AsPagination(2, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1&amp;a=b\">first</a> | <a href=\"Test.mvc?page=1&amp;a=b\">prev</a> | next | last</span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_different_message_if_pagesize_is_1()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 1));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));

		}

		[Test]
		public void Alternating_rows_should_have_correct_css_class()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_helper.Grid<Person>("people", column => column.For(p => p.Name));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr><tr class=\"gridrow_alternate\"><td>Person 2</td></tr><tr class=\"gridrow\"><td>Person 3</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_not_render_pagination_when_datasource_is_empty()
		{
			AddToViewData("foo", new List<Person>().AsPagination(1));
			_helper.Grid<Person>("foo", column => column.For(p => p.Name));
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_localized_pagination()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Visar 1 - 2 av 3 </span><span class='paginationRight'>första | föregående | <a href=\"Test.mvc?page=2\">nästa</a> | <a href=\"Test.mvc?page=2\">sista</a></span></div>";
			_helper.Grid<Person>("pagedPeople", new Hash(paginationFormat => "Visar {0} - {1} av {2} ", first => "första", prev => "föregående", next => "nästa", last => "sista") ,column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_custom_row_start_with_alternate_row()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); }, sections => sections.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}


		[Test]
		public void Should_render_localized_pagination_with_different_message_if_pagesize_is_1()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 1));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Visar 1 av 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			_helper.Grid<Person>("pagedPeople",new Hash(paginationSingleFormat => "Visar {0} av {1} "), column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_custom_page_name()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?my_page=2\">next</a> | <a href=\"Test.mvc?my_page=2\">last</a></span></div>";
			_helper.Grid<Person>("pagedPeople", new Hash(page => "my_page"), column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_header_attributes()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_helper.Grid<Person>("people", column => column.For(p => p.Name).HeaderAttributes(new Hash(style=>"width:100%")), sections => sections.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">")));
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

	}
#pragma warning restore 618,612
}
