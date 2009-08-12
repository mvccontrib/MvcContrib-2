using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using MvcContrib.UI.Grid.ActionSyntax;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridRendererTester
	{
		private List<Person> _people;
		private GridModel<Person> _model;
		private IViewEngine _viewEngine;
		private ViewEngineCollection _engines;
		private StringWriter _writer;

		[SetUp]
		public void Setup()
		{
			_model = new GridModel<Person>();
			_people = new List<Person> {new Person {Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19)}};
			_viewEngine = MockRepository.GenerateMock<IViewEngine>();
			_engines = new ViewEngineCollection(new List<IViewEngine> { _viewEngine });
			_writer = new StringWriter();
		}

		private IGridColumn<Person> ColumnFor(Expression<Func<Person, object>> expression)
		{
			return _model.Column.For(expression);
		}

		[Test]
		public void Should_render_empty_table()
		{
			string expected = ExpectedEmptyTable("There is no data available.", "grid");
			RenderGrid(null).ShouldEqual(expected);
		}

	    private string ExpectedEmptyTable(string message, string @class)
	    {
	        return "<table class=\""+ @class +"\"><thead><tr><th></th></tr></thead><tbody><tr><td>" + message +"</td></tr></tbody></table>";
	    }

	    [Test]
		public void Should_render_empty_table_when_collection_is_empty()
		{
			_people.Clear();
	        string expected = ExpectedEmptyTable("There is no data available.", "grid");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_empty_table_with_custom_message()
		{
			_people.Clear();
		    string expected = ExpectedEmptyTable("Test", "grid");// "<table class=\"grid\"><tr><td>Test</td></tr></table>";
			_model.Empty("Test");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_html_attrs()
		{
			_people.Clear();
		    string expected = ExpectedEmptyTable("There is no data available.", "sortable grid");// "<table class=\"sortable grid\"><tr><td>There is no data available.</td></tr></table>";
			_model.Attributes(@class => "sortable grid");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render()
		{
			ColumnFor(x => x.Name);
			ColumnFor(x => x.Id);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_with_custom_Header()
		{
			ColumnFor(x => x.Name).Header("<td>TEST</td>");
			ColumnFor(x => x.Id);
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Header_should_be_split_pascal_case()
		{
			ColumnFor(x => x.DateOfBirth).Format("{0:dd}");
			string expected =
				"<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tbody><tr class=\"gridrow\"><td>19</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void With_format()
		{
			ColumnFor(x => x.DateOfBirth).Format("{0:ddd}");
			var dayString = string.Format("{0:ddd}", _people[0].DateOfBirth);
			dayString = HttpUtility.HtmlEncode(dayString);
			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tbody><tr class=\"gridrow\"><td>" +
			                  dayString + "</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Complicated_column()
		{
			ColumnFor(x => x.Id + "-" + x.Name).Named("Test");
			string expected =
				"<table class=\"grid\"><thead><tr><th>Test</th></tr></thead><tbody><tr class=\"gridrow\"><td>1-Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Column_heading_should_be_empty()
		{
			ColumnFor(x => x.Id + "-" + x.Name);
			string expected =
				"<table class=\"grid\"><thead><tr><th></th></tr></thead><tbody><tr class=\"gridrow\"><td>1-Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Column_should_be_rendered_using_custom_partial()
		{
			SetupViewEngine("Foo", (v, w) => {
				var model = ((Person)v.ViewData.Model);
				w.Write("<td>" + model.Name + "_TEST</td>");
			});

			ColumnFor(x => x.Name).Partial("Foo");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy_TEST</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_column_should_use_partial_with_same_name_as_column()
		{
			SetupViewEngine("Name", "<td>Foo</td>");
			_model.Column.For("Name");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Foo</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_column_with_custom_partial()
		{
			SetupViewEngine("Foo", "<td>Foo</td>");
			_model.Column.For("Name").Partial("Foo");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Foo</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void With_cell_condition()
		{
			ColumnFor(x => x.Name);
			ColumnFor(x => x.Id).CellCondition(x => false);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td><td></td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void With_col_condition()
		{
			ColumnFor(x => x.Name);
			ColumnFor(x => x.Id).Visible(false);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_row_start()
		{
			ColumnFor(x => x.Id);
			_model.Sections.RowStart(x => "<tr foo=\"bar\">");
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tbody><tr foo=\"bar\"><td>1</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void Should_render_custom_row_end()
		{
			ColumnFor(x => x.Id);
			_model.Sections.RowEnd(x => "</tr>TEST");
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tbody><tr class=\"gridrow\"><td>1</td></tr>TEST</tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Alternating_rows_should_have_correct_css_class()
		{
			_people.Add(new Person {Name = "Person 2"});
			_people.Add(new Person {Name = "Person 3"});
			ColumnFor(x => x.Name);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td></tr><tr class=\"gridrow_alternate\"><td>Person 2</td></tr><tr class=\"gridrow\"><td>Person 3</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_row_start_with_alternate_row()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			ColumnFor(x => x.Name);
			_model.Sections.RowStart(row => "<tr class=\"row " + (row.IsAlternate ? "gridrow_alternate" : "gridrow") + "\">");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_row_start_with_action()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			ColumnFor(x => x.Name);
			_model.Sections.RowStart(c  =>
			{
				_writer.Write("<tr class=\"gridrow\">");
			});

			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td></tr><tr class=\"gridrow\"><td>Person 2</td></tr><tr class=\"gridrow\"><td>Person 3</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void Should_render_custom_row_start_with_action_alternate()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			ColumnFor(x => x.Name);
			_model.Sections.RowStart((c, vd) =>
			{
				_writer.Write("<tr class=\"row " + (vd.IsAlternate ? "gridrow_alternate" : "gridrow") + "\">");
			});

			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_header_attributes()
		{
			ColumnFor(x => x.Name).HeaderAttributes(style => "width:100%");
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}
		
		[Test]
		public void Should_render_header_attributes_when_rendering_custom_row_start()
		{
			ColumnFor(x => x.Name).HeaderAttributes(style => "width:100%");
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_model.Sections.RowStart(row => "<tr class=\"row " + (row.IsAlternate ? "gridrow_alternate" : "gridrow") + "\">");
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tbody><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void Custom_item_section()
		{
			ColumnFor(x => x.Name).Action(s => _writer.Write("<td>Test</td>"));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Test</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void Should_render_with_custom_header_section()
		{
			ColumnFor(p => p.Name).HeaderAction(() => _writer.Write("<td>TEST</td>"));
			ColumnFor(p => p.Id);
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_attributes_in_table_cell()
		{
			ColumnFor(x => x.Name).Attributes(foo => "bar");
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td foo=\"bar\">Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_attributes_when_Attributes_called_multiple_times()
		{
			ColumnFor(x => x.Name).Attributes(foo => "bar").Attributes(baz => "blah");
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td foo=\"bar\" baz=\"blah\">Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_attributes_in_table_cell_with_logic()
		{
			_people.Add(new Person() { Name = "foo"});
			ColumnFor(x => x.Name).Attributes(row => new Hash(foo => row.IsAlternate ? "bar" : "baz" ));
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td foo=\"baz\">Jeremy</td></tr><tr class=\"gridrow_alternate\"><td foo=\"bar\">foo</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_attributes_for_row()
		{
			ColumnFor(x => x.Name);
			_model.Sections.RowAttributes(x => new Hash(foo => "bar"));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr foo=\"bar\" class=\"gridrow\"><td>Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_css_class_for_row()
		{
			ColumnFor(x => x.Name);
			_model.Sections.RowAttributes(x => new Hash(@class => "foo"));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tbody><tr class=\"foo\"><td>Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_attributes_for_header_row()
		{
			ColumnFor(x => x.Name);
			_model.Sections.HeaderRowAttributes(new Hash(foo => "bar"));
			string expected = "<table class=\"grid\"><thead><tr foo=\"bar\"><th>Name</th></tr></thead><tbody><tr class=\"gridrow\"><td>Jeremy</td></tr></tbody></table>";
			RenderGrid().ShouldEqual(expected);
		}

		private string RenderGrid()
		{
			return RenderGrid(_people);
		}
		

		private string RenderGrid(IEnumerable<Person> dataSource)
		{
			var renderer = new HtmlTableGridRenderer<Person>(_engines);

			var viewContext = MockRepository.GenerateStub<ViewContext>();
			viewContext.View = MockRepository.GenerateStub<IView>();
			viewContext.TempData = new TempDataDictionary();
			var response = MockRepository.GenerateStub<HttpResponseBase>();
			var context = MockRepository.GenerateStub<HttpContextBase>();
			viewContext.HttpContext = context;
			context.Stub(p =>p.Response).Return(response);
			response.Stub(p => p.Output).Return(_writer);

			renderer.Render(_model, dataSource, _writer, viewContext);
            
			return _writer.ToString();
		}

		private void SetupViewEngine(string viewName, string viewContents)
		{
			SetupViewEngine(viewName, (v, w) => w.Write(viewContents));
		}

		private void SetupViewEngine(string viewName, Action<ViewContext, TextWriter> action)
		{
			var view = MockRepository.GenerateMock<IView>();
			_viewEngine.Expect(x => x.FindPartialView(Arg<ControllerContext>.Is.Anything, Arg<string>.Is.Equal(viewName), Arg<bool>.Is.Anything)).Return(new ViewEngineResult(view, _viewEngine)).Repeat.Any();

			view.Expect(x => x.Render(null, null)).IgnoreArguments()
				.Do(action).Repeat.Any();
		}

		public static RenderingContext FakeRenderingContext() 
		{
			var engine = MockRepository.GenerateStub<IViewEngine>();
			engine.Stub(x => x.FindPartialView(null, null, true)).IgnoreArguments().Return(new ViewEngineResult(MockRepository.GenerateStub<IView>(), engine)).Repeat.Any();

			var context = new RenderingContext(
				new StringWriter(),
				new ViewContext() { View = MockRepository.GenerateStub<IView>(), TempData = new TempDataDictionary() },
				new ViewEngineCollection(new List<IViewEngine>() { engine }));

			return context;
		}
	}
}
