using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
    [TestFixture]
    public class ButtonTests
    {
        [Test]
        public void button_renders_with_corect_tag_and_type()
        {
            new Button("x").ToString()
                .ShouldHaveHtmlNode("x")
                .ShouldBeNamed(HtmlTag.Input)
                .ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Button);
        }
    }
}
