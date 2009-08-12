using System.Collections.Generic;
using HtmlAgilityPack;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using HtmlAttribute=MvcContrib.FluentHtml.Html.HtmlAttribute;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class CheckBoxListTests
	{
		[Test]
		public void checkbox_list_renders_in_a_div()
		{
			var html = new CheckBoxList("foo.Bar").ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			element.ShouldBeNamed(HtmlTag.Div).ShouldHaveChildNodesCount(0);
		}

		[Test]
		public void can_generate_checkbox_list_from_model_choices()
		{
			var items = new List<FakeModel> 
			{ 
				new FakeModel { Price = 1, Title = "One" },
				new FakeModel { Price = 2, Title = "Two" },
			};
			var html = new CheckBoxList("foo.Bar").Options(items, x => x.Price, x => x.Title).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");

			VerifyItem("foo.Bar", items[0].Price, items[0].Title, element, 0, false);
			VerifyItem("foo.Bar", items[1].Price, items[1].Title, element, 1, false);
		}

		[Test]
		public void radio_renders_selected_item_as_checked()
		{
			var html = new CheckBoxList("foo.Bar").Options<FakeEnum>()
				.Selected(new[] { FakeEnum.Zero, FakeEnum.Two }).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");

			VerifyItem("foo.Bar", (int)FakeEnum.Zero, FakeEnum.Zero.ToString(), element, 0, true);
			VerifyItem("foo.Bar", (int)FakeEnum.One, FakeEnum.One.ToString(), element, 1, false);
			VerifyItem("foo.Bar", (int)FakeEnum.Two, FakeEnum.Two.ToString(), element, 2, true);
			VerifyItem("foo.Bar", (int)FakeEnum.Three, FakeEnum.Three.ToString(), element, 3, false);
		}

		[Test]
		public void can_generate_checkbox_list_with_formatted_items()
		{
			var items = new List<FakeModel> 
			{ 
				new FakeModel { Price = 1, Title = "One" },
			};
			var html = new CheckBoxList("foo.Bar").Options(items, x => x.Price, x => x.Title)
				.ItemFormat("<div>{0}</div>").ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");

			element.ShouldHaveChildNodesCount(1)[0].ShouldBeNamed(HtmlTag.Div);
		}

		private void VerifyItem(string name, object value, object text, HtmlNode element, int index, bool @checked)
		{
			var input = element.ShouldHaveChildNode(string.Format("{0}_{1}", name.FormatAsHtmlId(), index));
			input.ShouldBeNamed(HtmlTag.Input);
			input.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Checkbox);
			input.ShouldHaveAttribute(HtmlAttribute.Name).WithValue(name);
			input.ShouldHaveAttribute(HtmlAttribute.Value).WithValue(value.ToString());
			if (@checked)
			{
				input.ShouldHaveAttribute(HtmlAttribute.Checked).WithValue(HtmlAttribute.Checked);
			}
			else
			{
				input.ShouldNotHaveAttribute(HtmlAttribute.Checked);
			}

			var label = element.ShouldHaveChildNode(string.Format("{0}_{1}_Label", name.FormatAsHtmlId(), index));
			label.ShouldBeNamed(HtmlTag.Label);
			label.ShouldHaveInnerTextEqual(text.ToString());
		}
	}
}