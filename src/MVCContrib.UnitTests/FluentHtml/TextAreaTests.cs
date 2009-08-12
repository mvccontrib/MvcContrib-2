using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class TextAreaTests
	{
		[Test]
		public void textarea_renders_with_correct_tag_id_and_name()
		{
			var element = new TextArea("foo.Bar").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldBeNamed(HtmlTag.TextArea);

			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
		}

		[Test]
		public void model_textbox_renders_with_correct_tag_id_and_name()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var element = new TextArea(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString()
				.ShouldHaveHtmlNode("Person_FirstName")
				.ShouldBeNamed(HtmlTag.TextArea);

			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Person.FirstName");
		}

		[Test]
		public void textarea_renders_with_correct_inner_text()
		{
			new TextArea("foo.Bar").Value("foo bar").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveInnerTextEqual("foo bar");
		}

		[Test]
		public void textarea_renders_with_formatted_inner_text()
		{
			var item = 1234.5m;
			var expected = string.Format("{0:$#,##0.00}", item);
			new TextArea("foo.Bar").Value(item).Format("$#,##0.00").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveInnerTextEqual(expected);
		}

		[Test]
		public void textarea_renders_with_correct_inner_text_from_enumerable_value_with_formatting()
		{

			var items = new List<decimal> { 1234.5m, 234, 345.666m };
			String expected = string.Empty;
			foreach(var item in items)
			{
				expected += string.Format("{0:$#,##0.00}\r\n", item);
			}
            expected = expected.TrimEnd('\r', '\n');
			new TextArea("foo.Bar").Value(items).Format("$#,##0.00").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveInnerTextEqual(expected);
		}

		[Test]
		public void textarea_rows_renders_rows()
		{
			new TextArea("x").Rows(9).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Rows).WithValue("9");
		}

		[Test]
		public void textarea_columns_renders_columns()
		{
			new TextArea("x").Columns(44).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Cols).WithValue("44");
		}

        [Test]
        public void textarea_value_html_encodes_inner_text()
        {
            var value = "<div>Foo</div>";
            new TextArea("x").Value(value).ToString()
                .ShouldHaveHtmlNode("x")
                .ShouldHaveInnerTextEqual(HttpUtility.HtmlEncode(value));
        }
	}
}
