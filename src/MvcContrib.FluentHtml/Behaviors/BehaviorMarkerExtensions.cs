using System.Collections.Generic;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	/// <summary>
	/// Extensions for IBehaviorMarker
	/// </summary>
	public static class BehaviorMarkerExtensions
	{
		/// <summary>
		/// Apply behaviors to an object.
		/// </summary>
		/// <param name="behaviors">The behaviors to apply.</param>
		/// <param name="target">The target element to apply behaviors to.</param>
		public static IEnumerable<IBehaviorMarker> ApplyTo<T>(this IEnumerable<IBehaviorMarker> behaviors, T target) where T : IElement
		{
			foreach (var behavior in behaviors)
			{
				ApplyTo(behavior, target);
			}
			return behaviors;
		}

		/// <summary>
		/// Apply behavior to an object.
		/// </summary>
		/// <param name="behavior">The behavior to apply.</param>
		/// <param name="target">The target element to apply behavior to.</param>
		public static IBehaviorMarker ApplyTo<T>(this IBehaviorMarker behavior, T target) where T : IElement
		{
			CacheingBehaviorApplier.Apply(behavior, target);
			return behavior;
		}
	}
}