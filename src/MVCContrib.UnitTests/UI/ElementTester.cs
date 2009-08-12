using System;
using System.Collections;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	[TestFixture]
	public class ElementTester
	{
		[TestFixture]
		public class With_All_Properties
		{
			[Test]
			public void When_Tag_Has_No_Value_Exception_Is_Thrown()
			{
				var element = new Element {Tag = ""};
				try
				{
					string val = element.ToString();
				}
				catch (System.Exception e)
				{
					Assert.That(e.Message, Is.EqualTo("tag must contain a value"));
				}
			}

			[Test]
			public void When_Created_Without_A_Tag_Then_The_Tag_Is_A_Div()
			{
				var el = new Element();
				Assert.That(el.Tag, Is.EqualTo("div"));
			}

			[Test]
			public void Use_Full_Close_Tag_Is_False()
			{
				var el = new Element("ul");
				Assert.That(el.UseFullCloseTag, Is.False);
			}

			[Test]
			public void When_Created_With_A_Tag_Then_That_Tag_Is_Got()
			{
				var el = new Element("ul");
				Assert.That(el.Tag, Is.EqualTo("ul"));
			}

			[Test]
			public void When_Tag_Is_Set_Then_Tag_Is_Got()
			{
				var el = new Element {Tag = "goose"};
				Assert.That(el.Tag, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Id_Is_Set_Then_Id_Is_Got()
			{
				var el = new Element();
				Assert.That(el.Id, Is.Null);
				el.Id = "goose";
				Assert.That(el.Id, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Class_Is_Set_Then_Class_Is_Got()
			{
				var el = new Element();
				Assert.That(el.Class, Is.Null);
				el.Class = "goose";
				Assert.That(el.Class, Is.EqualTo("goose"));
			}

			[Test]
			public void When_OnClick_Is_Set_Then_OnClick_Is_Got()
			{
                var el = new ScriptableElement();
				Assert.That(el.OnClick, Is.Null);
				el.OnClick = "goose";
				Assert.That(el.OnClick, Is.EqualTo("goose"));
			}

			[Test]
			public void When_InnerHtml_Is_Set_Then_InnerHtml_Is_Got()
			{
				var el = new Element();
				Assert.That(el.InnerText, Is.EqualTo(string.Empty));
				el.InnerText = "goose";
				Assert.That(el.InnerText, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Using_This_Id_Is_Set_And_This_Id_Is_Got_Then_This_Id_Equals_Id_Property()
			{
				var el = new Element();
				Assert.That(el.Id, Is.Null);
				el["id"] = "goose";
				Assert.That(el["id"], Is.EqualTo("goose"));
				Assert.That(el.Id, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Id_And_Class_Are_Set_Then_Can_Enumerate_Over_Id_And_Class()
			{
				var el = new Element {Id = "goose", Class = "chicken"};
				bool sawId = false;
				bool sawClass = false;
				foreach (DictionaryEntry attribute in el)
				{
					if (attribute.Key.Equals("id") && attribute.Value.Equals("goose"))
					{
						sawId = true;
					}
					if (attribute.Key.Equals("class") && attribute.Value.Equals("chicken"))
					{
						sawClass = true;
					}
				}
				Assert.That(sawId, Is.True);
				Assert.That(sawClass, Is.True);
			}

			[Test]
			public void When_Id_And_Class_Are_Set_Then_Can_Enumerate_Over_Id_And_Class_NonGeneric()
			{
				var el = new Element {Id = "goose", Class = "chicken"};
				bool sawId = false;
				bool sawClass = false;
				foreach (var val in (IEnumerable)el)
				{
					var attribute = (DictionaryEntry) val;
					if (attribute.Key.Equals("id") && attribute.Value.Equals("goose"))
					{
						sawId = true;
					}
					if (attribute.Key.Equals("class") && attribute.Value.Equals("chicken"))
					{
						sawClass = true;
					}
				}
				Assert.That(sawId, Is.True);
				Assert.That(sawClass, Is.True);
			}

			[Test]
			public void When_Getting_ToString_Then_Tag_Is_Generated()
			{
				var el = new Element();
				Assert.That(el.ToString(), Text.Contains("<div />"));
			}

			[Test]
			public void When_Getting_ToString_With_Id_Then_Id_Attribute_Is_Generated()
			{
				var el = new Element {Id = "goose"};
				Assert.That(el.ToString(), Text.Contains("id=\"goose\""));
			}

            [Test]
            public void When_Getting_ToString_With_A_Empty_Attribute_Then_It_Is_Not_Generated()
            {
                var el = new Element {Id = "goose"};
            	el["checked"] = null;
                Assert.That(el.ToString(), Text.DoesNotContain("checked"));
            }

			[Test]
			public void When_Set_Selector_Then_Get_Selector()
			{
				var element = new Element {Selector = "#foo"};
				Assert.That(element.Selector.ToString(), Is.EqualTo("#foo"));
			}

			[Test]
			public void When_Set_Id_Then_Get_Selector()
			{
				var element = new Element {Id = "foo"};
				Assert.That(element.Selector.ToString(), Is.EqualTo("#foo"));
			}

			[Test]
			public void When_Selector_Is_Null_Get_ID()
			{
				var element = new Element {Id = "foo", Selector = null};
				Assert.That(element.Selector.ToString(), Is.EqualTo("foo"));
			}

			[Test]
			public void When_SelfClosing_Then_SelfCloses()
			{
				var element = new Element();
				Assert.That(element.ToString(), Is.EqualTo("<div />"));
			}

			[Test]
			public void When_Not_SelfClosing_Then_TagHasEnd()
			{
				var script = new MvcContrib.UI.Tags.Script();
				Assert.That(script.ToString(), Is.EqualTo("<script type=\"text/javascript\"></script>"));
			}
			
			[Test]
			public void Tag_Will_Close_With_Full_Tag_If_There_Is_InnerText()
			{
				var element = new Element {InnerText = "This is Text"};
				Assert.That(element.ToString(), Is.EqualTo("<div >This is Text</div>"));
			}

			[Test]
			public void Tag_With_Escaped_Flag_Has_Escaped_Content_When_Rendered()
			{
				var element = new Element {InnerText = "This is Text", EscapeInnerText = true};
				Assert.That(element.ToString(), Is.EqualTo("<div >/*<![CDATA[*/\r\nThis is Text\r\n//]]></div>"));
			}

			[Test]
			public void Tag_With_Four_Or_More_Attributes_Renders_Correctly()
			{
				var element = new Element {InnerText = "This is Text", Id = "MyID", Class = "MyClass"};
				element["Style"] = "MyStyle";
				element["onclick"] = "MyOnClick";
				element["Gizmodo"] = "A cool Website";
				Assert.That(element.ToString(), Is.EqualTo("<div id=\"MyID\" class=\"MyClass\" Style=\"MyStyle\" onclick=\"MyOnClick\" Gizmodo=\"A cool Website\">This is Text</div>"));
			}

			[Test]
			public void When_value_is_empty_string_should_still_be_rendered()
			{
				var element = new Element("input", new Hash(value => ""));
				string actual = element.ToString();
				string expected = "<input value=\"\"/>";
				Assert.That(actual, Is.EqualTo(expected));
				
			}

			[Test]
			public void When_id_has_period_should_be_replaced_with_hyphen()
			{
				var element = new Element {Id = "foo.bar"};
				string actual = element.ToString();
				string expected = "<div id=\"foo-bar\"/>";
				Assert.That(actual, Is.EqualTo(expected));
			}

			[Test]
			public void When_label_element_then_for_attribute_should_have_periods_replaced_with_hyphens()
			{
				var element = new Element("label");
				element["for"] = "foo.bar";
				element.InnerText = "Test";
				string actual = element.ToString();
				string expected = "<label for=\"foo-bar\">Test</label>";
				Assert.That(actual, Is.EqualTo(expected));
				
			}


		}
	}
}
