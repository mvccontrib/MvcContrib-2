using System.Web.Mvc;

namespace MvcContrib.FluentHtml.Behaviors
{
	public interface ISupportsModelState
	{
		void ApplyModelState(ModelState state);
	}
}