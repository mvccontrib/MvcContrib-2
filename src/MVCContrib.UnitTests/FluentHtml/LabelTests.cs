using System;
using System.Linq.Expressions;
using System.Web;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class LabelTests
	{
		[Test]
		public void basic_label_renders_with_the_correct_tag()
		{
			new Label().ToString()
				.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label);
		}

		[Test]
		public void lable_renders_with_the_correct_inner_text()
		{
			new Label().Value("foo bar").ToString()
				.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldHaveInnerTextEqual("foo bar");
		}

		[Test]
		public void label_renders_with_inner_text_formatted()
		{
			using (CultureHelper.EnUs())
			{
				const decimal unformatedItem = 1234.5m;
				var expectedFormatedItem = string.Format("{0:$#,##0.00}", unformatedItem);

				new Label().Value(unformatedItem).Format("$#,##0.00").ToString()
					.ShouldRenderHtmlDocument().ChildNodes[0]
					.ShouldHaveInnerTextEqual(expectedFormatedItem);
			}
		}

		[Test]
		public void label_html_encodes_its_value()
		{
			const string valueWithHtml = "<div>Foo</div>";

			new Label().Value(valueWithHtml).ToString()
				.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldHaveInnerTextEqual(HttpUtility.HtmlEncode(valueWithHtml));
		}

		[Test]
		public void label_renders_with_correct_for_tag_when_pointed_to_an_element()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;

			new Label(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString()
				.ShouldHaveHtmlNode("Person_FirstName_DetachedLabel")
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveAttribute(HtmlAttribute.For).WithValue("Person_FirstName");
		}

		[Test]
		public void label_with_specified_id_renders_that_id()
		{
			new Label().Id("some_id").ToString()
				.ShouldHaveHtmlNode("some_id");
		}
	}
}
