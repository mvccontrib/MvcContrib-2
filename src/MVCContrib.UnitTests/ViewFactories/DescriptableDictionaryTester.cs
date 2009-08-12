using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using MvcContrib.ViewEngines;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, NUnit.Framework.Category("NVelocityViewEngine")]
	public class DescriptableDictionaryTester
	{
		[Test]
		public void Creates_One_Property_For_Each_Dictionary_Entry()
		{
			var hash = new Hashtable();
			hash["Prop1"] = 1;
			hash["Prop2"] = "a";

			var dict = new DescriptableDictionary(hash);

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dict);
			Assert.AreEqual(2, properties.Count);
			Assert.AreEqual(1, properties[0].GetValue(dict));
			Assert.AreEqual("a", properties[1].GetValue(dict));
		}

		[Test]
		public void ForCoverage()
		{
			var hash = new Hashtable();
			hash["Prop1"] = 1;
			var dict = new DescriptableDictionary(hash);
			PropertyDescriptor property = TypeDescriptor.GetProperties(dict)[0];

			property.CanResetValue(null);
			var componentType = property.ComponentType;
			var isReadOnly = property.IsReadOnly;
			var propertyType = property.PropertyType;
			property.ResetValue(null);
			property.SetValue(null, null);
			property.ShouldSerializeValue(null);

			dict = new DescriptableDictionary(new SerializationInfo(typeof(Hashtable), new FormatterConverter()), new StreamingContext());
			dict.GetAttributes();
			dict.GetClassName();
			dict.GetComponentName();
			dict.GetConverter();
			dict.GetDefaultEvent();
			dict.GetDefaultProperty();
			dict.GetEditor(typeof(string));
			dict.GetEvents();
			dict.GetEvents(new Attribute[] {});
			dict.GetPropertyOwner(null);
		}
	}
}
