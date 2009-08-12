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
	public class PasswordTests
	{
		[Test]
		public void basic_password_upload_renders_with_corect_tag_and_type()
		{
			new Password("x").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Password);
		}

		[Test]
		public void model_password_renders_with_id_and_name()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.LastName;
			var html = new Password(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString();
			html.ShouldHaveHtmlNode("Person_LastName").ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Person.LastName");
		}
	}
}
