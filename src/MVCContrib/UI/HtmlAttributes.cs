using System;
using System.Collections;
using System.Collections.Generic;

namespace MvcContrib.UI
{
	public class HtmlAttributes : IHtmlAttributes
	{
		private static char[] _invalidKeyCharacters = new[]
			{
				'`', '~', '!', '@', '#', '$', '^', '&', '*', '(', ')', '+', '=',
				'{', '}', '[', ']', '\t', '\r', '\n', ':', ';', '"', '\'', '<', ',', '>', '.', '?', '/'
			};

		private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>(5, StringComparer.OrdinalIgnoreCase);

		private int _estLength;
			//Track approximate size in characters of what the rendered attributes will be, this will speed up the string builder.

		private int _extraLength = 3; //Extra lenght that is added per attribute.

		public HtmlAttributes(IDictionary attribs)
		{
			foreach (DictionaryEntry de in attribs)
			{
				Add(de.Key.ToString(), de.Value.ToString());
			}
		}

		public HtmlAttributes()
		{
		}

		#region IDictionary Members

		public void Add(object key, object value)
		{
			Add(key.ToString(), value.ToString());
		}

		public bool Contains(object key)
		{
			return ContainsKey(key.ToString());
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary) _attributes).GetEnumerator();
		}

		public bool IsFixedSize
		{
			get { return ((IDictionary) _attributes).IsFixedSize; }
		}

		ICollection IDictionary.Keys
		{
			get { return ((IDictionary) _attributes).Keys; }
		}

		void IDictionary.Remove(object key)
		{
			Remove(key.ToString());
		}

		ICollection IDictionary.Values
		{
			get { return ((IDictionary) _attributes).Values; }
		}

		object IDictionary.this[object key]
		{
			get { return this[key.ToString()]; }
			set
			{
				if (value != null)
					this[key.ToString()] = value.ToString();
				else
					this[key.ToString()] = null;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		bool ICollection.IsSynchronized
		{
			get { throw new NotImplementedException(); }
		}

		object ICollection.SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IDictionary<string,string> Members

		public void Add(string key, string value)
		{
			this[key] = value;
		}

		public bool ContainsKey(string key)
		{
			return _attributes.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return _attributes.Keys; }
		}

		public bool Remove(string key)
		{
			this[key] = null;
			return true;
		}

		public bool TryGetValue(string key, out string value)
		{
			return _attributes.TryGetValue(key, out value);
		}

		public ICollection<string> Values
		{
			get { return _attributes.Values; }
		}

		public string this[string key]
		{
			get { return NullGet(key); }
			set { NullSet(key, value); }
		}

		public void Add(KeyValuePair<string, string> item)
		{
			this[item.Key] = item.Value;
		}

		public void Clear()
		{
			_estLength = 0;
			_attributes.Clear();
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return ContainsKey(item.Key);
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return _attributes.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			this[item.Key] = null;
			return true;
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return _attributes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) _attributes).GetEnumerator();
		}

		#endregion

		protected string NullGet(string key)
		{
			if (_attributes.ContainsKey(key))
			{
				return _attributes[key];
			}
			return null;
		}

		protected void NullSet(string key, string value)
		{
			if (value == null && _attributes.ContainsKey(key))
			{
				_estLength += (-1 * (key.Length + this[key].Length));
				_attributes.Remove(key);
			}
			else if (value != null)
			{
				if (_attributes.ContainsKey(key))
				{
					_estLength += (-1 * (this[key].Length));
					_attributes[key] = value;
					_estLength += this[key].Length;
				}
				else
				{
					_attributes.Add(key, value);
					_estLength += (key.Length + value.Length);
				}
			}
		}

		public int GetEstLength()
		{
			return _estLength + (_extraLength * _attributes.Count);
		}
	}
}
