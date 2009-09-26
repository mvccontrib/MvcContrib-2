using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.UI.Grid;
using Rhino.Mocks;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using MvcContrib.Routing;

namespace MvcContrib.UnitTests.UI.Grid
{
    [TestFixture]
    public class SortableColumnRenderTester
    {
        private StringWriter _writer;
        private ViewContext _context;
        private IViewEngine _viewEngine;
        private ViewEngineCollection _engines;
        private RenderingContext _renderContext;

        [SetUp]
        public void Setup()
        {
            _writer = new StringWriter();
            _context = new ViewContext();
            _viewEngine = MockRepository.GenerateMock<IViewEngine>();
            _engines = new ViewEngineCollection(new List<IViewEngine> { _viewEngine });
            _context.HttpContext = MvcMockHelpers.DynamicHttpContextBase();
            _renderContext = new RenderingContext(_writer, _context, _engines);
        }

        [Test]
        public void Should_render_sortable_column_link()
        {
            GridColumn<Person> column = GetColumnWithOptions();
            string link = new SortableLinkRenderer<Person>(column as GridColumn<Person>, _renderContext).SortLink();
            link.ShouldEqual("<a href=\"?SortBy=Name&SortOrder=Descending\">Name</a>");
        }

        [Test]
        public void Should_retain_existing_parameters()
        {
            _context.HttpContext.Request.QueryString["Bar"] = "Baz";
            _context.HttpContext.Request.QueryString["Foo"] = "Bar";

            GridColumn<Person> column = GetColumnWithOptions();

            string link = new SortableLinkRenderer<Person>(column as GridColumn<Person>, _renderContext).SortLink();
            link.ShouldEqual("<a href=\"?SortBy=Name&SortOrder=Descending&Bar=Baz&Foo=Bar\">Name</a>");
        }

        [Test]
        public void Should_retain_existing_path()
        {
            ViewContext pathContext = new ViewContext();
            pathContext.HttpContext = MvcMockHelpers.DynamicHttpContextBase("TestPath");
            GridColumn<Person> column = GetColumnWithOptions();
            string link = new SortableLinkRenderer<Person>(column, new RenderingContext(_writer, pathContext, _engines)).SortLink();
            link.ShouldEqual("<a href=\"TestPath?SortBy=Name&SortOrder=Descending\">Name</a>");
        }

        [Test]
        public void Should_render_with_alternate_query_params()
        {
            GridColumn<Person> column = GetColumnWithOptions();
            column.SortOptions.SortByQueryParameterName = "Foo";
            column.SortOptions.SortOrderQueryParameterName = "Bar";
            string link = new SortableLinkRenderer<Person>(column as GridColumn<Person>, _renderContext).SortLink();
            link.ShouldEqual("<a href=\"?Foo=Name&Bar=Descending\">Name</a>");
        }

        private GridColumn<Person> GetColumnWithOptions()
        {
            GridColumn<Person> column = new GridColumn<Person>(p => p.Name, "Name", typeof(string));
            (column as IGridColumn<Person>).Sortable(true);
            column.SortOptions.SortOrder = System.Data.SqlClient.SortOrder.Descending;
            column.SortOptions.SortByQueryParameterName = "SortBy";
            column.SortOptions.SortOrderQueryParameterName = "SortOrder";
            return column;
        }
    }
}
