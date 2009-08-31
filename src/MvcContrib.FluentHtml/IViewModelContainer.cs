using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public interface IViewModelContainer<T> : IViewDataContainer, IBehaviorsContainer where T : class
	{
		T ViewModel { get; }
		string HtmlNamePrefix { get; set; }
		HtmlHelper Html { get; }
	}
}