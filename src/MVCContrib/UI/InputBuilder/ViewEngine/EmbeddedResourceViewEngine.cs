using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
	public class InputBuilderViewEngine : WebFormViewEngine
	{
		public InputBuilderViewEngine(string[] subdirs)
		{
			var inputs = subdirs.Concat(new string[]{"InputBuilders"});

			PartialViewLocationFormats = inputs.Select(s => "~/Views/" + s + "/{0}.aspx").ToArray();

			MasterLocationFormats = inputs.Select(s => "~/Views/" + s + "/{0}.master").ToArray();

			ViewLocationFormats = subdirs.Select(s => "~/Views/" + s + "/{0}.aspx").ToArray(); ;
		}
	}
}