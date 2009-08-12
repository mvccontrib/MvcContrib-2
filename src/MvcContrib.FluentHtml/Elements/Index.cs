using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates an HTML input element of type 'hidden.'
	/// </summary>
	public class Index : Hidden
	{
		/// <summary>
		/// Generates an HTML input element of type 'hidden' with '.Index" appended to the name.
		/// </summary>
		/// <param name="baseName">Base value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public Index(string baseName) : base(GetName(baseName)) { }

		/// <summary>
		/// Generates an HTML input element of type 'hidden' with '.Index" appended to the name.
		/// </summary>
		/// <param name="baseName">Base value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the model member assocaited with the element.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public Index(string baseName, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(GetName(baseName), forMember, behaviors) { }

		public static string GetName(string baseName)
		{
			return baseName + ".Index";
		}

		protected override void InferIdFromName()
		{
			Attr(HtmlAttribute.Id, string.Format("{0}_{1}",
				builder.Attributes[HtmlAttribute.Name].FormatAsHtmlId(), elementValue));
		}
	}
}