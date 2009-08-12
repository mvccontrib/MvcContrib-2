using System;
using System.Web.Mvc;
using MvcContrib.FluentHtml;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ModelStateDictionaryExtensionsTests
	{
		private class FakeViewModel
		{
			public class SubFakeViewModel
			{
				public int Age { get; set; }
			}

			public string Name { get; set; }
			public SubFakeViewModel SubModel { get; set; }
		}

		[Test]
		public void Should_add_an_error_message_to_the_model_state_using_an_expression_for_a_simple_property()
		{
			var modelState = new ModelStateDictionary();

			modelState.AddModelError<FakeViewModel>(x => x.Name, "error message");

			modelState["Name"].Errors[0].ErrorMessage.ShouldEqual("error message");
		}

		[Test]
		public void Should_add_an_error_message_to_the_model_state_using_an_expression_for_a_nested_property()
		{
			var modelState = new ModelStateDictionary();

			modelState.AddModelError<FakeViewModel>(x => x.SubModel.Age, "error message");

			modelState["SubModel_Age"].Errors[0].ErrorMessage.ShouldEqual("error message");
		}

		[Test]
		public void Should_add_an_exception_to_the_model_state_using_an_expression_for_a_simple_property()
		{
			var modelState = new ModelStateDictionary();
			var argumentException = new ArgumentException("exception message");

			modelState.AddModelError<FakeViewModel>(x => x.Name, argumentException);

			modelState["Name"].Errors[0].Exception.ShouldEqual(argumentException);
		}

		[Test]
		public void Should_add_an_exception_to_the_model_state_using_an_expression_for_a_nested_property()
		{
			var modelState = new ModelStateDictionary();
			var argumentException = new ArgumentException("exception message");

			modelState.AddModelError<FakeViewModel>(x => x.SubModel.Age, argumentException);

			modelState["SubModel_Age"].Errors[0].Exception.ShouldEqual(argumentException);
		}
	}
}
