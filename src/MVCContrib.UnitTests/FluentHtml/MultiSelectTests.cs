using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class MultiSelectTests
	{
		[Test]
		public void basic_multi_select_renders_with_no_options()
		{
			var element = new MultiSelect("foo.Bar").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveAttributesCount(3)
				.ShouldBeNamed(HtmlTag.Select);
			
			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
			element.ShouldHaveAttribute(HtmlAttribute.Multiple).WithValue(HtmlAttribute.Multiple);
			element.ShouldHaveNoChildNodes();
		}

		[Test]
		public void basic_multiselect_renders_with_options_from_dictionary()
		{
			var options = new Dictionary<int, string> { { 1, "One" }, { 2, "Two" }, { 3, "Three" } };
			var selectedOptions = new List<int> { 1, 3 };

			var html = new MultiSelect("foo.Bar").Options(options).Selected(selectedOptions).ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");

			var optionNodes = element.ShouldHaveChildNodesCount(3);

			optionNodes[0].ShouldBeSelectedOption(1, options[1]);
			optionNodes[1].ShouldBeUnSelectedOption(2, options[2]);
			optionNodes[2].ShouldBeSelectedOption(3, options[3]);
		}

		[Test]
		public void basic_select_renders_select_with_options_from_select_list()
		{
			var items = new List<FakeModel>
			{
				new FakeModel {Id = 1, Title = "One"},
				new FakeModel {Id = 2, Title = "Two"},
				new FakeModel {Id = 3, Title = "Three"}
			};
			var selectedOptions = new List<int> { 1, 3 };

			var selectList = new MultiSelectList(items, "Id", "Title", selectedOptions);

			var html = new MultiSelect("foo.Bar").Options(selectList).ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");

			var optionNodes = element.ShouldHaveChildNodesCount(3);

			optionNodes[0].ShouldBeSelectedOption(items[0].Id, items[0].Title);
			optionNodes[1].ShouldBeUnSelectedOption(items[1].Id, items[1].Title);
			optionNodes[2].ShouldBeSelectedOption(items[2].Id, items[2].Title);
		}
	}
}
