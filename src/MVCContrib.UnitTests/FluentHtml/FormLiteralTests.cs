using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class FormLiteralTests
	{
		[Test]
		public void can_render_form_literal()
		{
			var html = new FormLiteral("foo.Bar").Value("foo bar").Value(123).ToString();
			
			var element = html.ShouldRenderHtmlDocument();

			var hidden = element.ShouldHaveChildNode("foo_Bar");
            hidden.ShouldBeNamed(HtmlTag.Input);
			hidden.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Hidden);
			hidden.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
			hidden.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("123");

			var span = element.ShouldHaveChildNode("foo_Bar_Literal");
			span.ShouldBeNamed(HtmlTag.Span);
			span.ShouldHaveInnerTextEqual("123");
		}
	}
}
