using System;
using MvcContrib.FluentHtml;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ViewModelContainerExtensionsTests
	{
		private FakeViewModelContainer<FakeModel> target;
		private FakeModel fake;

		[SetUp]
		public void SetUp()
		{
			target = new FakeViewModelContainer<FakeModel>();
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
			target.ViewModel = fake;
		}

		[Test]
		public void can_get_textbox_with_value_from_simple_property()
		{
			var element = target.TextBox(x => x.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_textbox_with_value_from_complex_property()
		{
			var element = target.TextBox(x => x.Person.FirstName);
			element.ValueAttributeShouldEqual(fake.Person.FirstName);
		}

		[Test]
		public void can_get_checkbox_with_checked()
		{
			var element = target.CheckBox(x => x.Done);
			element.AttributeShouldEqual(HtmlAttribute.Checked, HtmlAttribute.Checked);
		}

		[Test]
		public void can_get_label_for_given_input()
		{
			var element = target.Label(x => x.Title);
			element.AttributeShouldEqual(HtmlAttribute.For, "Title");
			element.InnerTextShouldEqual("Title");
		}

		[Test]
		public void Can_get_label_for_given_input_with_prefix()
		{
			target.HtmlNamePrefix = "foo";
			var element = target.Label(x => x.Title);
			element.AttributeShouldEqual(HtmlAttribute.For, "foo_Title");
		}

		[Test]
		public void can_get_literal_with_value()
		{
			var element = target.Literal(x => x.Title);
			element.InnerTextShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_form_literal_with_value()
		{
			var element = target.FormLiteral(x => x.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_hidden_with_value()
		{
			var element = target.Hidden(x => x.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_index_element()
		{
			target = new FakeViewModelContainer<FakeModel>("fake") { ViewModel = fake };
			var element = target.Index(x => x, x => x.Id);
			element.AttributeShouldEqual(HtmlAttribute.Name, "fake.Index");
			element.AttributeShouldEqual(HtmlAttribute.Id, "fake_Index_" + fake.Id);
		}

		[Test]
		public void can_get_index_element_with_value()
		{
			target = new FakeViewModelContainer<FakeModel>("fake") {ViewModel = fake};
			var element = target.Index(x => x, x => x.Id);
			element.AttributeShouldEqual(HtmlAttribute.Name, "fake.Index");
			element.ValueAttributeShouldEqual(fake.Id.ToString());
		}

		[Test]
		public void can_get_select_with_selected_value()
		{
			var element = target.Select(x => x.Id);
			element.SelectedValues.ShouldCount(1);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Id);
		}

		[Test]
		public void can_get_multi_select_with_selected_values()
		{
			var element = target.MultiSelect(x => x.Numbers);
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
			var element = target.Password(x => fake.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_text_area_with_value()
		{
			var element = target.TextArea(x => fake.Title);
			element.InnerTextShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_validation_message_for_element_with_error()
		{
			var message = "You did something wrong";
			target.ViewData.ModelState.AddModelError("Title", "Bad stuff");
			var element = target.ValidationMessage(x => x.Title, message);
			element.InnerTextShouldEqual(message);
			element.AttributeShouldEqual(HtmlAttribute.Class, "field-validation-error");
		}

		[Test]
		public void validation_message_for_element_with_not_error_return_null()
		{
			target.ValidationMessage(x => x.Title, "foo").ToString().ShouldBeNull();
		}

		[Test]
		public void can_get_validation_message_for_element_with_error_with_custom_class()
		{
			var message = "You did something wrong";
			target.ViewData.ModelState.AddModelError("Title", "Bad stuff");
			var element = target.ValidationMessage(x => x.Title, message).Class("error");
			element.AttributeShouldEqual(HtmlAttribute.Class, "error");
		}

		[Test]
		public void validation_message_for_element_with_no_message_specified_uses_model_state_error_message()
		{
			target.ViewData.ModelState.AddModelError("Title", "Bad stuff");
			var element = target.ValidationMessage(x => x.Title);
			element.InnerTextShouldEqual("Bad stuff");
		}

		[Test]
		public void can_get_id_for_expression()
		{
			var result = target.IdFor(x => x.Person.FirstName);
			result.ShouldEqual("Person_FirstName");
		}

		[Test]
		public void can_get_name_for_expression()
		{
			var result = target.NameFor(x => x.Person.FirstName);
			result.ShouldEqual("Person.FirstName");
		}

		[Test]
		public void can_get_id_for_expression_with_prefix()
		{
			target.HtmlNamePrefix = "foo";
			var result = target.IdFor(x => x.Person.FirstName);
			result.ShouldEqual("foo_Person_FirstName");
		}

		[Test]
		public void can_get_name_for_expression_with_prefix()
		{
			target.HtmlNamePrefix = "foo";
			var result = target.NameFor(x => x.Person.FirstName);
			result.ShouldEqual("foo.Person.FirstName");			
		}


		[Test]
		public void can_get_radio_set_with_selected_value()
		{
			var element = target.RadioSet(x => x.Id);
			element.SelectedValues.ShouldCount(1);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Id);
		}

		[Test]
		public void can_get_radio_button()
		{
			target.RadioButton(x => x.Id).AttributeShouldEqual(HtmlAttribute.Name, "Id");
		}

		[Test]
		public void can_get_checkboxlist_with_selected_values()
		{
			var element = target.CheckBoxList(x => x.Numbers);
			element.SelectedValues.ShouldCount(2);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[0]);
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[1]);
		}

		[Test]
		public void can_get_submit_button_with_value()
		{
			var element = target.SubmitButton("Push Me");
			element.ValueAttributeShouldEqual("Push Me");
		}
	}
}
