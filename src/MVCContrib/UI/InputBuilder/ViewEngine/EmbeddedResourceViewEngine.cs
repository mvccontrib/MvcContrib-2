using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
	public class InputBuilderViewEngine : WebFormViewEngine
	{
		public InputBuilderViewEngine(string[] subdirs)
		{
			var inputs = subdirs.Concat(new string[]{"InputBuilders"});

            PartialViewLocationFormats = inputs.Select(s => "~/Views/" + s + "/{0}.aspx").Concat(subdirs.Select(s => "~/Views/" + s + "/{0}.ascx")).ToArray();

			MasterLocationFormats = inputs.Select(s => "~/Views/" + s + "/{0}.master").ToArray();

            ViewLocationFormats = inputs.Select(s => "~/Views/" + s + "/{0}.aspx").Concat(subdirs.Select(s => "~/Views/" + s + "/{0}.ascx")).ToArray(); ;
		}
	}
}