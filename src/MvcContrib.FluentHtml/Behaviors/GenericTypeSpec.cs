using System;

namespace MvcContrib.FluentHtml.Behaviors
{
	public class GenericTypeSpec
	{
		public Type OpenGenericType;
		public Type GenericParameterType;

		public Func<Type, bool> Matcher
		{
			get
			{
				return x => x.IsGenericType &&
							x.GetGenericTypeDefinition() == OpenGenericType &&
							x.GetGenericArguments()[0].IsAssignableFrom(GenericParameterType);
			}
		}
	}
}