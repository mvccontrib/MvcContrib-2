using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for input elements.
	/// </summary>
	/// <typeparam name="T">Derived class type.</typeparam>
	public abstract class Input<T> : FormElement<T>, ISupportsModelState where T : Input<T>, IElement
	{
		protected object elementValue;

		protected Input(string type, string name) : base(HtmlTag.Input, name)
		{
			builder.MergeAttribute(HtmlAttribute.Type, type, true);
		}

		protected Input(string type, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlTag.Input, name, forMember, behaviors)
		{
			builder.MergeAttribute(HtmlAttribute.Type, type, true);
		}

		/// <summary>
		/// Set the 'value' attribute.
		/// </summary>
		/// <param name="value">The value for the attribute.</param>
		public virtual T Value(object value)
		{
			elementValue = value;
			return (T)this;
		}

		/// <summary>
		/// Set the 'size' attribute.
		/// </summary>
		/// <param name="value">The value for the attribute.</param>
		public virtual T Size(int value)
		{
			Attr(HtmlAttribute.Size, value);
			return (T)this;
		}

		protected override void PreRender()
		{
			Attr(HtmlAttribute.Value, elementValue);
			base.PreRender();
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
