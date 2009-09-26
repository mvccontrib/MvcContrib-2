using System;
using System.Collections;
using HtmlAgilityPack;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using NUnit.Framework;
using FluentHtmlAttribute = MvcContrib.FluentHtml.Html.HtmlAttribute;
using HtmlAttribute=HtmlAgilityPack.HtmlAttribute;

namespace MvcContrib.UnitTests.FluentHtml.Helpers
{
	public static class TestExtensions
	{
		public static T ShouldBeOfType<T>(this object obj)
		{
			Assert.IsInstanceOf<T>(obj);
			return (T)obj;
		}

		public static object ShouldBeOfType(this object obj, Type type)
		{
			Assert.IsInstanceOf(type, obj);
			return obj;
		}

		public static object ShouldNotBeOfType<T>(this object obj)
		{
			Assert.IsNotInstanceOf<T>(obj);
			return obj;
		}

		public static object ShouldNotBeOfType(this object obj, Type type)
		{
			Assert.IsNotInstanceOf(type, obj);
			return obj;
		}

		public static HtmlAttribute ShouldHaveAttribute(this HtmlNode n, string attributeName)
		{
			var attribute = n.Attributes[attributeName];
			Assert.IsNotNull(attribute);
			return attribute;
		}

		public static HtmlNode ShouldNotHaveAttribute(this HtmlNode n, string attributeName)
		{
			var attribute = n.Attributes[attributeName];
			Assert.IsNull(attribute);
			return n;
		}

		public static void WithValue(this HtmlAttribute a, string expectedValue)
		{
			Assert.AreEqual(expectedValue, a.Value);
		}

		public static HtmlNode ShouldHaveAttributesCount(this HtmlNode n, int expectedCount)
		{
			Assert.AreEqual(expectedCount, n.Attributes.Count);
			return n;
		}

		public static HtmlNode ShouldBeNamed(this HtmlNode n, string name)
		{
			Assert.AreEqual(name, n.Name);
			return n;
		}

		public static HtmlNode ShouldHaveInnerTextEqual(this HtmlNode n, string innerText)
		{
			Assert.AreEqual(innerText, n.InnerText);
			return n;
		}

        public static HtmlNode ShouldHaveInnerHtmlEqual(this HtmlNode n, string innerHtml)
        {
            Assert.AreEqual(innerHtml, n.InnerHtml);
            return n;
        }

		public static HtmlNode ShouldHaveHtmlNode(this string s, string elementId)
		{
			var doc = s.ShouldRenderValidHtml();
			var node = doc.GetElementbyId(elementId);
			Assert.IsNotNull(node);
			Assert.IsTrue(node.Closed, "The element is not closed");
			return node;
		}

		public static HtmlNode ShouldHaveChildNode(this HtmlNode n, string elementId)
		{
			foreach (var node in n.ChildNodes)
			{
				if (node.Id == elementId)
				{
					return node;
				}
			}
			Assert.Fail("The node did not contain a child node with id of " + elementId);
			return null;
		}

		public static HtmlNode ShouldRenderHtmlDocument(this string s)
		{
			var doc = s.ShouldRenderValidHtml();
			return doc.DocumentNode;
		}

		public static HtmlDocument ShouldRenderValidHtml(this string s)
		{
			var doc = new HtmlDocument
			{
				OptionAutoCloseOnEnd = false,
				OptionCheckSyntax = false,
				OptionFixNestedTags = false
			};
			doc.LoadHtml(s);
			if (doc.ParseErrors.Count > 0)
			{
				string errors = null;
				foreach (var error in doc.ParseErrors)
				{
					errors += ((HtmlParseError)error).Code + Environment.NewLine;
				}
				Assert.Fail("There were parser errors:{0}{1}", Environment.NewLine, errors);
			}
			return doc;
		}

		public static HtmlAttribute ValueShouldContain(this HtmlAttribute a, string expectedSubstring)
		{
			a.Value.ShouldContain(expectedSubstring);
			return a;
		}

		public static string ShouldContain(this string s, string expectedSubstring)
		{
			Assert.IsTrue(s.LastIndexOf(expectedSubstring) > -1);
			return s;
		}

		public static HtmlNode ShouldHaveNoChildNodes(this HtmlNode n)
		{
			Assert.IsFalse(n.HasChildNodes);
			return n;
		}

		public static HtmlNodeCollection ShouldHaveChildNodesCount(this HtmlNode n, int expectedCount)
		{
			Assert.AreEqual(expectedCount, n.ChildNodes.Count);
			return n.ChildNodes;
		}

		public static object ShouldEqual(this object o, object expectedValue)
		{
			Assert.AreEqual(expectedValue, o);
			return o;
		}

		public static object ShouldBeNull(this object o)
		{
			Assert.IsNull(o);
			return o;
		}

		public static object ShouldNotBeNull(this object o)
		{
			Assert.IsNotNull(o);
			return o;
		}

		public static IEnumerable ShouldCount(this IEnumerable e, int expectedCount)
		{
			var count = 0; 
			foreach (var item in e)
			{
				count++;
			}
			Assert.AreEqual(expectedCount, count);
			return e;
		}

		public static IElement ValueAttributeShouldEqual(this IElement e, string expectedValue)
		{
			return e.AttributeShouldEqual(FluentHtmlAttribute.Value, expectedValue);
		}

		public static IElement AttributeShouldEqual(this IElement e, string attributeKey, string expectedValue)
		{
			e.ToString(); //NOTE: Some attributes may be set lazily.  This forces it.
			Assert.IsTrue(e.Builder.Attributes.ContainsKey(attributeKey));
			e.Builder.Attributes[attributeKey].ShouldEqual(expectedValue);
			return e;
		}

		public static IElement InnerTextShouldEqual(this IElement e, string expectedValue)
		{
			e.ToString(); //NOTE: Some attributes may be set lazily.  This forces it.
			Assert.AreEqual(expectedValue, e.Builder.InnerHtml);
			return e;
		}

		public static void ShouldBeUnSelectedOption(this HtmlNode node, object expectedValue, object expectedText)
		{
			node.ShouldBeNamed(HtmlTag.Option);
			node.ShouldHaveAttribute(FluentHtmlAttribute.Value).WithValue(ToSafeString(expectedValue));
			node.ShouldNotHaveAttribute(FluentHtmlAttribute.Selected);
			node.ShouldHaveInnerTextEqual(ToSafeString(expectedText));
		}

		public static void ShouldBeSelectedOption(this HtmlNode node, object expectedValue, object expectedText)
		{
			node.ShouldBeNamed(HtmlTag.Option);
			node.ShouldHaveAttribute(FluentHtmlAttribute.Value).WithValue(ToSafeString(expectedValue));
			node.ShouldHaveAttribute(FluentHtmlAttribute.Selected).WithValue(FluentHtmlAttribute.Selected);
			node.ShouldHaveInnerTextEqual(ToSafeString(expectedText));
		}

		private static string ToSafeString(object value)
		{
			return value == null ? string.Empty : value.ToString();
		}
	}
}