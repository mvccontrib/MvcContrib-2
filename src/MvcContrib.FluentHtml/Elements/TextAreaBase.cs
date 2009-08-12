using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// base class for an HTML textarea element.
	/// </summary>
	public abstract class TextAreaBase<T> : FormElement<T>, ISupportsModelState where T : TextAreaBase<T>
	{
		protected string format;
		protected object rawValue;

		protected TextAreaBase(string name) : base(HtmlTag.TextArea, name) { }

		protected TextAreaBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlTag.TextArea, name, forMember, behaviors) { }

		/// <summary>
		/// Set the inner text.
		/// </summary>
		/// <param name="value">The value of the inner text.</param>
		public virtual T Value(object value)
		{
			rawValue = value;
			return (T)this;
		}

		/// <summary>
		/// Set the 'rows' attribute.
		/// </summary>
		/// <param name="value">The value of the rows attribute<./param>
		public virtual T Rows(int value)
		{
			Attr(HtmlAttribute.Rows, value);
			return (T)this;
		}

		/// <summary>
		/// Set the 'columns' attribute.
		/// </summary>
		/// <param name="value">The value of the columns attribute.</param>
		public virtual T Columns(int value)
		{
			Attr(HtmlAttribute.Cols, value);
			return (T)this;
		}

		/// <summary>
		/// Specify a format string to be applied to the value.  The format string can be either a
		/// specification (e.g., '$#,##0.00') or a placeholder (e.g., '{0:$#,##0.00}').
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual T Format(string value)
		{
			format = value;
			return (T)this;
		}

		protected override void PreRender()
		{
			if (!(rawValue is string) && rawValue is IEnumerable)
			{
				var items = new List<string>();
				foreach (var item in (IEnumerable)rawValue)
				{
					items.Add(FormatValue(item));
				}
				builder.SetInnerText(string.Join(Environment.NewLine, items.ToArray()));
			}
			else
			{
				builder.SetInnerText(FormatValue(rawValue));
			}
			base.PreRender();
		}

		protected virtual string FormatValue(object value)
		{
			return string.IsNullOrEmpty(format)
					   ? value == null 
							 ? null 
							 : value.ToString()
					   : (format.StartsWith("{0") && format.EndsWith("}"))
							 ? string.Format(format, value)
							 : string.Format("{0:" + format + "}", value);
		}

		protected override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		void ISupportsModelState.ApplyModelState(ModelState state) 
		{
			ApplyModelState(state);
		}

		protected virtual void ApplyModelState(ModelState state) 
		{
			var value = state.Value.ConvertTo(typeof(string));
			Value(value);
		}
	}
}