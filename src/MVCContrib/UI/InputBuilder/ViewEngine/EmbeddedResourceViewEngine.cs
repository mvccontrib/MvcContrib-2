using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder.ViewEngine
{
	public class InputBuilderViewEngine : WebFormViewEngine
	{
		public InputBuilderViewEngine(string[] subdirs)
		{
			IEnumerable<string> inputs = subdirs.Concat(new[] {"InputBuilders"});

			PartialViewLocationFormats =
				inputs.Select(s => "~/Views/" + s + "/{0}.aspx").Concat(subdirs.Select(s => "~/Views/" + s + "/{0}.ascx")).ToArray();

			MasterLocationFormats = inputs.Select(s => "~/Views/" + s + "/{0}.Master").ToArray();

			ViewLocationFormats =
				inputs.Select(s => "~/Views/" + s + "/{0}.aspx").Concat(subdirs.Select(s => "~/Views/" + s + "/{0}.ascx")).ToArray();
			;
		}
	}
}