using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class Select : ScriptableElement
	{
		private const string ON_FOCUS = "onfocus";
		private const string ON_BLUR = "onblur";
		private const string ON_CHANGE = "onchange";
		private const string MULTIPLE = "multiple";
		private const string DISABLED = "disabled";
		private const string NAME = "name";
		private const string SIZE = "size";

		private readonly List<Option> _options = new List<Option>();
		private readonly List<string> _selectedValues = new List<string>();

		public Select(IDictionary attributes)
			: base("select", attributes)
		{
		}

		public Select()
			: this(Hash.Empty)
		{
		}

		public string Name
		{
			get { return NullGet(NAME); }
			set { NullSet(NAME, value); }
		}

		public override bool UseFullCloseTag
		{
			get { return true; }
		}

		public string OnFocus
		{
			get { return NullGet(ON_FOCUS); }
			set { NullSet(ON_FOCUS, value); }
		}

		public string OnBlur
		{
			get { return NullGet(ON_BLUR); }
			set { NullSet(ON_BLUR, value); }
		}

		public string OnChange
		{
			get { return NullGet(ON_CHANGE); }
			set { NullSet(ON_CHANGE, value); }
		}

		public bool Disabled
		{
			get { return NullGet(DISABLED) == DISABLED; }
			set
			{
				if (value)
					NullSet(DISABLED, DISABLED);
				else
					NullSet(DISABLED, null);
			}
		}

		public int Size
		{
			get
			{
				if (NullGet(SIZE) != null)
				{
					int val;
					if (int.TryParse(NullGet(SIZE), out val))
					{
						return val;
					}
					else
					{
						NullSet(SIZE, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(SIZE, value.ToString());
				}
				else
				{
					NullSet(SIZE, null);
				}
			}
		}

		public bool Multiple
		{
			get { return NullGet(MULTIPLE) == MULTIPLE; }
			set
			{
				if (value)
					NullSet(MULTIPLE, MULTIPLE);
				else
					NullSet(MULTIPLE, null);
			}
		}

		public void AddOption(string optionValue, string innerText)
		{
			var option = new Option(new Hash(value => optionValue)) {InnerText = innerText};
			_options.Add(option);
		}

		public IList<Option> Options
		{
			get { return _options; }
		}

		public string TextField { get; set; }

		public string ValueField { get; set; }

		public IList<string> SelectedValues
		{
			get { return _selectedValues.AsReadOnly(); }
		}

		public string FirstOption { get; set; }

		public string FirstOptionValue { get; set; }

		public override string ToString()
		{
			InnerText = OptionsToString();
			return base.ToString();
		}

		protected virtual string OptionsToString()
		{
			var builder = new StringBuilder();

			if (FirstOption != null)
			{
				var option = new Option {Value = FirstOptionValue, InnerText = FirstOption};

				if (SelectedValues.Contains(option.Value))
				{
					option.Selected = true;
				}

				builder.Append(option.ToString());
			}

			foreach (var option in _options)
			{
				if (SelectedValues.Contains(option.Value))
				{
					option.Selected = true;
				}

				builder.Append(option.ToString());
			}

			return builder.ToString();
		}

		public virtual void SetSelectedValues(object values)
		{
			if (values == null)
			{
				_selectedValues.Add(string.Empty);
			}
			else
			{
				//NOTE: Could reduce code here by pushing GetProperty into the ConvertValue method. Did it here to minmize reflection
				PropertyInfo prop = null;
				if (typeof(ICollection).IsAssignableFrom(values.GetType()))
				{
					var collection = (ICollection)values;
					if (!string.IsNullOrEmpty(ValueField) && collection.Count > 0)
					{
						var enumerator = collection.GetEnumerator();
						if (enumerator.MoveNext())
						{
							var type = enumerator.Current.GetType();
							prop = type.GetProperty(ValueField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
						}
					}
					foreach (var item in collection)
					{
						_selectedValues.Add(ConvertValue(item, prop));
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(ValueField))
					{
						var type = values.GetType();
						prop = type.GetProperty(ValueField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
					}
					_selectedValues.Add(ConvertValue(values, prop));
				}
			}
		}

		private string ConvertValue(object value, PropertyInfo prop)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.GetType().IsEnum)
			{
				return Convert.ToInt32(value).ToString();
			}
			return prop == null
				? value.ToString()
				: prop.GetValue(value, null).ToString();
		}
	}
}
