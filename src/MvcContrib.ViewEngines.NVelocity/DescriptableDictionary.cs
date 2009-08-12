using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MvcContrib.ViewEngines
{
	[Serializable]
	public class DescriptableDictionary : Hashtable, ICustomTypeDescriptor
	{
		public DescriptableDictionary(IDictionary d)
			: base(d)
		{
		}

		public DescriptableDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			var properties = new PropertyDescriptor[Count];

			int i = 0;
			foreach(DictionaryEntry entry in this)
			{
				properties[i++] = new DictionaryPropertyDescriptor(this, entry.Key);
			}

			return new PropertyDescriptorCollection(properties);
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(new Attribute[0]);
		}

		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		private class DictionaryPropertyDescriptor : PropertyDescriptor
		{
			private readonly IDictionary _dictionary;
			private readonly object _key;

			public DictionaryPropertyDescriptor(IDictionary dictionary, object key)
				: base(key.ToString(), null)
			{
				_dictionary = dictionary;
				_key = key;
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override object GetValue(object component)
			{
				return _dictionary[_key];
			}

			public override void ResetValue(object component)
			{
			}

			public override void SetValue(object component, object value)
			{
				_dictionary[_key] = value;
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			public override Type ComponentType
			{
				get { return null; }
			}

			public override bool IsReadOnly
			{
				get { return false; }
			}

			public override Type PropertyType
			{
				get { return _dictionary[_key].GetType(); }
			}
		}
	}
}