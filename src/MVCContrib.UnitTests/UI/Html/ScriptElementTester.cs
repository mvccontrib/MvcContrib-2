using System;
using System.Collections;
using MvcContrib.UI.Tags;
using NUnit.Framework;


namespace MvcContrib.UnitTests.UI.Html
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	[TestFixture]
	public class ScriptElementTester
	{
		[TestFixture]
		public class With_All_Properties
		{

			[Test]
			public void When_Created_Then_Tag_Is_Script()
			{
                var el = new Script();
				Assert.That(el.Tag, Is.EqualTo("script"));
			}

			[Test]
			public void When_Created_With_Attribute_Dictionary_Then_Properties_Match_Dictionary_Values()
			{
                var el = new Script(new Hash(src => "source", type => "text/javascript", lang => "javascript", defer => "true"));
				Assert.That(el.Src, Is.EqualTo("source"));
				Assert.That(el.Type, Is.EqualTo("text/javascript"));
				Assert.That(el.Lang, Is.EqualTo("javascript"));
				Assert.That(el.Defer, Is.EqualTo(true));
			}

			[Test]
			public void When_Src_Is_Set_Then_Src_Is_Got()
			{
                var el = new Script {Src = "goose"};
			    Assert.That(el.Src, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Type_Is_Set_Then_Type_Is_Got()
			{
                var el = new Script();
				Assert.That(el.Type, Is.EqualTo("text/javascript"));
				el.Type = "goose";
				Assert.That(el.Type, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Defer_Is_Set_Then_Defer_Is_Got()
			{
                var el = new Script();
				Assert.That(el.Defer, Is.EqualTo(null));
				el.Defer = true;
				Assert.That(el.Defer, Is.EqualTo(true));
			}

			[Test]
			public void When_Defer_Is_Set_With_Null_Then_Defer_Is_Removed_From_Attributes()
			{
                var el = new Script();
				
				Assert.That(el.Defer, Is.Null);
				el.Defer = true;
				Assert.That(el.Defer, Is.Not.Null);

				el.Defer = null;
				foreach(DictionaryEntry attribute in el)
				{
                    if (attribute.Key.Equals(Script.ATTR_DEFER))
						Assert.Fail("Defer should have been removed from the attribute enumeration by setting it to null.");
				}
			}

			[Test]
			public void When_Lang_Is_Set_Then_Lang_Is_Got()
			{
                var el = new Script();
				Assert.That(el.Lang, Is.Null);
				el.Lang = "goose";
				Assert.That(el.Lang, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Script_Is_Set_Then_Script_Is_Got()
			{
                var el = new Script();
				Assert.That(el.Code, Is.EqualTo(string.Empty));
				el.Code = "goose";
				Assert.That(el.Code, Text.Contains("goose"));
			}

			[Test]
			public void When_InnerHtml_Is_Set_Then_Script_Is_Set()
			{
                var el = new Script();
				Assert.That(el.InnerText, Is.Empty);
				el.InnerText = "goose";
				Assert.That(el.Code, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Getting_ToString_Then_Tag_Is_Generated()
			{
                var el = new Script();
				Assert.That(el.ToString(), Text.Contains("<script"));
				Assert.That(el.ToString(), Text.Contains("</script>"));
			}

			[Test]
			public void When_Getting_ToString_With_Script_Then_Script_Is_Wrapped_In_Xhtml_Compliant_Comments()
			{
                var el = new Script {Code = "goose"};
			    Assert.That(el.ToString(), Text.Contains("/*<![CDATA[*/\r\ngoose\r\n//]]>"));
			}
		}
	}
}
