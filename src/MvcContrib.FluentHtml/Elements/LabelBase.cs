using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class LabelBase<T> : Element<T> where T : LabelBase<T>
	{
		protected const string labelIdSuffix = "_DetachedLabel";

		protected string forName;
		protected object rawValue;
		protected string format;
		protected string overridenId;

		protected LabelBase(string forName, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) : 
			base(HtmlTag.Label, forMember, behaviors)
		{
			this.forName = forName;
		}

		protected LabelBase(string forName) : base(HtmlTag.Label)
		{
			this.forName = forName;
		}

		protected LabelBase() : base(HtmlTag.Label) { }

		/// <summary>
		/// Set the inner text of the span element.
		/// </summary>
		/// <param name="value">The value of the inner text.</param>
		public virtual T Value(object value)
		{
			rawValue = value;
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

		public override T Id(string value)
		{
			overridenId = value;
			return (T)this;
		}

		public override string ToString()
		{
			SetId();
			
			builder.MergeAttribute(HtmlAttribute.For, forName.FormatAsHtmlId());
			builder.SetInnerText(FormatValue(rawValue));

			return base.ToString();
		}

		private void SetId()
		{
			if(!string.IsNullOrEmpty(overridenId))
			{
				base.Id(overridenId);
			}
			else
			{
				base.Id(forName.FormatAsHtmlId() + labelIdSuffix);
			}
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
	}
}
