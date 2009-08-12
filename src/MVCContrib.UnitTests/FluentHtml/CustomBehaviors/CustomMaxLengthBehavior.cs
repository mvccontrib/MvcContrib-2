using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class CustomMaxLengthBehavior : IMemberBehavior
	{
		public void Execute(IMemberElement element)
		{
			var helper = new MemberBehaviorHelper<RangeAttribute>();
			var attribute = helper.GetAttribute(element);

			if (attribute == null) 
			{
				return;
			}

			if (element is ISupportsMaxLength) 
			{
				element.SetAttr(HtmlAttribute.MaxLength, attribute.Maximum);
			}
		}
	}
}