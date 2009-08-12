using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Expressions;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class MemberExpressionExtensionTests
	{
		private FakeModel model;

		[SetUp]
		public void SetUp()
		{
			model = new FakeModel
			{
				Title = "Test Title",
				Date = DateTime.Now,
				Done = true,
				Id = 123,
				Person = new FakeChildModel { FirstName = "Mick", LastName = "Jagger" },
				Numbers = new [] {1, 3},
				Customers = new List<FakeChildModel>
				{
					new FakeChildModel { FirstName = "John", LastName = "Wyane", Balance = 123.33m },
					new FakeChildModel { FirstName = "Marlin", LastName = "Brando", Balance = 234.56m }
				}
			};
		}

		[Test]
		public void can_get_name_for_simple_string_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Title;
			var name = expression.GetNameFor();
			name.ShouldEqual("Title");
		}

		[Test]
		public void can_get_name_for_simple_decimal_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Price;
			var name = expression.GetNameFor();
			name.ShouldEqual("Price");
		}

		[Test]
		public void can_get_name_for_compound_string_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var name = expression.GetNameFor();
			name.ShouldEqual("Person.FirstName");
		}

		[Test]
		public void can_get_name_for_instance_of_simple_collection_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Numbers[0];
			var name = expression.GetNameFor();
			name.ShouldEqual("Numbers[0]");
		}

		[Test]
		public void can_get_name_for_instance_of_simple_collection_property_using_a_local_variable_as_indexer()
		{
			var i = 33;
			Expression<Func<FakeModel, object>> expression = x => x.Numbers[i];
			var name = expression.GetNameFor();
			name.ShouldEqual("Numbers[33]");
		}

		[Test]
		public void can_get_name_for_instance_of_complex_collection_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Customers[0].FirstName;
			var name = expression.GetNameFor();
			name.ShouldEqual("Customers[0].FirstName");
		}

		[Test]
		public void can_get_name_for_instance_of_nested_complex_collection_property()
		{
			var i = 0;
			Expression<Func<FakeModel, object>> expression = x => x.FakeModelList[1].Customers[i].Balance;
			var name = expression.GetNameFor();
			name.ShouldEqual("FakeModelList[1].Customers[0].Balance");
		}

		[Test]
		public void can_get_name_for_property_of_instance_of_collection()
		{
			Expression<Func<IList<FakeModel>, object>> expression = x => x[999].Title;
			var name = expression.GetNameFor();
			name.ShouldEqual("[999].Title");
		}

		[Test]
		public void can_get_name_for_property_of_instance_of_array()
		{
			var i = 888;
			Expression<Func<FakeModel, object>> expression = x => x.FakeModelArray[999].FakeModelArray[i].Person.FirstName;
			var name = expression.GetNameFor();
			name.ShouldEqual("FakeModelArray[999].FakeModelArray[888].Person.FirstName");
		}

		[Test]
		public void can_get_value_from_model()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Customers[1].Balance;
			var value = expression.GetValueFrom(model);
			value.ShouldEqual(model.Customers[1].Balance);
		}

		[Test]
		public void get_value_for_null_intermediate_property_returns_null()
		{
			model.Person = null;
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var value = expression.GetValueFrom(model);
			value.ShouldBeNull();
		}

		[Test]
		public void get_name_for_applies_prefix_from_view()
		{
			var view = new FakeViewModelContainer<FakeModel>("prefix");
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			expression.GetNameFor(view).ShouldEqual("prefix.Person.FirstName");
		}

		[Test]
		public void get_name_for_applies_prefix_from_view_to_collection_model()
		{
			var view = new FakeViewModelContainer<IList<FakeModel>>("prefix");
			Expression<Func<IList<FakeModel>, object>> expression = x => x[123].Person.FirstName;
			expression.GetNameFor(view).ShouldEqual("prefix[123].Person.FirstName");
		}
	}
}
