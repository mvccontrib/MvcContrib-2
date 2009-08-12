using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.CustomBehaviors;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using HtmlAttribute=MvcContrib.FluentHtml.Html.HtmlAttribute;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class AutoLabelTests
	{
		[Test]
		public void label_renders_label_with_pascal_case_name_split()
		{
			new TextBox("FooBar").AutoLabel().ToString().ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveInnerTextEqual("Foo Bar");
		}

		[Test]
		public void can_render_label_with_full_name()
		{
			new TextBox("Foo.Bar", null, new[] { new AutoLabelSettings(true, null, null) }).AutoLabel()
				.ToString().ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveInnerTextEqual("Foo Bar");
		}

		[Test]
		public void can_render_label_with_format()
		{
			new TextBox("Foo.Bar", null, new[] { new AutoLabelSettings(false, "{0}:", null) })
				.AutoLabel().ToString().ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveInnerTextEqual("Bar:");
		}

		[Test]
		public void can_render_label_with_class()
		{
			new TextBox("Bar", null, new[] { new AutoLabelSettings(false, null, "test-class") })
				.AutoLabel().ToString().ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveAttribute(HtmlAttribute.Class).WithValue("test-class");
		}

		[Test]
		public void auto_label_not_set_if_label_was_explicitly_set_for_the_element()
		{
			new TextBox("Bar", null, new[] { new AutoLabelBehavior()}).Label("Not Bar")
				.ToString().ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveInnerTextEqual("Not Bar");
		}

		[Test]
		public void do_not_render_lable_if_not_qualified()
		{
			var nodes = new TextBox("Foo.Bar", null, new[] { new AutoLabelBehavior(IsNotTextBox, null) })
				.ToString().ShouldRenderHtmlDocument().ChildNodes;
			nodes.ShouldCount(1);
			nodes[0].ShouldBeNamed(HtmlTag.Input);
		}

		[Test]
		public void render_label_after()
		{
			new TextBox("FooBar").AutoLabelAfter()
				.ToString().ShouldRenderHtmlDocument().ChildNodes[1]
				.ShouldBeNamed(HtmlTag.Label);
		}

		[Test]
		public void render_label_for_array_element_removes_array_notations()
		{
			new TextBox("Foo[0].Bar[1]", null, new[] { new AutoLabelSettings(true, null, null) })
				.AutoLabel().ToString().ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Label)
				.ShouldHaveInnerTextEqual("Foo Bar");
		}

        [Test]
        public void auto_label_renders_if_the_element_has_some_other_behaviors_but_not_auto_label_settings()
        {
            new TextBox("Foo[0].Bar[1]", null, new[] { new CustomMaxLengthBehavior() })
                .AutoLabel().ToString().ShouldRenderHtmlDocument().ChildNodes[0]
                .ShouldBeNamed(HtmlTag.Label)
                .ShouldHaveInnerTextEqual("Bar");
        }

		private bool IsTextBox(IElement element)
		{
			return element is TextBox;
		}

		private bool IsNotTextBox(IElement element)
		{
			return !IsTextBox(element);
		}
	}
}