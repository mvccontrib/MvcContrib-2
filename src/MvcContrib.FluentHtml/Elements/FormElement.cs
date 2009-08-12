using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for form elements.
	/// </summary>
	/// <typeparam name="T">Derived type</typeparam>
	public abstract class FormElement<T> : DisableableElement<T>, ISupportsAutoLabeling where T : FormElement<T>, IElement
	{
		protected FormElement(string tag, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(tag, forMember, behaviors)
		{
			SetName(name);
		}

		protected FormElement(string tag, string name) : base(tag)
		{
			SetName(name);
		}

		public override string ToString()
		{
			InferIdFromName();
			return base.ToString();
		}

		/// <summary>
		/// Determines how the HTML element is closed.
		/// </summary>
		protected override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.SelfClosing; }
		}

		/// <summary>
		/// If no label has been explicitly set, set the label using the element name.
		/// </summary>
		public virtual T AutoLabel()
		{
			SetAutoLabel();
			return (T)this;
		}

		/// <summary>
		/// If no label before has been explicitly set, set the label before using the element name.
		/// </summary>
		public virtual T AutoLabelAfter()
		{
			SetAutoLabelAfter();
			return (T)this;
		}

		protected virtual void InferIdFromName()
		{
			if (!builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				Attr(HtmlAttribute.Id, builder.Attributes[HtmlAttribute.Name].FormatAsHtmlId());
			}
		}

		protected void SetName(string name)
		{
			((IElement)this).SetAttr(HtmlAttribute.Name, name);
		}

		public virtual void SetAutoLabel()
		{
			if (((IElement)this).LabelBeforeText == null)
			{
				var settings = GetAutoLabelSettings();
				((IElement)this).LabelBeforeText = GetAutoLabelText(settings);
				((IElement)this).LabelClass = settings == null ? null : settings.LabelCssClass;
			}
		}

		public virtual void SetAutoLabelAfter()
		{
			if (((IElement)this).LabelAfterText == null)
			{
				var settings = GetAutoLabelSettings();
				((IElement)this).LabelAfterText = GetAutoLabelText(settings);
				((IElement)this).LabelClass = settings == null ? null : settings.LabelCssClass;
			}
		}

		private AutoLabelSettings GetAutoLabelSettings()
		{
			//TODO: should we throw if there is more than one?
			AutoLabelSettings foundSettings = null;
			if (behaviors != null)
			{
				foundSettings = behaviors.Where(x => x is AutoLabelSettings).FirstOrDefault() as AutoLabelSettings;
			}
			return foundSettings ?? new AutoLabelSettings(false, null, null);
		}

		private string GetAutoLabelText(AutoLabelSettings settings)
		{
			var result = ((IElement)this).GetAttr(HtmlAttribute.Name);
			if (result == null)
			{
				return result;
			}
			if (settings.UseFullNameForNestedProperties)
			{
				result = result.Replace('.', ' ');
			}
			else
			{
				var lastDot = result.LastIndexOf(".");
				if (lastDot >= 0)
				{
					result = result.Substring(lastDot + 1);
				}
			}
			result = result.PascalCaseToPhrase();
			result = RemoveArrayNotationFromPhrase(result);
			result = settings.LabelFormat != null
				? string.Format(settings.LabelFormat, result)
				: result;
			return result;
		}

		private string RemoveArrayNotationFromPhrase(string phrase)
		{
			if (phrase.IndexOf("[") >= 0)
			{
				var words = new List<string>(phrase.Split(' '));
				words = words.ConvertAll<string>(RemoveArrayNotation);
				phrase = string.Join(" ", words.ToArray());
			}
			return phrase;
		}

		private string RemoveArrayNotation(string s)
		{
			var index = s.LastIndexOf('[');
			return index >= 0
				? s.Remove(index)
				: s;
		}
	}
}