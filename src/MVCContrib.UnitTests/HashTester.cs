using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;


namespace MvcContrib.UnitTests
{
	public class HashTester
	{

		[TestFixture]
		public class When_Empty
		{
			[Test]
			public void Then_Hash_Is_Not_Null()
			{
				IDictionary hash = Hash.Empty;
				Assert.That(hash, Is.Not.Null);
			}

			[Test]
			public void Then_Hash_Has_Zero_Items()
			{
				IDictionary hash = Hash.Empty;
				Assert.That(hash.Count, Is.EqualTo(0));
			}
		}

		[TestFixture]
		public class With_Null_Hash
		{
			[Test]
			public void Then_A_Empty_Dictionary_Is_Returned()
			{
				IDictionary hash = new Hash(null);
				Assert.That(hash.Count, Is.EqualTo(0));
			}

			[Test]
			public void Then_The_Dictionary_Is_Not_Null()
			{
				IDictionary hash = new Hash(null);
				Assert.That(hash, Is.Not.Null);
			}
		}

		[TestFixture]
		public class With_Two_Items_And_No_Duplicate_Keys
		{
			[Test]
			public void Then_A_Dictionary_Is_Returned()
			{
				IDictionary hash = new Hash(id=>"goose", @class => "chicken");
				Assert.That(hash, Is.Not.Null);
			}

			[Test]
			public void Then_The_Dictionary_Has_A_Count_Equal_To_2()
			{
				IDictionary hash = new Hash(id => "goose", @class => "chicken");
				Assert.That(hash.Count, Is.EqualTo(2));
			}

			[Test]
			public void Then_The_Dictionary_Contains_Both_Items_With_Keys()
			{
				IDictionary hash = new Hash(id => "goose", @class => "chicken");
				Assert.That(hash["id"], Is.EqualTo("goose"));
				Assert.That(hash["class"], Is.EqualTo("chicken"));
			}
		}

		[TestFixture]
		public class When_The_Add_Extension_Method_Is_Used_With_A_New_Dictionary
		{
			[Test]
			public void Then_The_Items_Are_Added()
			{
				IDictionary hash = new Hashtable();
				hash.Add(id => "goose", @class => "chicken");
				Assert.That(hash.Count, Is.EqualTo(2));
				Assert.That(hash.Contains("id"), Is.True);
				Assert.That(hash.Contains("class"), Is.True);
			}

			[Test]
			public void With_Null_Items_Then_Nothing_Is_Added()
			{
				IDictionary hash = new Hashtable();
				IDictionary hash2 = hash.Add(null);
				Assert.That(hash.Count, Is.EqualTo(0));
				Assert.That(hash2, Is.SameAs(hash));
			}
		}

		[TestFixture]
		public class With_Two_Keys_That_Have_The_Same_Name_But_Different_Case
		{
			[Test, ExpectedException(typeof(ArgumentException))]
			public void Then_Throws_An_ArgumentException_For_Duplicate_Keys()
			{
				IDictionary hash = new Hash(id => "goose", ID => "chicken");
				Assert.Fail("Should throw an ArgumentException for duplicate keys because the dictionary is case insensitive.");
			}
		}
	}

	public class GenericHashTester
	{
		[TestFixture]
		public class When_Empty
		{
			[Test]
			public void Then_Hash_Is_Not_Null()
			{
				IDictionary<string, string> hash = Hash<string>.Empty;
				Assert.That(hash, Is.Not.Null);
			}

			[Test]
			public void Then_Hash_Has_Zero_Items()
			{
				IDictionary<string, string> hash = Hash<string>.Empty;
				Assert.That(hash.Count, Is.EqualTo(0));
			}
		}

		[TestFixture]
		public class With_Null_Hash
		{
			[Test]
			public void Then_A_Empty_Dictionary_Is_Returned()
			{
				IDictionary<string, string> hash = new Hash<string>(null);
				Assert.That(hash.Count, Is.EqualTo(0));
			}

			[Test]
			public void Then_The_Dictionary_Is_Not_Null()
			{
				IDictionary<string, string> hash = new Hash<string>(null);
				Assert.That(hash, Is.Not.Null);
			}
		}

		[TestFixture]
		public class With_Two_Keys_That_Have_The_Same_Name_But_Different_Case
		{
			[Test, ExpectedException(typeof(ArgumentException))]
			public void Then_Throws_An_ArgumentException_For_Duplicate_Keys()
			{
				IDictionary<string, string> hash = new Hash<string>(id => "goose", ID => "chicken");
				Assert.Fail("Should throw an ArgumentException for duplicate keys because the dictionary is case insensitive.");
			}
		}

		[TestFixture]
		public class When_The_Add_Extension_Method_Is_Used_With_A_New_Dictionary
		{
			[Test]
			public void Then_The_Items_Are_Added()
			{
				IDictionary<string, string> hash = new Dictionary<string, string>();
				hash.Add(id => "goose", @class => "chicken");
				Assert.That(hash.Count, Is.EqualTo(2));
				Assert.That(hash.ContainsKey("id"), Is.True);
				Assert.That(hash.ContainsKey("class"), Is.True);
			}

			[Test]
			public void With_Null_Items_Then_Nothing_Is_Added()
			{
				IDictionary<string, string> hash = new Dictionary<string, string>();
				IDictionary<string, string> hash2 = hash.Add(null);
				Assert.That(hash.Count, Is.EqualTo(0));
				Assert.That(hash2, Is.SameAs(hash));
			}
		}

		[TestFixture]
		public class With_Two_Items_And_No_Duplicate_Keys
		{
			[Test]
			public void Then_A_Dictionary_Is_Returned()
			{
				IDictionary<string, string> hash = new Hash<string>(id => "goose", @class => "chicken");
				Assert.That(hash, Is.Not.Null);
			}

			[Test]
			public void Then_The_Dictionary_Has_A_Count_Equal_To_2()
			{
				IDictionary<string, string> hash = new Hash<string>(id => "goose", @class => "chicken");
				Assert.That(hash.Count, Is.EqualTo(2));
			}

			[Test]
			public void Then_The_Dictionary_Contains_Both_Items_With_Keys()
			{
				IDictionary<string, string> hash = new Hash<string>(id => "goose", @class => "chicken");
				Assert.That(hash["id"], Is.EqualTo("goose"));
				Assert.That(hash["class"], Is.EqualTo("chicken"));
			}
		}

		[TestFixture]
		public class When_the_ConvertObjectToCaseSensitiveDictionary_method_is_called
		{
			[Test]
			public void Then_a_dictionary_should_be_created_containing_the_properties_of_the_object()
			{
				var dict = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(new {Foo = "Bar", Baz = 1});
				Assert.That(dict.Count, Is.EqualTo(2));
				Assert.That(dict["Foo"], Is.EqualTo("Bar"));
				Assert.That(dict["Baz"], Is.EqualTo(1));
			}

			[Test]
			public void Then_the_dictionary_that_is_created_should_be_case_sensitive()
			{
				var dict = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(new {Foo = "Bar"});
				Assert.That(dict.ContainsKey("Foo"));
				Assert.That(! dict.ContainsKey("foo"));
			}

			[Test]
			public void And_the_object_is_null_then_an_empty_dictionary_should_be_returned()
			{
				var dict = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(null);
				Assert.That(dict, Is.Not.Null);
				Assert.That(dict.Count, Is.EqualTo(0));
			}
		}
	}
}
