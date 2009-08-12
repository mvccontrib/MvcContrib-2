using System;
using MvcContrib.FluentHtml;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ViewDataContainerExtensionsTests
	{
		private FakeViewDataContainer target;
		private FakeModel fake;

		[SetUp]
		public void SetUp()
		{
			target = new FakeViewDataContainer();
			fake = new FakeModel
			{
				Title = "Test Title",
				Date = DateTime.Now,
				Done = true,
				Id = 123,
				Person = new FakeChildModel
				{
					FirstName = "Mick",
					LastName = "Jagger"
				},
				Numbers = new [] {1, 3}
			};
			target.ViewData["fake"] = fake;
		}

		[Test]
		public void can_get_textbox_with_value_from_simple_property()
		{
			var element = target.TextBox("fake.Title");
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_textbox_with_value_from_complex_property()
		{
			var element = target.TextBox("fake.Person.FirstName");
			element.ValueAttributeShouldEqual(fake.Person.FirstName);
		}

		[Test]
		public void can_get_checkbox_with_checked()
		{
			var element = target.CheckBox("fake.Done");
			element.AttributeShouldEqual(HtmlAttribute.Checked, HtmlAttribute.Checked);
		}

		[Test]
		public void can_get_file_upload()
		{
			var element = target.FileUpload("test");
			element.ShouldNotBeNull();
		}

		[Test]
		public void can_get_label_with_value()
		{
			var element = target.Label("fake.Title");
			element.AttributeShouldEqual(HtmlAttribute.For, "fake_Title");
			element.InnerTextShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_literal_with_value()
		{
			var element = target.Literal("foo bar");
			element.InnerTextShouldEqual("foo bar");
		}

        [Test]
        public void can_get_literal_with_name_and_value()
        {
            var element = target.Literal("name", "foo bar");
            element.InnerTextShouldEqual("foo bar");
            element.AttributeShouldEqual("name", "name");
        }

		[Test]
		public void can_get_form_literal_with_value()
		{
			var element = target.FormLiteral("fake.Title");
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_hidden_with_value()
		{
			var element = target.Hidden("fake.Title");
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_select()
		{
			var element = target.Select("fake.Id");
			element.SelectedValues.ShouldCount(1);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Id);
		}

		[Test]
		public void can_get_multi_select()
		{
			var element = target.MultiSelect("fake.Numbers");
			element.SelectedValues.ShouldCount(2);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[0]);
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[1]);
		}

		[Test]
		public void can_get_password_with_value()
		{
			var element = target.Password("fake.Title");
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_submit_button_with_value()
		{
			var element = target.SubmitButton("Push Me");
			element.ValueAttributeShouldEqual("Push Me");
		}

		[Test]
		public void can_get_text_area_with_value()
		{
			var element = target.TextArea("fake.Title");
			element.InnerTextShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_radio_set_with_selected_value()
		{
			var element = target.RadioSet("fake.Id");
			element.SelectedValues.ShouldCount(1);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Id);
		}

		[Test]
		public void can_get_radio_button()
		{
			target.RadioButton("Id").AttributeShouldEqual(HtmlAttribute.Name, "Id");
		}

		[Test]
		public void can_get_checkboxlist_with_selected_values()
		{
			var element = target.CheckBoxList("fake.Numbers");
			element.SelectedValues.ShouldCount(2);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[0]);
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[1]);
		}
	}
}
