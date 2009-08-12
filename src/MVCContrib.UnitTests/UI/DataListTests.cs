using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcContrib.UI;
using MvcContrib.UI.DataList;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.DataList
{
    [TestFixture]
    public class DataListTests
    {
        IList<string> _datasource;
        private HttpContextBase _context;

        [SetUp]
        public void Setup()
        {
            _datasource = new List<string> { "test1", "test2" };
            _context = MvcMockHelpers.DynamicHttpContextBase();
        }

        private TextWriter Writer
        {
            get { return _context.Response.Output; }
        }

        [Test]
        public void Should_render_blank_table_when_DataSource_has_no_items()
        {
            new DataList<string>(new List<string>(), Writer)
                .CellTemplate(x => { Writer.Write(x.ToUpper()); }).ToString();

            Assert.AreEqual("<table><tr><td></td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Should_render_blank_table_when_DataSource_has_no_items_and_RepeatColumns_are_set()
        {
            new DataList<string>(new List<string>(), Writer)
                .NumberOfColumns(3).RepeatHorizontally()
                .CellTemplate(x => { Writer.Write(x.ToUpper()); }).ToString();

            Assert.AreEqual("<table><tr><td></td></tr></table>", Writer.ToString());
        }

        [Test]
        public void ByDefault_RepeatDirection_is_Vertical()
        {
            var dl = new DataList<string>(_datasource, Writer);

            Assert.AreEqual(RepeatDirection.Vertical, dl.RepeatDirection);
        }

        [Test]
        public void ByDefault_RepeatColumns_is_0()
        {
            var dl = new DataList<string>(_datasource, Writer);

            Assert.AreEqual(0, dl.RepeatColumns);
        }

        [Test]
        public void When_RepeatColumns_is_0_it_should_render_all_items_in_1_column()
        {
            new DataList<string>(_datasource, Writer)
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>test1</td></tr><tr><td>test2</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Can_set_RepeatDirection_Vertical_fluently()
        {
            var dl = new DataList<string>(_datasource, Writer)
                .RepeatVertically();

            Assert.AreEqual(RepeatDirection.Vertical, dl.RepeatDirection);
        }

        [Test]
        public void Can_set_RepeatDirection_Horizontal_fluently()
        {
            var dl = new DataList<string>(_datasource, Writer)
                .RepeatHorizontally();

            Assert.AreEqual(RepeatDirection.Horizontal, dl.RepeatDirection);
        }

        [Test]
        public void Dictionary_on_table_should_sticks()
        {
            new DataList<string>(_datasource, Writer, new Hash(id => "foo", @class => "bar"))
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table id=\"foo\" class=\"bar\"><tr><td>test1</td></tr><tr><td>test2</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Dictionary_on_table_should_sticks_with_empty_DataSource()
        {
            new DataList<string>(new List<string>(), Writer, new Hash(id => "foo", @class => "bar"))
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table id=\"foo\" class=\"bar\"><tr><td></td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Can_render_content_horizontally()
        {
            new DataList<string>(_datasource, Writer)
                .NumberOfColumns(3).RepeatHorizontally()
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>test1</td><td>test2</td><td></td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Can_render_content_vertically()
        {
            new DataList<string>(
                new List<string> { "1", "2", "3", "4", "5", "6" }, Writer)
                .NumberOfColumns(3).RepeatVertically()
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>1</td><td>3</td><td>5</td></tr><tr><td>2</td><td>4</td><td>6</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Should_set_blank_items_with_NoItemTemplate_when_extra_columns()
        {
            new DataList<string>(_datasource, Writer)
                .NumberOfColumns(3)
                .NoItemTemplate(() => { Writer.Write("No Data."); })
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>test1</td><td>test2</td><td>No Data.</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void When_DataSource_is_empty_and_EmptyDateSourceTemplate_is_not_set_should_render_blank_cell()
        {
            new DataList<string>(new List<string>(), Writer).ToString();

            Assert.AreEqual("<table><tr><td></td></tr></table>", Writer.ToString());
        }

        [Test]
        public void When_DataSource_is_empty_then_EmptyDateSourceTemplate_should_be_rendered_in_first_cell()
        {
            new DataList<string>(new List<string>(), Writer)
                .EmptyDateSourceTemplate(() => { Writer.Write("There is no data available."); }).ToString();

            Assert.AreEqual("<table><tr><td>There is no data available.</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void When_setting_CellAttributes_the_sttributes_it_Sticks()
        {
            new DataList<string>(_datasource, Writer)
                .CellTemplate(x => { Writer.Write(x.ToUpper()); })
                .CellAttributes(id => "foo", @class => "bar").ToString();

            Assert.AreEqual("<table><tr><td id=\"foo\" class=\"bar\">TEST1</td></tr><tr><td id=\"foo\" class=\"bar\">TEST2</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Should_only_render_items_that_match_CellCondition()
        {
            new DataList<string>(_datasource, Writer)
                .CellTemplate(x => { Writer.Write(x); }).CellCondition(x => x == "test1").ToString();

            Assert.AreEqual("<table><tr><td>test1</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Large_lists_render_correctly_Horizontally()
        {
            new DataList<string>(
                new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" }, Writer)
                .NumberOfColumns(3).RepeatHorizontally()
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>1</td><td>2</td><td>3</td></tr><tr><td>4</td><td>5</td><td>6</td></tr><tr><td>7</td><td>8</td><td>9</td></tr></table>", Writer.ToString());
        }
        [Test]
        public void Large_lists_render_correctly_with_RepeatColumns_not_set()
        {
            new DataList<string>(
                new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" }, Writer)
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>1</td></tr><tr><td>2</td></tr><tr><td>3</td></tr><tr><td>4</td></tr><tr><td>5</td></tr><tr><td>6</td></tr><tr><td>7</td></tr><tr><td>8</td></tr><tr><td>9</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Large_lists_render_correctly_Vertically()
        {
            new DataList<string>(
                new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" }, Writer)
                .NumberOfColumns(3).RepeatVertically()
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>1</td><td>4</td><td>7</td></tr><tr><td>2</td><td>5</td><td>8</td></tr><tr><td>3</td><td>6</td><td>9</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Large_lists_render_correctly_Vertically_with_non_even_columns()
        {
            new DataList<string>(
                new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11" }, Writer)
                .NumberOfColumns(4).RepeatVertically()
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>1</td><td>4</td><td>7</td><td>10</td></tr><tr><td>2</td><td>5</td><td>8</td><td>11</td></tr><tr><td>3</td><td>6</td><td>9</td><td></td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Can_set_the_amount_of_columns_fluently()
        {
            new DataList<string>(_datasource, Writer)
                .NumberOfColumns(2)
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>test1</td><td>test2</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void Can_set_direction_without_setting_columns_fluently()
        {
            new DataList<string>(_datasource, Writer)
                .RepeatVertically()
                .CellTemplate(x => { Writer.Write(x); }).ToString();

            Assert.AreEqual("<table><tr><td>test1</td></tr><tr><td>test2</td></tr></table>", Writer.ToString());
        }

        [Test]
        public void When_calling_AmoutOfColumnsIs_amount_should_set_RepeatColumns()
        {
            var dl = new DataList<string>(_datasource, Writer)
                .NumberOfColumns(5);

            Assert.AreEqual(5, dl.RepeatColumns);
        }

        //This isn't part of the tests and can be deleted
        private void SampleUsage()
        {
            HtmlHelper helper = new HtmlHelper(null, null);
            helper.DataList(_datasource)
                .NumberOfColumns(3).RepeatHorizontally()
                .CellTemplate(x => { Writer.Write(x.ToLower()); })
                .CellCondition(x => x == "test1")
                .EmptyDateSourceTemplate(() => { Writer.Write("There is no data available."); })
                .NoItemTemplate(() => { Writer.Write("No Data."); });


            new DataList<string>(_datasource, Writer)
                 .NumberOfColumns(3).RepeatHorizontally()
                 .CellTemplate(x => { Writer.Write(x.ToLower()); })
                 .CellCondition(x => x == "test1")
                 .EmptyDateSourceTemplate(() => { Writer.Write("There is no data available."); })
                 .NoItemTemplate(() => { Writer.Write("No Data."); }).ToString();
        }

    }
}