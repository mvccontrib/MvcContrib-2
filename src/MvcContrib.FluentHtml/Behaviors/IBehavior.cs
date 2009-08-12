using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	public interface IBehavior : IBehaviorMarker
	{
		void Execute(IElement element);
	}
}