using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates an input element of type 'file.'
	/// </summary>
	public class FileUpload : TextInput<FileUpload>
	{
		/// <summary>
		/// Generates an input element of type 'file.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element. Also used to derive the 'id' attribute.</param>
		public FileUpload(string name) : base(HtmlInputType.File, name) { }

		/// <summary>
		/// Generates an input element of type 'file.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element. Also used to derive the 'id' attribute.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public FileUpload(string name, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.File, name, null, behaviors) { }
	}
}
