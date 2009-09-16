using System.Collections.Generic;

namespace MvcContrib.IncludeHandling.Configuration
{
	public interface IIncludeHandlingSettings
	{
		CssTypeElement Css { get; }
		JsTypeElement Js { get; }
		IDictionary<IncludeType, IIncludeTypeSettings> Types { get; }
	}
}