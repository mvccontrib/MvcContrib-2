using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a radio button.
	/// </summary>
	public abstract class RadioButtonBase<T> : Input<T> where T : RadioButtonBase<T>
	{
		private string _format;
		protected RadioButtonBase(string name) : base(HtmlInputType.Radio, name) { }

		protected RadioButtonBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Radio, name, forMember, behaviors) { }

		/// <summary>
		/// Set the checked attribute.
		/// </summary>
		/// <param name="value">Whether the radio button should be checked.</param>
		public virtual T Checked(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Checked, HtmlAttribute.Checked);
			}
			else
			{
				((IElement)this).RemoveAttr(HtmlAttribute.Checked);
			}
			return (T)this;
		}

        protected override void ApplyModelState(ModelState state)
        {
            var value = state.Value.ConvertTo(typeof(string));
            var elem = elementValue.ToString();
            Checked(string.Equals(value, elem));
        }

		/// <summary>
		/// Specify a format string for the HTML output.
		/// </summary>
		/// <param name="format">A format string.</param>
		public virtual T Format(string format)
		{
			_format = format;
			return (T)this;
		}

		protected override void InferIdFromName()
		{
			if (!builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				Attr(HtmlAttribute.Id, string.Format("{0}{1}",
					builder.Attributes[HtmlAttribute.Name],
					elementValue == null 
						? null
						: string.Format("_{0}", elementValue)).FormatAsHtmlId());
			}
		}

		public override string ToString()
		{
			return _format == null
				? base.ToString()
				: string.Format(_format, base.ToString());
		}
	}
}