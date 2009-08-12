using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.Samples.UI.Views
{
	/// <summary>
	/// Behavior implementation that adds a maxlength to form elements if the corresponding model property is decorated with a Length attribute.
	/// </summary>
	public class MaxLengthBehavior : IMemberBehavior
	{
		public void Execute(IMemberElement element)
		{
			var helper = new MemberBehaviorHelper<StringLengthAttribute>();
			var attribute = helper.GetAttribute(element);

			if (attribute == null) 
			{
				return;
			}

			if (element is ISupportsMaxLength) 
			{
				element.SetAttr(HtmlAttribute.MaxLength, attribute.MaximumLength);
			}
		}
	}
}