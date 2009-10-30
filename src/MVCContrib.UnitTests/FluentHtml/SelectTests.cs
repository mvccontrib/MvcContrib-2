using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using HtmlAttribute=MvcContrib.FluentHtml.Html.HtmlAttribute;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class SelectTests
	{
		[Test]
		public void basic_select_renders_with_no_options()
		{
			var element = new Select("foo.Bar").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveAttributesCount(2)
				.ShouldBeNamed(HtmlTag.Select);
			
			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
			element.ShouldHaveNoChildNodes();
		}

		[Test]
		public void basic_select_renders_with_options_from_dictionary()
		{
			var options = new Dictionary<int, string> { { 1, "One" }, { 2, "Two" } };

			var html = new Select("foo.Bar").Selected(1).Options(options).ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");

			var optionNodes = element.ShouldHaveChildNodesCount(2);

			optionNodes[0].ShouldBeSelectedOption(1, "One");
			optionNodes[1].ShouldBeUnSelectedOption(2, "Two");
		}

		[Test]
		public void can_render_options_from_enumerable_of_simple_objects()
		{
			var optionNodes = new Select("foo.Bar").Options(new[] { 1, 2 }).ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveChildNodesCount(2);

			optionNodes[0].ShouldBeUnSelectedOption("1", "1");
			optionNodes[1].ShouldBeUnSelectedOption("2", "2");
		}

		[Test]
		public void basic_select_renders_select_with_options_from_select_list()
		{
			var items = new List<FakeModel>
			{
				new FakeModel {Id = 1, Title = "One"},
				new FakeModel {Id = 2, Title = "Two"}
			};
			var selectList = new SelectList(items, "Id", "Title", items[0].Id);

			var html = new Select("foo.Bar").Options(selectList).ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var optionNodes = element.ShouldHaveChildNodesCount(2);
			optionNodes[0].ShouldBeSelectedOption(items[0].Id, items[0].Title);
			optionNodes[1].ShouldBeUnSelectedOption(items[1].Id, items[1].Title);
		}

		[Test]
		public void basic_select_renders_select_size()
		{
			new Select("foo.Bar").Size(22).ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveAttribute(HtmlAttribute.Size).WithValue("22");
		}

		[Test]
		public void select_with_options_for_enum_renders_enum_values_as_options()
		{
			var html = new Select("foo.Bar").Options<FakeEnum>().Selected(FakeEnum.Two).ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var optionNodes = element.ShouldHaveChildNodesCount(4);
			optionNodes[0].ShouldBeUnSelectedOption((int)FakeEnum.Zero, FakeEnum.Zero);
			optionNodes[1].ShouldBeUnSelectedOption((int)FakeEnum.One, FakeEnum.One);
			optionNodes[2].ShouldBeSelectedOption((int)FakeEnum.Two, FakeEnum.Two);
			optionNodes[3].ShouldBeUnSelectedOption((int)FakeEnum.Three, FakeEnum.Three);
		}

		[Test]
		public void select_with_options_for_enum_renders_null_first_option()
		{
			var html = new Select("foo.Bar").Options<FakeEnum>().FirstOption("-Choose-").ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var optionNodes = element.ShouldHaveChildNodesCount(5);
			optionNodes[0].ShouldBeUnSelectedOption(string.Empty, "-Choose-");
		}

		[Test]
		public void basic_select_can_select_null_valued_options()
		{
			var items = new List<FakeModel>
			{
				new FakeModel {Price = null, Title = "One"},
				new FakeModel {Price = 2, Title = "Two"}
			};

            var optionNodes = new Select("foo.Bar").Options(items, "Price", "Title").Selected(items[0].Price).ToString()
                .ShouldHaveHtmlNode("foo_Bar")
                .ShouldHaveChildNodesCount(2);

			optionNodes[0].ShouldBeSelectedOption(items[0].Price, items[0].Title);
			optionNodes[1].ShouldBeUnSelectedOption(items[1].Price, items[1].Title);
		}

        [Test]
        public void basic_select_can_set_selected_value_before_options()
        {
            var items = new List<FakeModel>
			{
				new FakeModel {Price = 1, Title = "One"},
				new FakeModel {Price = 2, Title = "Two"}
			};

            var optionNodes = new Select("foo.Bar").Options(items, "Price", "Title").Selected(items[1].Price).ToString()
                .ShouldHaveHtmlNode("foo_Bar")
                .ShouldHaveChildNodesCount(2);

            optionNodes[0].ShouldBeUnSelectedOption(items[0].Price, items[0].Title);
            optionNodes[1].ShouldBeSelectedOption(items[1].Price, items[1].Title);
        }

		[Test]
		public void select_option_null_renders_with_no_options()
		{
			new Select("foo.Bar").Options(null).ToString()
				.ShouldHaveHtmlNode("foo_Bar").ShouldHaveChildNodesCount(0);
		}

		[Test]
		public void select_option_with_no_items_renders_with_no_options()
		{
			new Select("foo.Bar").Options(new Dictionary<int, string>()).ToString()
				.ShouldHaveHtmlNode("foo_Bar").ShouldHaveChildNodesCount(0);
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void select_options_with_wrong_data_value_field_throws_on_tostring()
		{
			var items = new List<FakeModel> { new FakeModel {Price = null, Title = "One"} };
			new Select("x").Options(items, "Wrong", "Title").ToString();
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void select_options_with_wrong_data_text_field_throws_on_tostring()
		{
			var items = new List<FakeModel> { new FakeModel { Price = null, Title = "One" } };
			new Select("x").Options(items, "Price", "Wrong").ToString();
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void select_options_with_generic_param_not_enum_throws()
		{
			new Select("x").Options<int>().ToString();
		}

		[Test]
		public void select_option_of_enumerable_select_list_item_renders_options()
		{
			var items = new List<SelectListItem>
			{
				new SelectListItem {Value = "1", Text = "One", Selected = false},
				new SelectListItem {Value = "2", Text = "Two", Selected = true},
				new SelectListItem {Value = "3", Text = "Three", Selected = true}
			};
			var html = new Select("foo.Bar").Options(items).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var optionNodes = element.ShouldHaveChildNodesCount(3);
			optionNodes[0].ShouldBeUnSelectedOption(items[0].Value, items[0].Text);
			optionNodes[1].ShouldBeSelectedOption(items[1].Value, items[1].Text);
			optionNodes[2].ShouldBeSelectedOption(items[2].Value, items[2].Text);
		}

		[Test]
		public void select_with_lambda_selector_for_options_should_render()
		{
			var items = new List<FakeModel> { new FakeModel {Price = 1, Title = "One"} };
            var options = new Select("x").Options(items, x => x.Price, x => x.Title).ToString()
                .ShouldHaveHtmlNode("x")
                .ShouldHaveChildNodesCount(1);
			options[0].ShouldBeUnSelectedOption("1", "One");
		}

        [Test]
        public void select_with_lambda_selector_can_called_selected_before_options()
        {
            var items = new List<FakeModel>
            {
                new FakeModel { Price = 1, Title = "One" },
                new FakeModel { Price = 2, Title = "Two" }
            };
            var options = new Select("x").Selected(2).Options(items, x => x.Price, x => x.Title).ToString()
                .ShouldHaveHtmlNode("x")
                .ShouldHaveChildNodesCount(2);
            options[0].ShouldBeUnSelectedOption("1", "One");
            options[1].ShouldBeSelectedOption("2", "Two");
        }

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void select_options_with_null_text_field_selector_should_throw()
		{
			new Select("x").Options(new List<FakeModel>(), null, x => x.Title);
		}
			
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void select_options_with_null_value_field_selector_should_throw()
		{
			new Select("x").Options(new List<FakeModel>(), x => x.Price, null);	
		}

        [Test]
        public void select_options_with_simple_enumeration_of_objects_can_have_selected_called_first()
        {
            var optionNodes = new Select("foo").Selected(2).Options(new[] { 1, 2 }).ToString()
                .ShouldHaveHtmlNode("foo")
                .ShouldHaveChildNodesCount(2);

            optionNodes[0].ShouldBeUnSelectedOption("1", "1");
            optionNodes[1].ShouldBeSelectedOption("2", "2");
        }

		[Test]
		public void select_options_with_simple_enumeration_of_objects_can_have_a_first_option_text_specified()
		{
			var optionNodes = new Select("foo").Options(new[] { 1, 2 }).FirstOption("-Choose-").ToString()
				.ShouldHaveHtmlNode("foo")
				.ShouldHaveChildNodesCount(3);

			optionNodes[0].ShouldBeUnSelectedOption("", "-Choose-");
			optionNodes[1].ShouldBeUnSelectedOption("1", "1");
			optionNodes[2].ShouldBeUnSelectedOption("2", "2");
		}

        [Test]
        public void select_options_with_simple_enumeration_of_objects_can_have_a_first_option_specified()
        {
            var optionNodes = new Select("foo").Options(new[] { 1, 2 }).FirstOption(new Option().Text("No Relation").Value("-1")).ToString()
                .ShouldHaveHtmlNode("foo")
                .ShouldHaveChildNodesCount(3);
            optionNodes[0].ShouldBeUnSelectedOption("-1", "No Relation");
            optionNodes[1].ShouldBeUnSelectedOption("1", "1");
            optionNodes[2].ShouldBeUnSelectedOption("2", "2");
        }

		[Test]
		public void select_options_not_told_to_hide_the_first_option_should_emit_the_first_option_text()
		{
			var optionNodes = new Select("foo").Options(new[] {1, 2}).FirstOption("-Choose-").HideFirstOptionWhen(false).ToString()
				.ShouldHaveHtmlNode("foo")
				.ShouldHaveChildNodesCount(3);

			optionNodes[0].ShouldBeUnSelectedOption("", "-Choose-");
			optionNodes[1].ShouldBeUnSelectedOption("1", "1");
			optionNodes[2].ShouldBeUnSelectedOption("2", "2");
		}

		[Test]
		public void select_options_told_to_hide_the_first_option_should_not_emit_the_first_option_text()
		{
			var optionNodes = new Select("foo").Options(new[] { 1, 2 }).FirstOption("-Choose-").HideFirstOptionWhen(true).ToString()
				.ShouldHaveHtmlNode("foo")
				.ShouldHaveChildNodesCount(2);

			optionNodes[0].ShouldBeUnSelectedOption("1", "1");
			optionNodes[1].ShouldBeUnSelectedOption("2", "2");
		}
	}
}