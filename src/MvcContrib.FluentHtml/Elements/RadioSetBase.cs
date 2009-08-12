using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a set of radio buttons.
	/// </summary>
	public abstract class RadioSetBase<T> : OptionsElementBase<T> where T : RadioSetBase<T>
	{
		protected string _format;
		protected string _itemClass;

		protected RadioSetBase(string tag, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(tag, name, forMember, behaviors) { }

		protected RadioSetBase(string tag, string name) : base(tag, name) { }

		/// <summary>
		/// Set the selected option.
		/// </summary>
		/// <param name="selectedValue">A value matching the option to be selected.</param>
		/// <returns></returns>
		public virtual T Selected(object selectedValue)
		{
			_selectedValues = new List<object> { selectedValue };
			return (T)this;
		}

		/// <summary>
		/// Specify a format string for the HTML of each radio button and label.
		/// </summary>
		/// <param name="format">A format string.</param>
		public virtual T ItemFormat(string format)
		{
			_format = format;
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
			foreach (var option in _options)
			{
				var value = _valueFieldSelector(option);
				var radioButton = (new RadioButton(name, forMember, behaviors)
					.Value(value)
					.Format(_format))
					.LabelAfter(_textFieldSelector(option).ToString(), _itemClass)
					.Checked(IsSelectedValue(value));
				if (_itemClass != null)
				{
					radioButton.Class(_itemClass);
				}
				sb.Append(radioButton);
			}
			return sb.ToString();
		}
	}
}