using System;

namespace MvcContrib.PortableAreas
{
	public interface IMessageHandlerFactory
	{
		IMessageHandler Create(Type type);
	}

	public class MessageHandlerFactory : IMessageHandlerFactory
	{
		public IMessageHandler Create(Type type)
		{
			return (IMessageHandler) Activator.CreateInstance(type);
		}
	}
}