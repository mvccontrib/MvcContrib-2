using System;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	/// <summary>
	/// Generic implementation of behavior for an <see cref="IBehaviorMarker"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBehavior<T> : IBehaviorMarker
	{
		/// <summary>
		/// Perform behavior modification on an object.
		/// </summary>
		/// <param name="behavee">The object to modify based on the behavior.</param>
		void Execute(T behavee);
	}

	[Obsolete("Use IBehavior<IElement> instead.")]
	public interface IBehavior : IBehavior<IElement> { }
}