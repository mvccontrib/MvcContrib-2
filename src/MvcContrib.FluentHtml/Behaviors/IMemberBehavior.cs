using System;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	[Obsolete("Use IBehavior<IMemberElement> instead.")]
	public interface IMemberBehavior : IBehavior<IMemberElement> { }
}