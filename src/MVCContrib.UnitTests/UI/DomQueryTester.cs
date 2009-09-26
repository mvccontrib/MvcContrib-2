using System;
using MvcContrib.UI;
using NUnit.Framework;

using System.Linq;

namespace MvcContrib.UnitTests.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	[TestFixture]
	public class DomQueryTester
	{
		[TestFixture]
		public class WhenParse
		{
			[Test]
			public void WithOnlyId_Then_HasOnlyIds_Is_True()
			{
				DomQuery q = "#id";
				Assert.That(q.HasOnlyIds, Is.True);
			}

			[Test]
			public void WithMultipleIds_Then_HasOnlyIds_Is_False()
			{
				// We can't match muliple yet, sorry.
				DomQuery q = "#id, #chicken, #goose";
				Assert.That(q.HasOnlyIds, Is.False);
			}
			[Test]
			public void Implicit_Cast_To_String_Works()
			{

				var query = new DomQuery("#goose", "goose");
				string squery = query;
				Assert.That(squery,Is.EqualTo("#goose"));

			}

			[Test]
			public void Implicit_Cast_To_String_On_Null_Returns_Null()
			{
				DomQuery query = null;
				string squery = query;
				Assert.That(squery, Is.EqualTo(null));
			}
		}

		[TestFixture]
		public class WhenCreate
		{
			[Test]
			public void With_A_SingleID()
			{
				var query = new DomQuery("#goose", "goose");
				Assert.That(query.HasOnlyIds, Is.True);
				Assert.That(query.IsSingle, Is.True);
				Assert.That(query.Ids.First(), Is.EqualTo("goose"));
				Assert.That(query.Ids.Count(), Is.EqualTo(1));
			}

			[Test]
			public void With_A_Bunch_Of_Ids()
			{
				var query = new DomQuery("#goose, #chicken, #duck", true, new[] { "goose", "chicken", "duck" });
				Assert.That(query.HasOnlyIds, Is.True);
				Assert.That(query.IsSimple, Is.False);
				Assert.That(query.Ids.First(), Is.EqualTo("goose"));
				Assert.That(query.Ids.Count(), Is.EqualTo(3));
			}

			[Test, ExpectedException(typeof(InvalidOperationException))]
			public void With_A_Bunch_Of_Ids_Throws_When_Trying_To_Get_One_Id()
			{
				var query = new DomQuery("#goose, #chicken, #duck", true, new[] { "goose", "chicken", "duck" });
				string id = query.Id;
				Assert.Fail("Should not be able to get a single id when domquery has a bunch of ids");
			}
		}
	}
}
