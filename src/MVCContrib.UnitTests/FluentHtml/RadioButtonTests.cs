using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class RadioButtonTests
	{
		[Test]
		public void can_get_a_radio_button_with_label_after()
		{
			var html = new RadioButton("foo.Bar").Value(10).LabelAfter("Ten").ToString();
			var radioInput = html.ShouldHaveHtmlNode("foo_Bar_10");
			radioInput.ShouldBeNamed(HtmlTag.Input);
			radioInput.ShouldBeNamed(HtmlTag.Input);
			radioInput.ShouldHaveAttribute(HtmlAttribute.Id).WithValue("foo_Bar_10");
			radioInput.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
			radioInput.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Radio);
			radioInput.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("10");
			radioInput.ShouldNotHaveAttribute(HtmlAttribute.Checked);
			var label = html.ShouldHaveHtmlNode("foo_Bar_10_Label");
			label.ShouldHaveAttribute(HtmlAttribute.For).WithValue("foo_Bar_10");
			label.ShouldHaveInnerTextEqual("Ten");
		}

		[Test]
		public void can_get_a_checked_radio_button()
		{
			var html = new RadioButton("foo.Bar").Value(10).Checked(true).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar_10");
			element.ShouldHaveAttribute(HtmlAttribute.Checked).WithValue(HtmlAttribute.Checked);
		}

		[Test]
		public void can_format_a_radio_button()
		{
			var html = new RadioButton("foo.Bar").Value(10).Format("{0}<br/>").ToString();
			var doc = html.ShouldRenderHtmlDocument();
			var br = doc.ShouldHaveChildNodesCount(2)[1];
			br.ShouldBeNamed("br");
		}
	}
}