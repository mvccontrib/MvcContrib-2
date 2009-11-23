using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MvcContrib.Attributes;
using NUnit.Framework;

namespace MvcContrib.UnitTests
{
	[Obsolete("Consider using System.Web.Mvc.DefaultModelBinder instead.")]
	[TestFixture]
	public class NameValueDeserializerTester
	{
		private NameValueCollection collection;
		private NameValueDeserializer nvd;

		[SetUp]
		public void Setup()
		{
			collection = new NameValueCollection();
			nvd = new NameValueDeserializer();
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyPrefixThrows()
		{
			collection["junk"] = "stuff";

			nvd.Deserialize(collection, string.Empty, typeof(Customer));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullTargetTypeThrows()
		{
			collection["junk"] = "stuff";

			nvd.Deserialize(collection, "junk", null);
		}

		[Test]
		public void ListPropertyIsSkippedIfNotInitializedAndReadOnly()
		{
			collection["list.ReadonlyIds[0]"] = "10";
			collection["list.ReadonlyIds[1]"] = "20";

			var list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.IsNull(list.ReadonlyIds);
		}

		[Test]
		public void ErrorsSettingPropertiesAreIgnored()
		{
			collection["emp.Age"] = "-1";

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(0, emp.Age);
		}

		[Test]
		public void ComplexPropertyIsSkippedIfNotInitializedAndReadOnly()
		{
			collection["emp.BatPhone.Number"] = "800-DRK-KNGT";

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.IsNull(emp.BatPhone);
		}

		[Test]
		public void DeserializeSimpleObject()
		{
			collection["cust.Id"] = "10";

			var cust = nvd.Deserialize<Customer>(collection, "cust");

			Assert.IsNotNull(cust);
			Assert.AreEqual(10, cust.Id);
		}

		[Test]
		public void DeserializeSimpleObjectNoPrefix()
		{
			collection["Id"] = "10";

			var cust = nvd.Deserialize<Customer>(collection);

			Assert.IsNotNull(cust);
			Assert.AreEqual(10, cust.Id);
		}

		[Test]
		public void DeserializeSimpleArray()
		{
			collection["array.Ids[0]"] = "10";
			collection["array.Ids[1]"] = "20";

			var array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleArrayNoPrefix()
		{
			collection["Ids[0]"] = "10";
			collection["Ids[1]"] = "20";

			var array = nvd.Deserialize<ArrayClass>(collection);

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleArrayFromMultiple()
		{
			collection.Add("array.ids", "10");
			collection.Add("array.ids", "20");

			var array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleArrayFromMultipleNoPrefix()
		{
			collection.Add("ids", "10");
			collection.Add("ids", "20");

			var array = nvd.Deserialize<ArrayClass>(collection);

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializePrimitiveArray()
		{
			collection["ids[0]"] = "10";
			collection["ids[1]"] = "20";

			var array = (int[])nvd.Deserialize(collection, "ids", typeof(int[]));

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Length);
			Assert.AreEqual(10, array[0]);
			Assert.AreEqual(20, array[1]);
		}

		[Test]
		public void DeserializePrimitiveArrayFromMultiple()
		{
			collection.Add("ids", "10");
			collection.Add("ids", "20");

			var array = (int[])nvd.Deserialize(collection, "ids", typeof(int[]));

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Length);
			Assert.AreEqual(10, array[0]);
			Assert.AreEqual(20, array[1]);
		}

		[Test]
		public void DeserializePrimitiveGenericList()
		{
			collection["ids[0]"] = "10";
			collection["ids[1]"] = "20";

			var list = nvd.Deserialize<List<int>>(collection, "ids");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
		}

		[Test]
		public void DeserializePrimitiveGenericListFromMultiple()
		{
			collection.Add("ids", "10");
			collection.Add("ids", "20");

			var list = nvd.Deserialize<List<int>>(collection, "ids");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
		}

		[Test]
		public void DeserializeEnumGenericListFromMultiple()
		{
			collection.Add("testEnum", "0");
			collection.Add("testEnum", "2");

			var list = nvd.Deserialize<List<TestEnum>>(collection, "testEnum");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(TestEnum.One, list[0]);
			Assert.AreEqual(TestEnum.Three, list[1]);
		}

		[Test]
		public void DeserializeSimpleGenericList()
		{
			collection["list.Ids[0]"] = "10";
			collection["list.Ids[1]"] = "20";

			var list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleGenericListNoPrefix()
		{
			collection["Ids[0]"] = "10";
			collection["Ids[1]"] = "20";

			var list = nvd.Deserialize<GenericListClass>(collection);

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleGenericListFromMultiple()
		{
			collection.Add("list.Ids", "10");
			collection.Add("list.Ids", "20");

			var list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleGenericListFromMultipleNoPrefix()
		{
			collection.Add("Ids", "10");
			collection.Add("Ids", "20");

			var list = nvd.Deserialize<GenericListClass>(collection);

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeComplexGenericList()
		{
			collection["emp.OtherPhones[0].Number"] = "800-555-1212";
			collection["emp.OtherPhones[1].Number"] = "800-867-5309";
			collection["emp.OtherPhones[1].AreaCodes[0]"] = "800";
			collection["emp.OtherPhones[1].AreaCodes[1]"] = "877";

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(2, emp.OtherPhones.Count);
			Assert.AreEqual("800-555-1212", emp.OtherPhones[0].Number);
			Assert.AreEqual("800-867-5309", emp.OtherPhones[1].Number);
			Assert.AreEqual(2, emp.OtherPhones[1].AreaCodes.Count);
			Assert.AreEqual("800", emp.OtherPhones[1].AreaCodes[0]);
			Assert.AreEqual("877", emp.OtherPhones[1].AreaCodes[1]);
		}

		[Test]
		public void DeserializeComplexGenericListNoPrefix()
		{
			collection["OtherPhones[0].Number"] = "800-555-1212";
			collection["OtherPhones[1].Number"] = "800-867-5309";
			collection["OtherPhones[1].AreaCodes[0]"] = "800";
			collection["OtherPhones[1].AreaCodes[1]"] = "877";

			var emp = nvd.Deserialize<Employee>(collection);

			Assert.IsNotNull(emp);
			Assert.AreEqual(2, emp.OtherPhones.Count);
			Assert.AreEqual("800-555-1212", emp.OtherPhones[0].Number);
			Assert.AreEqual("800-867-5309", emp.OtherPhones[1].Number);
			Assert.AreEqual(2, emp.OtherPhones[1].AreaCodes.Count);
			Assert.AreEqual("800", emp.OtherPhones[1].AreaCodes[0]);
			Assert.AreEqual("877", emp.OtherPhones[1].AreaCodes[1]);
		}

		[Test]
		public void DeserializeWithEmptyArray()
		{
			collection["array.Name"] = "test";

			var array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(0, array.Ids.Length);
		}

		[Test]
		public void DeserializeWithEmptyArrayNoPrefix()
		{
			collection["Name"] = "test";

			var array = nvd.Deserialize<ArrayClass>(collection);

			Assert.IsNotNull(array);
			Assert.AreEqual(0, array.Ids.Length);
		}

		[Test]
		public void DeserializeComplexObject()
		{
			collection["emp.Id"] = "20";
			collection["emp.Phone.Number"] = "800-555-1212";

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(20, emp.Id);
			Assert.AreEqual("800-555-1212", emp.Phone.Number);
		}

		[Test]
		public void DeserializeComplexObjectNoPrefix()
		{
			collection["Id"] = "20";
			collection["Phone.Number"] = "800-555-1212";

			var emp = nvd.Deserialize<Employee>(collection);

			Assert.IsNotNull(emp);
			Assert.AreEqual(20, emp.Id);
			Assert.AreEqual("800-555-1212", emp.Phone.Number);
		}

		[Test]
		public void EmptyValuesUseDefaultOfType()
		{
			collection["cust.Id"] = "";

			var cust = nvd.Deserialize<Customer>(collection, "cust");

			Assert.IsNotNull(cust);
			Assert.AreEqual(0, cust.Id);
		}

		[Test]
		public void EmptyValuesUseDefaultOfTypeNoPrefix()
		{
			collection["Id"] = "";

			var cust = nvd.Deserialize<Customer>(collection);

			Assert.IsNotNull(cust);
			Assert.AreEqual(0, cust.Id);
		}

		[Test]
		public void NoMatchingValuesReturnsNewedObjectNoPrefix()
		{
			collection["Ip"] = "10";

			var cust = nvd.Deserialize<Customer>(collection);

			Assert.IsNotNull(cust);
		}

		[Test]
		public void DeserializeTrueBool()
		{
			collection["bool.myBool"] = "true,false";

			var boolClass = nvd.Deserialize<BoolClass>(collection, "bool");

			Assert.AreEqual(true, boolClass.MyBool);
		}

		[Test]
		public void DeserializeTrueBoolNoPrefix()
		{
			collection["myBool"] = "true,false";

			var boolClass = nvd.Deserialize<BoolClass>(collection);

			Assert.AreEqual(true, boolClass.MyBool);
		}

		[Test]
		public void DeserializeFalseBool()
		{
			collection["bool.myBool"] = "false";

			var boolClass = nvd.Deserialize<BoolClass>(collection, "bool");

			Assert.AreEqual(false, boolClass.MyBool);
		}

		[Test]
		public void DeserializeFalseBoolNoPrefix()
		{
			collection["myBool"] = "false";

			var boolClass = nvd.Deserialize<BoolClass>(collection);

			Assert.AreEqual(false, boolClass.MyBool);
		}

		[Test]
		public void EmptyCollectionReturnsNull()
		{
			var cust = nvd.Deserialize<Customer>(null, "cust");

			Assert.IsNull(cust);
		}

		[Test]
		public void EmptyCollectionReturnsNullNoPrefix()
		{
			var cust = nvd.Deserialize<Customer>(null);

			Assert.IsNull(cust);
		}

		[Test]
		public void ForCompleteness()
		{
			var attribute = new DeserializeAttribute("test");

			Assert.AreEqual("test", attribute.Prefix);
		}

		[Test]
		public void ForCompletenessNoPrefix()
		{
			var attribute = new DeserializeAttribute();

			Assert.IsNull(attribute.Prefix);
		}

		[Test]
		public void NoRequestForPropertyShouldNotInstantiateProperty()
		{
			collection["emp.Id"] = "20";
			collection["emp.Phone.Number"] = "800-555-1212";

			var deserializer = new NameValueDeserializer();
			var emp = deserializer.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp, "Employee should not be null.");
			Assert.IsNotNull(emp.Phone, "Employee phone should not be null.");
			Assert.IsNull(emp.BatPhone, "Employee OtherPhones should be null because it was not in request parameters.");
		}

		[Test]
		public void NoRequestForPropertyShouldNotInstantiatePropertyNoPrefix()
		{
			collection["Id"] = "20";
			collection["Phone.Number"] = "800-555-1212";

			var deserializer = new NameValueDeserializer();
			var emp = deserializer.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp, "Employee should not be null.");
			Assert.IsNotNull(emp.Phone, "Employee phone should not be null.");
			Assert.IsNull(emp.BatPhone, "Employee OtherPhones should be null because it was not in request parameters.");
		}

		[Test]
		public void ShouldNotThrowWithNullValues()
		{
			collection[null] = null;

			var deserializer = new NameValueDeserializer();

			deserializer.Deserialize(collection, "cust", typeof(Customer));
		}

		[Test]
		public void ShouldNotThrowWithNullValuesNoPrefix()
		{
			collection[null] = null;

			var deserializer = new NameValueDeserializer();

			deserializer.Deserialize(collection, typeof(Customer));
		}

		public class Customer
		{
			public int Id { get; set; }

			public Employee Employee { get; set; }
		}

		public class ArrayClass
		{
			public string Name { get; set; }

			public int[] Ids { get; set; }
		}

		public class BoolClass
		{
			public bool MyBool { get; set; }
		}

		public class GenericListClass
		{
			public string Name { get; set; }

			public IList<int> Ids { get; set; }

			private const IList<int> _readonlyIds = null;

			public IList<int> ReadonlyIds
			{
				get { return _readonlyIds; }
			}
		}

		public class Employee
		{
			private readonly Phone _phone = new Phone();
			private readonly List<Phone> _otherPhones = new List<Phone>();

			public int Id { get; set; }

			public Phone Phone
			{
				get { return _phone; }
			}

			private const Phone _batPhone = null;

			public Phone BatPhone
			{
				get { return _batPhone; }
			}

			public IList<Phone> OtherPhones
			{
				get { return _otherPhones; }
			}

			public Customer Customer { get; set; }

			private int _age;

			public int Age
			{
				get { return _age; }
				set
				{
					if(value < 0) throw new ArgumentException("Age must be greater than 0");
					_age = value;
				}
			}
		}

		public class Phone
		{
			private readonly List<string> _areaCodes = new List<string>();

			public string Number { get; set; }

			public IList<string> AreaCodes
			{
				get { return _areaCodes; }
			}
		}

		public enum TestEnum
		{
			One,
			Two,
			Three
		}
	}
}