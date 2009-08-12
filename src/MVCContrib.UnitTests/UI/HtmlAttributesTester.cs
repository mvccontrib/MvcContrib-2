using System;
using System.Collections.Generic;
using System.Collections;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	[TestFixture]
	public class HtmlAttributesTester
	{
		[TestFixture]
		public class With_All_Properties
		{
			[Test]
			public void A_Few_One_Liner_Tests()
			{
				var attribs = new HtmlAttributes();;
				Assert.That(attribs["IDontExist"], Is.EqualTo(null));
				Assert.That(attribs.IsReadOnly, Is.False);
				Assert.That(attribs.IsFixedSize, Is.False);
			}

			[Test]
			public void An_Attribute_Sticks_In_The_Collection()
			{
				var attribs = new HtmlAttributes {{"AnAttribute", "AValue"}};
				Assert.That(attribs["AnAttribute"], Is.EqualTo("AValue"));
			}

			[Test]
			public void An_Attribute_Sticks_When_Added_By_KeyValuePair()
			{
				var attribs = new HtmlAttributes {new KeyValuePair<string, string>("AnAttribute", "AValue")};
				Assert.That(attribs["AnAttribute"], Is.EqualTo("AValue"));
			}

			[Test]
			public void Can_Add_Objects_As_Attribs_And_Values_And_Will_use_ToString()
			{
				var attribs = new HtmlAttributes();
				var element = new Element();
				attribs.Add(element, 96);
				Assert.That(attribs[element.ToString()], Is.EqualTo("96"));
			}

			[Test]
			public void Counting_Is_Accurate()
			{
				var attribs = new HtmlAttributes();
				Assert.That(attribs.Count, Is.EqualTo(0));
				attribs["X"] = "1";
				attribs["Y"] = "2";
				attribs["Z"] = "3";
				Assert.That(attribs.Count, Is.EqualTo(3));
				attribs.Clear();
				Assert.That(attribs.Count, Is.EqualTo(0));
			}

			[Test]
			public void Est_Length_Is_Accurate()
			{
				var attribs = new HtmlAttributes();
				Assert.That(attribs.GetEstLength(), Is.EqualTo(0));
				attribs["Attrib1"] = "1Value";
				Assert.That(attribs.GetEstLength(), Is.EqualTo(16));
				attribs["Attrib2"] = "2Value-Changed";
				Assert.That(attribs.GetEstLength(), Is.EqualTo(40));
				attribs["Attrib3-Changed"] = "3Val";
				Assert.That(attribs.GetEstLength(), Is.EqualTo(62));
				attribs["Attrib1"] = null;
				Assert.That(attribs.GetEstLength(), Is.EqualTo(46));
				attribs["Attrib2"] = "2Value-Changed-more";
				Assert.That(attribs.GetEstLength(), Is.EqualTo(51));
			}

			[Test]
			public void Removing_An_Item_Doesnot_Break_Est()
			{
				var attribs = new HtmlAttributes();
				attribs["Attrib1"] = "1Value";
				attribs["Attrib2"] = "2Value-Changed";
				attribs["Attrib3-Changed"] = "3Val";
				Assert.That(attribs.GetEstLength(), Is.EqualTo(62));
				attribs.Remove("Attrib1");
				Assert.That(attribs.GetEstLength(), Is.EqualTo(46));
				attribs["Attrib1"] = "1Value";
				attribs.Remove(new KeyValuePair<string, string>("Attrib2", "2Value-Changed"));
				Assert.That(attribs.GetEstLength(), Is.EqualTo(38));
			}

			[Test]
			public void I_Can_Add_A_Dictionary_At_Init()
			{
				var hash = new Hash {{"Key1", "Val1"}};
				var attribs = new HtmlAttributes(hash);
				Assert.That(attribs["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Explisit_Interfaces_Work()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var attribs = new HtmlAttributes(hash);
				var iattribs = (IDictionary)attribs;
				Assert.That(iattribs.Count, Is.EqualTo(attribs.Count));
				Assert.That(iattribs.Keys, Is.EqualTo(attribs.Keys));
				Assert.That(iattribs.Values, Is.EqualTo(attribs.Values));
				iattribs.Remove("Key1");
				Assert.That(iattribs.Count, Is.EqualTo(attribs.Count));
				iattribs["NewKey"] = 99;
				Assert.That(iattribs["NewKey"], Is.EqualTo("99"));
				foreach (DictionaryEntry de in iattribs)
				{
					Assert.That(de.Value, Is.EqualTo(attribs[de.Key.ToString()]));
				}
				iattribs["NewKey"] = null;
				Assert.That(iattribs["NewKey"], Is.EqualTo(null));
				var enumAttribs = (IEnumerable)attribs;
				foreach (KeyValuePair<string,string> de in enumAttribs)
				{
					Assert.That(de.Value, Is.EqualTo(attribs[de.Key]));
				}
			}

			[Test]
			public void These_Method_Are_Not_Implemented()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var attribs = new HtmlAttributes(hash);
				var iattribs = (ICollection)attribs;
				int exceptionCount = 0;
				try
				{
					object val = iattribs.SyncRoot;
				}
				catch(Exception ex)
				{
					Assert.That(ex.GetType(), Is.EqualTo(typeof(NotImplementedException)));
					exceptionCount ++;
				}

				try
				{
					iattribs.CopyTo(new[] { "1" }, 0);
				}
				catch(Exception ex)
				{
					Assert.That(ex.GetType(), Is.EqualTo(typeof(NotImplementedException)));
					exceptionCount ++;
				}

				try
				{
					bool val = iattribs.IsSynchronized;
				}
				catch(Exception ex)
				{
					Assert.That(ex.GetType(), Is.EqualTo(typeof(NotImplementedException)));
					exceptionCount++;
				}

				try
				{
					attribs.CopyTo(new[] { new KeyValuePair<string, string>("ValNew", "ValNew") }, 10);
				}
				catch (Exception ex)
				{
					Assert.That(ex.GetType(), Is.EqualTo(typeof(NotImplementedException)));
					exceptionCount++;
				}

				Assert.That(exceptionCount, Is.EqualTo(4));
			}

			[Test]
			public void Contains_And_ContainsKeys_Work()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var attribs = new HtmlAttributes(hash);
				Assert.IsTrue(attribs.Contains("Key1"));
				Assert.IsFalse(attribs.Contains("Key99"));
				Assert.IsTrue(attribs.Contains(new KeyValuePair<string,string>("Key1","OnlyKeyIsChecked")));
				Assert.IsFalse(attribs.Contains(new KeyValuePair<string, string>("Key99", "OnlyKeyIsChecked")));
			}

			[Test]
			public void TryGetValue_Does_Not_Fail()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var attribs = new HtmlAttributes(hash);
				string val;
				attribs.TryGetValue("Key1", out val);
				Assert.That(val, Is.EqualTo("Val1"));
				attribs.TryGetValue("Key99", out val);
				Assert.That(val, Is.EqualTo(null));
			}

			[Test]
			public void Should_be_case_insensitive()
			{
				var attributes = new HtmlAttributes {{"Key", "value"}};
				Assert.That(attributes["key"], Is.EqualTo("value"));
			}
		}
	}
}
