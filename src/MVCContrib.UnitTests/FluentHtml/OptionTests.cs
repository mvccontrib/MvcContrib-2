using System.Web;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void option_value_html_encodes_value()
        {
            var value = "<div>Foo</div>";
            var html = new Option().Value(value).ToString();
            html.ShouldRenderHtmlDocument().ChildNodes[0]
                .ShouldHaveAttribute(HtmlAttribute.Value).WithValue(HttpUtility.HtmlAttributeEncode(value));
        }

        [Test]
        public void option_text_html_encodes_inner_text()
        {
            var text = "<div>Foo</div>";
            var html = new Option().Text(text).ToString();
            html.ShouldRenderHtmlDocument().ChildNodes[0].ShouldHaveInnerTextEqual(
                HttpUtility.HtmlEncode(text));
        }
    }
}