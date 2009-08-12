using System;
using MvcContrib.UI;
using System.Web.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI
{

	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	[TestFixture]
	public class DomBuilderTester
	{
		[Test]
		public void When_New_With_Id()
		{
			DomQuery domQuery = new DomQueryBuilder("goose");
			Assert.That(domQuery.Id, Is.EqualTo("goose"));
		}

		[Test]
		public void When_No_Id_AddToList_Does_Nothing()
		{
			var dq = new DomQueryBuilder();
			var emptyQuery = new DomQuery("",true,new string[]{});
			Assert.That(dq.ToDomQuery().ToString(), Is.EqualTo(emptyQuery.ToString()));
		}

		[TestFixture]
		public class When_Using_ElementId
		{
			[Test]
			public void Then_Returns_PoundElementId()
			{
				DomQuery query = new DomQueryBuilder().Id("goose");
				Assert.That(query.ToString(), Is.EqualTo("#goose"));
			}
		}

		[TestFixture]
		public class When_Using_Class
		{
			[Test]
			public void Then_Returns_PeriodClassName()
			{
				DomQuery query = new DomQueryBuilder().Class("goose");
				Assert.That(query.ToString(), Is.EqualTo(".goose"));
			}
		}

		[TestFixture]
		public class When_Using_Tag
		{
			[Test]
			public void Then_Returns_Tag()
			{
				DomQuery query = new DomQueryBuilder().Tag(HtmlTextWriterTag.P);
				Assert.That(query.ToString(), Is.EqualTo("p"));
			}
		}

		[TestFixture]
		public class When_Using_TwoElementIds
		{
			[Test]
			public void Then_Returns_PoundElementId1_Comma_PoundElementId2()
			{
				DomQuery query = new DomQueryBuilder().Id("goose").And.Id("chicken");
				Assert.That(query.ToString(), Is.EqualTo("#goose,#chicken"));
			}
		}

		[TestFixture]
		public class When_Using_TagWithClass
		{
			[Test]
			public void Then_Returns_Tag_Period_ClassName()
			{
				DomQuery query = new DomQueryBuilder().Tag(HtmlTextWriterTag.P).Class("chicken");
				Assert.That(query.ToString(), Is.EqualTo("p.chicken"));
			}
		}

		[TestFixture]
		public class When_Using_ElementIdWithClass
		{
			[Test]
			public void Then_Returns_PoundElementId_Period_ClassName()
			{
				DomQuery query = new DomQueryBuilder().Id("goose").Class("chicken");
				Assert.That(query.ToString(), Is.EqualTo("#goose.chicken"));
			}
		}

		[TestFixture]
		public class When_Using_TagWithElementId
		{
			[Test]
			public void Then_Returns_TagPoundElementId()
			{
				DomQuery query = new DomQueryBuilder().Tag(HtmlTextWriterTag.P).Id("goose");
				Assert.That(query.ToString(), Is.EqualTo("p#goose"));
			}
		}

		[TestFixture]
		public class When_Using_TagWithDescendantClass
		{
			[Test]
			public void Then_Returns_Tag_ClassName()
			{
				DomQuery query = new DomQueryBuilder().Tag(HtmlTextWriterTag.P).Descendant.Class("goose");
				Assert.That(query.ToString(), Is.EqualTo("p .goose"));
			}
		}

		[TestFixture]
		public class When_Using_ElementIdWithDescendantTagWithDescendantClass
		{
			[Test]
			public void Then_Returns_ElementId_Tag_ClassName()
			{
				DomQuery query = new DomQueryBuilder().Id("goose").Descendant.Tag(HtmlTextWriterTag.P).Descendant.Class("chicken");
				Assert.That(query.ToString(), Is.EqualTo("#goose p .chicken"));
			}
		}

		[TestFixture]
		public class When_Using_Implicit_Casting
		{
			[Test]
			public void Then_Returns_A_String()
			{
				string query = new DomQueryBuilder().Id("goose").Descendant.Tag(HtmlTextWriterTag.P).Descendant.Class("chicken");
				Assert.That(query, Is.EqualTo("#goose p .chicken"));
			}
		}
	}
}
