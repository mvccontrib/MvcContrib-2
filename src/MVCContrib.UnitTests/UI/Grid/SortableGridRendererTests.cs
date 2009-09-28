using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.UI.Grid;
using System.Web.Mvc;
using System.IO;
using Rhino.Mocks;
using System.Web;

namespace MvcContrib.UnitTests.UI.Grid
{
    [TestFixture]
    public class SortableGridRendererTests
    {
        private ComparableSortList<Person> _people;
        private GridModel<Person> _model;
        private IViewEngine _viewEngine;
        private ViewEngineCollection _engines;
        private StringWriter _writer;
        private ViewContext _context;

        [SetUp]
        public void Setup()
        {
            _model = new GridModel<Person>();
            _people = new ComparableSortList<Person>(new List<Person> { new Person { Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19) } });
            _viewEngine = MockRepository.GenerateMock<IViewEngine>();
            _engines = new ViewEngineCollection(new List<IViewEngine> { _viewEngine });
            _writer = new StringWriter();
            _context = new ViewContext();
            _context.HttpContext = MvcMockHelpers.DynamicHttpContextBase();
            var response = MockRepository.GenerateStub<HttpResponseBase>();
            _context.HttpContext.Stub(p => p.Response).Return(response);
            response.Stub(p => p.Output).Return(_writer);

        }

        [Test]
        public void Should_set_descending_sortorder_on_default_column()
        {
            _model.Column.For(p => p.Name).Sortable(true);
            RenderGrid(_context);
            GridColumn<Person> column = ((IGridModel<Person>)_model).Columns.FirstOrDefault();
            column.SortOptions.SortOrder.ShouldEqual(System.Data.SqlClient.SortOrder.Descending);
        }

        [Test]
        public void Should_set_descending_sortorder_on_current_column()
        {
            ViewContext context = new ViewContext();
            context.HttpContext = MvcMockHelpers.DynamicHttpContextBase();
            context.HttpContext.Request.QueryString["SortBy"] = "DateOfBirth";
            context.HttpContext.Request.QueryString["SortOrder"] = "Ascending";
            _model = new GridModel<Person>();
            _model.Column.For(x => x.Name).Sortable(true);
            _model.Column.For(x => x.DateOfBirth).Sortable(false);
            RenderGrid(context);
            GridColumn<Person> testColumn = ((IGridModel<Person>)_model).Columns.LastOrDefault();
            testColumn.SortOptions.SortOrder.ShouldEqual(System.Data.SqlClient.SortOrder.Descending);
        }

        [Test]
        public void Should_set_datasource_sort_options_from_request()
        {
            ViewContext context = new ViewContext();
            context.HttpContext = MvcMockHelpers.DynamicHttpContextBase();
            context.HttpContext.Request.QueryString["SortBy"] = "Name";
            context.HttpContext.Request.QueryString["SortOrder"] = "Ascending";
            _model.Column.For(x => x.Name).Sortable(true);
            RenderGrid(context);
            _people.SortBy.ShouldEqual("Name");
            _people.SortOrder.ShouldEqual(System.Data.SqlClient.SortOrder.Ascending);
        }

        [Test]
        public void Should_set_alternate_query_params()
        {
            _model.Column.For(p => p.Name).Sortable(true);
            RenderGrid(_context, "Foo", "Bar");
            GridColumn<Person> column = ((IGridModel<Person>)_model).Columns.FirstOrDefault();
            column.SortOptions.SortByQueryParameterName.ShouldEqual("Foo");
            column.SortOptions.SortOrderQueryParameterName.ShouldEqual("Bar");
        }

        private string RenderGrid(ViewContext viewContext, string sortParamName, string orderParamName)
        {
            var renderer = new SortableHtmlTableGridRenderer<Person>(_engines);
            if (String.IsNullOrEmpty(sortParamName) == false)
                renderer.ColumnQueryStringName = sortParamName;
            if(String.IsNullOrEmpty(orderParamName) == false)
                renderer.OrderQueryStringName = orderParamName;
            viewContext.View = MockRepository.GenerateStub<IView>();
            viewContext.TempData = new TempDataDictionary();
            renderer.Render(_model, _people, _writer, viewContext);
            return _writer.ToString();
        }

        private string RenderGrid(ViewContext viewContext)
        {
            return RenderGrid(viewContext, null, null);
        }
    }
}
