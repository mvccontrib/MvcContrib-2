using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	public interface IMemberBehavior : IBehaviorMarker
	{
		void Execute(IMemberElement element);
	}
}
