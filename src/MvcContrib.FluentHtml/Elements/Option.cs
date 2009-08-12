using System.Web.Mvc;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a select option.
	/// </summary>
	public class Option
	{
		protected readonly TagBuilder builder;

		/// <summary>
		/// Generate a select option.
		/// </summary>
		public Option()
		{
			builder = new TagBuilder(HtmlTag.Option);
		}

		/// <summary>
		/// Set the value of the 'value' attribute.
		/// </summary>
		/// <param name="value">The value.</param>
		public Option Value(string value) 
		{
			builder.MergeAttribute(HtmlAttribute.Value, value, true);
			return this;
		}

		/// <summary>
		/// Set the inner text.
		/// </summary>
		/// <param name="value">The value of the inner text.</param>
		public virtual Option Text(string value)
		{
			builder.SetInnerText(value);
			return this;
		}

		/// <summary>
		/// Set the selected attribute.
		/// </summary>
		/// <param name="value">Whether the option should be selected.</param>
		public virtual Option Selected(bool value)
		{
			if (value)
			{
				builder.MergeAttribute(HtmlAttribute.Selected, HtmlAttribute.Selected, true);
			}
			else
			{
				builder.Attributes.Remove(HtmlAttribute.Selected);
			}
			return this;
		}

		/// <summary>
		/// Set as disabled or enabled.
		/// </summary>
		/// <param name="value">Whether the option is disabled.</param>
		public virtual Option Disabled(bool value)
		{
			if (value)
			{
				builder.MergeAttribute(HtmlAttribute.Disabled, HtmlAttribute.Disabled, true);
			}
			else
			{
				builder.Attributes.Remove(HtmlAttribute.Disabled);
			}
			return this;
		}

		public override string ToString()
		{
			return builder.ToString();
		}
	}
}
