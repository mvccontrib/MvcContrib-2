using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML elements.
	/// </summary>
	/// <typeparam name="T">The derived class type.</typeparam>
	public abstract class Element<T> : IMemberElement where T : Element<T>, IElement
	{
		protected const string LABEL_FORMAT = "{0}_Label";

		protected readonly TagBuilder builder;
		protected MemberExpression forMember;
		protected IEnumerable<IBehaviorMarker> behaviors;

		protected Element(string tag, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) : this(tag)
		{
			this.forMember = forMember;
			this.behaviors = behaviors;
		}

		protected Element(string tag)
		{
			builder = new TagBuilder(tag);
		}

		/// <summary>
		/// TagBuilder object used to generate HTML for elements.
		/// </summary>
		TagBuilder IElement.Builder
		{
			get { return builder; }
		}

		/// <summary>
		/// Set the 'id' attribute.
		/// </summary>
		/// <param name="value">The value of the 'id' attribute.</param>
		public virtual T Id(string value)
		{
			builder.MergeAttribute(HtmlAttribute.Id, value, true);
			return (T)this;
		}

		/// <summary>
		/// Add a value to the 'class' attribute.
		/// </summary>
		/// <param name="classToAdd">The value of the class to add.</param>
		public virtual T Class(string classToAdd)
		{
			builder.AddCssClass(classToAdd);
			return (T)this;
		}

		/// <summary>
		/// Set the 'title' attribute.
		/// </summary>
		/// <param name="value">The value of the 'title' attribute.</param>
		public virtual T Title(string value)
		{
			builder.MergeAttribute(HtmlAttribute.Title, value, true);
			return (T)this;
		}

		/// <summary>
		/// Set the 'style' attribute.
		/// </summary>
		/// <param name="values">A list of funcs, each epxressing a style name value pair.  Replace dashes with 
		/// underscores in style names. For example 'margin-top:10px;' is expressed as 'margin_top => "10px"'.</param>
		public virtual T Styles(params Func<string, string>[] values)
		{
			var sb = new StringBuilder();
			foreach (var func in values)
			{
				sb.AppendFormat("{0}:{1};", func.Method.GetParameters()[0].Name.Replace('_', '-'), func(null));
			}
			builder.MergeAttribute(HtmlAttribute.Style, sb.ToString());
			return (T)this;
		}

		/// <summary>
		/// Set the 'onclick' attribute.
		/// </summary>
		/// <param name="value">The value for the attribute.</param>
		/// <returns></returns>
		public virtual T OnClick(string value)
		{
			builder.MergeAttribute(HtmlEventAttribute.OnClick, value, true);
			return (T)this;
		}

		/// <summary>
		/// Set the value of a specified attribute.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		public virtual T Attr(string name, object value)
		{
			((IElement)this).SetAttr(name, value);
			return (T)this;
		}

		/// <summary>
		/// Generate a label before the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		/// <param name="class">The value of the 'class' attribute for the label.</param>
		public virtual T Label(string value, string @class)
		{
			((IElement)this).LabelBeforeText = value;
			((IElement)this).LabelClass = @class;
			return (T)this;
		}

		/// <summary>
		/// Generate a label before the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		public virtual T Label(string value)
		{
			((IElement)this).LabelBeforeText = value;
			return (T)this;
		}

		/// <summary>
		/// Generate a label after the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		/// <param name="class">The value of the 'class' attribute for the label.</param>
		public virtual T LabelAfter(string value, string @class)
		{
			((IElement)this).LabelAfterText = value;
			((IElement)this).LabelClass = @class;
			return (T)this;
		}

		/// <summary>
		/// Generate a label after the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		public virtual T LabelAfter(string value)
		{
			((IElement)this).LabelAfterText = value;
			return (T)this;
		}

		public override string ToString()
		{
		    ApplyBehaviors();
			PreRender();
			var html = RenderLabel(((IElement)this).LabelBeforeText);
			html += builder.ToString(((IElement)this).TagRenderMode);
			html += RenderLabel(((IElement)this).LabelAfterText);
			return html;
		}

		#region Explicit IElement members

		void IElement.RemoveAttr(string name)
		{
			builder.Attributes.Remove(name);
		}

		void IElement.SetAttr(string name, object value)
		{
			var valueString = value == null ? null : value.ToString();
			builder.MergeAttribute(name, valueString, true);
		}

		string IElement.GetAttr(string name)
		{
			string result;
			builder.Attributes.TryGetValue(name, out result);
			return result;
		}

		string IElement.LabelBeforeText { get; set; }

		string IElement.LabelAfterText { get; set; }

		string IElement.LabelClass { get; set; }

		TagRenderMode IElement.TagRenderMode
		{
			get { return TagRenderMode; }
		}

		MemberExpression IMemberElement.ForMember
		{
			get { return forMember; }
		}

		protected virtual TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		} 

		#endregion

		protected virtual string RenderLabel(string labelText)
		{
			if (labelText == null)
			{
				return null;
			}
			var labelBuilder = GetLabelBuilder();
			labelBuilder.SetInnerText(labelText);
			return labelBuilder.ToString();
		}

		protected TagBuilder GetLabelBuilder()
		{
			var labelBuilder = new TagBuilder(HtmlTag.Label);
			if (builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				var id = builder.Attributes[HtmlAttribute.Id];
				labelBuilder.MergeAttribute(HtmlAttribute.For, id);
				labelBuilder.MergeAttribute(HtmlAttribute.Id, string.Format(LABEL_FORMAT, id));
			}
			if (!string.IsNullOrEmpty(((IElement)this).LabelClass))
			{
				labelBuilder.MergeAttribute(HtmlAttribute.Class, ((IElement)this).LabelClass);
			}
			return labelBuilder;
		}

		protected void ApplyBehaviors()
		{
            if (behaviors != null)
            {
                behaviors.ApplyTo(this);
            }
		}

		protected virtual void PreRender() { }
	}
}