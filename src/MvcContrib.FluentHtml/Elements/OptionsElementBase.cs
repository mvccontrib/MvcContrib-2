using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class OptionsElementBase<T> : FormElement<T> where T : OptionsElementBase<T>
	{
		protected OptionsElementBase(string tag, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) 
			: base(tag, name, forMember, behaviors) { }

		protected OptionsElementBase(string tag, string name) : base(tag, name) { }

		protected IEnumerable _options;
		protected Func<object, object> _textFieldSelector;
		protected Func<object, object> _valueFieldSelector;
		protected IEnumerable _selectedValues;

		/// <summary>
		/// The selected values.
		/// </summary>
		public IEnumerable SelectedValues
		{
			get { return _selectedValues; }
		}

		public virtual T Options(MultiSelectList value)
		{
			if (value != null)
			{
				_options = value.Items;
				SetFieldExpressions(value.DataValueField, value.DataTextField);
				if(value.SelectedValues != null)
				{
					_selectedValues = value.SelectedValues;
				}
			}
			return (T)this;
		}

		public virtual T Options<TKey, TValue>(IDictionary<TKey, TValue> value)
		{
			_options = value;
			SetFieldExpressions("Key", "Value");
			return (T)this;
		}

		public virtual T Options(IEnumerable<SelectListItem> value)
		{
			_options = value;
			SetFieldExpressions("Value", "Text");
			if (value != null)
			{
				_selectedValues = value.Where(x => x.Selected).Select(x => x.Value).ToList();
			}
			return (T)this;
		}

		public virtual T Options(IEnumerable values, string valueField, string textField)
		{
			_options = values;
			SetFieldExpressions(valueField, textField);
			return (T)this;
		}

		public virtual T Options<TValue>(IEnumerable<TValue> value)
		{
			return Options(value.ToDictionary(x => x, x => x));
		}

		public virtual T Options<TDataSource>(IEnumerable<TDataSource> values, Func<TDataSource, object> valueFieldSelector, Func<TDataSource, object> textFieldSelector)
		{
			if (valueFieldSelector == null) throw new ArgumentNullException("valueFieldSelector");
			if (textFieldSelector == null) throw new ArgumentNullException("textFieldSelector");

			_options = values;
			_textFieldSelector = x => textFieldSelector((TDataSource)x);
			_valueFieldSelector = x => valueFieldSelector((TDataSource)x);
			return (T)this;
		}

		public virtual T Options<TEnum>() where TEnum : struct
		{
			if(!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("The generic parameter must be an enum", "TEnum");
			}

			var dict = new Dictionary<string, string>();

			var values = Enum.GetValues(typeof(TEnum));
	
			foreach(var item in values)
			{
				dict.Add(Convert.ToInt32(item).ToString(), item.ToString());
			}

			return Options(dict);
		}

		protected void SetFieldExpressions(string dataValueField, string dataTextField)
		{
			if (dataValueField == null) throw new ArgumentNullException("dataValueField");
			if (dataTextField == null) throw new ArgumentNullException("dataTextField");

			var enumerator = _options.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return;
			}
			var type = enumerator.Current.GetType();
			var valueProp = type.GetProperty(dataValueField);
			if (valueProp == null)
			{
				throw new ArgumentException(string.Format("The option list does not contain the specified value property: {0}", dataValueField), "dataValueField");
			}
			var textProp = type.GetProperty(dataTextField);
			if (textProp == null)
			{
				throw new ArgumentException(string.Format("The option list does not contain the specified text property: {0}", dataTextField), "dataTextField");
			}

			_textFieldSelector = x => textProp.GetValue(x, null);
			_valueFieldSelector = x => valueProp.GetValue(x, null);
		}

		protected bool IsSelectedValue(object value)
		{
			var valueString = value == null ? string.Empty : value.ToString();
			if (_selectedValues != null)
			{
				var enumerator = _selectedValues.GetEnumerator();
				while (enumerator.MoveNext())
				{
					var selectedValueString = enumerator.Current == null
						? string.Empty
						: enumerator.Current.GetType().IsEnum
							? ((int)enumerator.Current).ToString()
							: enumerator.Current.ToString();
					if (valueString == selectedValueString)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}