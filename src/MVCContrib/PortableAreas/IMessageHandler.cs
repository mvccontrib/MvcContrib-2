using System;

namespace MvcContrib.PortableAreas
{
	public interface IMessageHandler
	{
		void Handle(object message);
		bool CanHandle(Type type);	
	}
	
	public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IEventMessage
	{
		void Handle(TMessage message);
	}

	
	public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IEventMessage
	{
		public abstract void Handle(TMessage message);

		public virtual void Handle(object message)
		{
			Handle((TMessage)message);
		}

		public virtual bool CanHandle(Type type)
		{
			return type == typeof(TMessage);
		}
	}

}