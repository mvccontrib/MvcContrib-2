using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'checkbox.'
	/// </summary>
	public abstract class CheckBoxBase<T> : Input<T> where T : CheckBoxBase<T>
	{
		protected CheckBoxBase(string name) : base(HtmlInputType.Checkbox, name)
		{
			elementValue = "true";
		}

		protected CheckBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Checkbox, name, forMember, behaviors)
		{
			elementValue = "true";
		}

		/// <summary>
		/// Set the checked attribute.
		/// </summary>
		/// <param name="value">Whether the checkbox should be checked.</param>
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

		public override string ToString()
		{
			var html = ToCheckBoxOnlyHtml();
			var hiddenId = "_Hidden";
			if (((IElement)this).Builder.Attributes.ContainsKey("id"))
			{
				hiddenId = ((IElement)this).Builder.Attributes["id"] + hiddenId;
			}
			var hidden = new Hidden(builder.Attributes[HtmlAttribute.Name]).Id(hiddenId).Value("false").ToString();
			return string.Concat(html, hidden);
		}

		public string ToCheckBoxOnlyHtml()
		{
			return base.ToString();
		}

		protected override void ApplyModelState(System.Web.Mvc.ModelState state) 
		{
			var isChecked = state.Value.ConvertTo(typeof(bool?)) as bool?;

			if (isChecked.HasValue) 
			{
				Checked(isChecked.Value);
			}
		}
	}
}