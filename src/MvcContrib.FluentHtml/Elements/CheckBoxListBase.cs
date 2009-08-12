using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a list of checkboxes.
	/// </summary>
	public abstract class CheckBoxListBase<T> : OptionsElementBase<T> where T : CheckBoxListBase<T>
	{
		protected string _itemFormat;
		protected string _itemClass;

		protected CheckBoxListBase(string tag, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(tag, name, forMember, behaviors) { }

		protected CheckBoxListBase(string tag, string name) : base(tag, name) { }

		/// <summary>
		/// Set the selected values.
		/// </summary>
		/// <param name="selectedValues">Values matching the values of options to be selected.</param>
		public virtual T Selected(IEnumerable selectedValues)
		{
			_selectedValues = selectedValues;
			return (T)this;
		}

		/// <summary>
		/// Specify a format string for the HTML of each checkbox button and label.
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual T ItemFormat(string value)
		{
			_itemFormat = value;
			return (T)this;
		}

		/// <summary>
		/// Specify the class for the input and label elements of each item.
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual T ItemClass(string value)
		{
			_itemClass = value;
			return (T)this;
		}

		protected override void PreRender()
		{
			builder.InnerHtml = RenderBody();
			base.PreRender();
		}

		protected override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		private string RenderBody()
		{
			if (_options == null)
			{
				return null;
			}

			var name = builder.Attributes[HtmlAttribute.Name];
			builder.Attributes.Remove(HtmlAttribute.Name);
			var sb = new StringBuilder();
			var i = 0;
			foreach (var option in _options)
			{
				var value = _valueFieldSelector(option);
				var checkbox = (new CheckBox(name, forMember, behaviors)
					.Id(string.Format("{0}_{1}", name.FormatAsHtmlId(), i))
					.Value(value))
					.LabelAfter(_textFieldSelector(option).ToString(), _itemClass)
					.Checked(IsSelectedValue(value));
				if (_itemClass != null)
				{
					checkbox.Class(_itemClass);
				}
				sb.Append(_itemFormat == null
					? checkbox.ToCheckBoxOnlyHtml()
					: string.Format(_itemFormat, checkbox.ToCheckBoxOnlyHtml()));
				i++;
			}
			return sb.ToString();
		}
	}
}