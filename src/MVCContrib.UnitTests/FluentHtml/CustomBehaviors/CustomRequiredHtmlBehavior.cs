using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class CustomRequiredHtmlBehavior : IMemberBehavior
	{
		public void Execute(IMemberElement element)
		{
			var helper = new MemberBehaviorHelper<RequiredAttribute>();
			var attribute = helper.GetAttribute(element);
			if (attribute != null)
			{
				element.SetAttr("class", "req");
			}
		}
	}
}
