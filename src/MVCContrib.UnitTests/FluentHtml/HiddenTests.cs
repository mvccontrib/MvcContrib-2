using System;
using System.Linq.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class HiddenTests
	{
		[Test]
		public void basic_password_renders_with_corect_tag_and_type()
		{
			new Hidden("x").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Hidden);
		}

		[Test]
		public void model_password_renders_with_id_and_name()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var html = new Hidden(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString();
			html.ShouldHaveHtmlNode("Person_FirstName").ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Person.FirstName");
		}
	}
}
