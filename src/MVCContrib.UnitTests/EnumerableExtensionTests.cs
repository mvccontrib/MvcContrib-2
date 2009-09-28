using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.EnumerableExtensions;
using NUnit.Framework;

using System.Linq;
namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class EnumerableExtensionTests
	{
		[Test]
		public void Should_create_select_list_with_value_and_text_field()
		{
			var sequence = People.ToSelectList(x => x.Id, x => x.Name);
			Assert.That(sequence.Count(), Is.EqualTo(3));

			Assert.That(sequence.ElementAt(1).Value, Is.EqualTo("2"));
			Assert.That(sequence.ElementAt(1).Text, Is.EqualTo("Eric"));

			Assert.That(sequence.Last().Value, Is.EqualTo("3"));
			Assert.That(sequence.Last().Text, Is.EqualTo("Jeremy"));
		}

		[Test]
		public void Should_create_select_list_with_selected_value()
		{
			var sequence = People.ToSelectList(x => x.Id, x => x.Name, new[] { 3 });
			Assert.That(sequence.Last().Selected, Is.True);
		}

		[Test]
		public void Should_create_select_list_with_selected_value_using_selector()
		{
			var sequence = People.ToSelectList(x => x.Id, x => x.Name, x => x.Name == "Jeremy");
			Assert.That(sequence.Last().Selected, Is.True);
		}

		[Test]
		public void Should_not_throw_when_value_field_is_null()
		{
			var people = new List<Person> {new Person {Id = 1, Name = null}};
			var sequence = people.ToSelectList(x => x.Id, x => x.Name, (IEnumerable<int>)null);
			Assert.That(sequence.Count(), Is.EqualTo(1));
		}

		private IEnumerable<Person> People
		{
			get
			{
				yield return new Person { Id = 1, Name = "Jeffrey" };
				yield return new Person { Id = 2, Name = "Eric" };
				yield return new Person { Id = 3, Name = "Jeremy" };
			}
		}


		private class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}