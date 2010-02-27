using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MvcContrib
{
	/// <summary>
	/// Provides the ability to deserialize a Request into a CLR object.
	/// </summary>
	[Obsolete("Please switch to using MVC's DefaultModelBinder for model binding.")]
	public class NameValueDeserializer
	{
		protected virtual IConvertible GetConvertible(string sValue)
		{
			return new DefaultConvertible(sValue);
		}

		/// <summary>
		/// Deserializes the specified request collection.
		/// </summary>
		/// <typeparam name="T">The type to deserialize to.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="prefix">The prefix.</param>
		/// <returns>The deserialized object</returns>
		public T Deserialize<T>(NameValueCollection collection, string prefix) where T : new()
		{
			return (T)Deserialize(collection, prefix, typeof(T));
		}

		/// <summary>
		/// Deserializes the specified request collection.
		/// </summary>
		/// <typeparam name="T">The type to deserialize to.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>The deserialized object</returns>
		public T Deserialize<T>(NameValueCollection collection) where T : new()
		{
			return (T)Deserialize(collection, null, typeof(T));
		}

		/// <summary>
		/// Deserializes the specified request collection.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <returns>The deserialized object</returns>
		public object Deserialize(NameValueCollection collection, Type targetType)
		{
			return Deserialize(collection, null, targetType);
		}

		/// <summary>
		/// Deserializes the specified request collection.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="prefix">The prefix.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <returns>The deserialized object</returns>
		public object Deserialize(NameValueCollection collection, string prefix, Type targetType)
		{
			if(collection == null || collection.Count == 0) return null;

			if(prefix == string.Empty) throw new ArgumentException("prefix must not be empty", prefix);

			if(targetType == null) throw new ArgumentNullException("targetType");

			if(targetType.IsArray)
			{
				Type elementType = targetType.GetElementType();
				ArrayList arrayInstance = DeserializeArrayList(collection, prefix, elementType);
				return arrayInstance.ToArray(elementType);
			}

			if(IsGenericList(targetType))
			{
				IList genericListInstance = CreateGenericListInstance(targetType);
				DeserializeGenericList(collection, prefix, targetType, ref genericListInstance);
				return genericListInstance;
			}

			object instance = null;
			Deserialize(collection, prefix, targetType, ref instance);
			return instance ?? CreateInstance(targetType);
		}

		protected virtual void Deserialize(NameValueCollection collection, string prefix, Type targetType, ref object instance)
		{
			if(CheckPrefixInRequest(collection, prefix))
			{
				if(instance == null)
				{
					instance = CreateInstance(targetType);
				}

				PropertyInfo[] properties = GetProperties(targetType);

				foreach(var property in properties)
				{
					string name = prefix != null ? string.Concat(prefix, ".", property.Name) : property.Name;
					Type propertyType = property.PropertyType;

					if(IsSimpleProperty(propertyType))
					{
						string sValue = collection.Get(name);
						if(sValue != null)
						{
							SetValue(instance, property, GetConvertible(sValue));
						}
					}
					else if(propertyType.IsArray)
					{
						Type elementType = propertyType.GetElementType();

						ArrayList arrayInstance = DeserializeArrayList(collection, name, elementType);

						SetValue(instance, property, arrayInstance.ToArray(elementType));
					}
					else if(IsGenericList(propertyType))
					{
						IList genericListProperty = GetGenericListProperty(instance, property);

						if(genericListProperty == null) continue;

						DeserializeGenericList(collection, name, propertyType, ref genericListProperty);
					}
					else
					{
						object complexProperty = GetComplexProperty(instance, property);

						if(complexProperty == null) continue;

						Deserialize(collection, name, propertyType, ref complexProperty);
					}
				}
			}
		}

		protected virtual void DeserializeGenericList(NameValueCollection collection, string prefix, Type targetType,
		                                              ref IList instance)
		{
			Type elementType = targetType.GetGenericArguments()[0];

			ArrayList arrayInstance = DeserializeArrayList(collection, prefix, elementType);

			foreach(var inst in arrayInstance)
			{
				instance.Add(inst);
			}
		}

		protected virtual IList GetGenericListProperty(object instance, PropertyInfo property)
		{
			// If property is already initialized (via object's constructor) use that
			// Otherwise attempt to new it
			var genericListProperty = property.GetValue(instance, null) as IList;
			if(genericListProperty == null)
			{
				genericListProperty = CreateGenericListInstance(property.PropertyType);
				if(!SetValue(instance, property, genericListProperty))
				{
					return null;
				}
			}
			return genericListProperty;
		}

		protected virtual IList CreateGenericListInstance(Type targetType)
		{
			Type elementType = targetType.GetGenericArguments()[0];

			Type desiredListType = targetType.IsInterface
			                       	? typeof(List<>).MakeGenericType(elementType)
			                       	: targetType;

			return CreateInstance(desiredListType) as IList;
		}

		protected virtual bool IsGenericList(Type instanceType)
		{
			if(!instanceType.IsGenericType)
			{
				return false;
			}

			if(typeof(IList).IsAssignableFrom(instanceType))
			{
				return true;
			}

			Type[] genericArgs = instanceType.GetGenericArguments();

			Type listType = typeof(IList<>).MakeGenericType(genericArgs[0]);

			return listType.IsAssignableFrom(instanceType);
		}

		protected virtual ArrayList DeserializeArrayList(NameValueCollection collection, string prefix, Type elementType)
		{
			ArrayList arrayInstance;

			if(IsSimpleProperty(elementType))
			{
				string[] sValueArray = collection.GetValues(prefix);
				if(sValueArray != null)
				{
					arrayInstance = new ArrayList(sValueArray.Length);
					foreach(var item in sValueArray)
					{
						var inst = GetConvertible(item).ToType(elementType, CultureInfo.CurrentCulture);
						if(inst != null)
						{
							arrayInstance.Add(inst);
						}
					}
					return arrayInstance;
				}
			}

			string[] arrayPrefixes = GetArrayPrefixes(collection, prefix);

			arrayInstance = new ArrayList(arrayPrefixes.Length);

			foreach(var arrayPrefix in arrayPrefixes)
			{
				object inst = null;

				if(IsSimpleProperty(elementType))
				{
					string sValue = collection.Get(arrayPrefix);
					if(sValue != null)
					{
						inst = GetConvertible(sValue).ToType(elementType, CultureInfo.CurrentCulture);
					}
				}
				else
				{
					inst = Deserialize(collection, arrayPrefix, elementType);
				}

				if(inst != null)
				{
					arrayInstance.Add(inst);
				}
			}

			return arrayInstance;
		}

		protected virtual bool CheckPrefixInRequest(NameValueCollection collection, string prefix)
		{
			return prefix != null
			       	? collection.AllKeys.Any(key => key != null && key.StartsWith(prefix, true, CultureInfo.InvariantCulture))
			       	: true;
		}

		protected virtual string[] GetArrayPrefixes(NameValueCollection collection, string prefix)
		{
			var arrayPrefixes = new List<string>();

			prefix = string.Concat(prefix, "[").ToLower();
			int prefixLength = prefix.Length;
			string[] names = collection.AllKeys;
			foreach(var name in names)
			{
				if(name.IndexOf(prefix, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					int bracketIndex = name.IndexOf(']', prefixLength);
					if(bracketIndex > prefixLength)
					{
						string arrayPrefix = name.Substring(0, bracketIndex + 1).ToLower();
						if(!arrayPrefixes.Contains(arrayPrefix))
						{
							arrayPrefixes.Add(arrayPrefix);
						}
					}
				}
			}

			return arrayPrefixes.ToArray();
		}

		private static readonly Dictionary<Type, PropertyInfo[]> _cachedProperties = new Dictionary<Type, PropertyInfo[]>();

		private static readonly object _syncRoot = new object();

		protected static PropertyInfo[] GetProperties(Type targetType)
		{
			PropertyInfo[] properties;
			if(!_cachedProperties.TryGetValue(targetType, out properties))
			{
				lock(_syncRoot)
				{
					if(!_cachedProperties.TryGetValue(targetType, out properties))
					{
						properties = targetType.GetProperties();
						_cachedProperties.Add(targetType, properties);
					}
				}
			}
			return properties;
		}

		protected virtual bool SetValue(object instance, PropertyInfo property, object value)
		{
			if(property.CanWrite)
			{
				property.SetValue(instance, value, null);
				return true;
			}

			return false;
		}

		protected virtual bool SetValue(object instance, PropertyInfo property, IConvertible oValue)
		{
			try
			{
				object convertedValue = oValue.ToType(property.PropertyType, CultureInfo.CurrentCulture);
				return SetValue(instance, property, convertedValue);
			}
			catch
			{
				return false;
			}
		}

		protected virtual object CreateInstance(Type targetType)
		{
			return Activator.CreateInstance(targetType);
		}

		protected virtual object GetComplexProperty(object instance, PropertyInfo property)
		{
			// If property is already initialized (via object's constructor) use that
			// Otherwise attempt to new it
			object complexProperty = property.GetValue(instance, null);
			if(complexProperty == null)
			{
				complexProperty = CreateInstance(property.PropertyType);
				if(!SetValue(instance, property, complexProperty))
				{
					return null;
				}
			}
			return complexProperty;
		}

		protected virtual bool IsSimpleProperty(Type propertyType)
		{
			if(propertyType.IsArray)
			{
				return false;
			}

			bool isSimple = propertyType.IsPrimitive ||
			                propertyType.IsEnum ||
			                propertyType == typeof(String) ||
			                propertyType == typeof(Guid) ||
			                propertyType == typeof(DateTime) ||
			                propertyType == typeof(Decimal);

			if(isSimple)
			{
				return true;
			}

			TypeConverter tconverter = TypeDescriptor.GetConverter(propertyType);

			return tconverter.CanConvertFrom(typeof(String));
		}
	}
}