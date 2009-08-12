using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.Samples.UI.Views
{
	/// <summary>
	/// Behavior implementation that adds "class='required'" to form elements if the corresponding model property is decorated with a Required attribute.
	/// </summary>
	public class RequiredBehavior : IMemberBehavior
	{
		public void Execute(IMemberElement element) 
		{
			var helper = new MemberBehaviorHelper<RequiredAttribute>();

			var attribute = helper.GetAttribute(element);
			if (attribute != null) 
			{
				element.SetAttr(HtmlAttribute.Class, "required");
			}
		}
	}
}