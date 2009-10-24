using System;
using System.Collections.Generic;

namespace MvcContrib.PortableAreas
{
	public class ApplicationBus : List<Type>, IApplicationBus
	{		
		public new void Add(Type type)
		{
			if (type.GetInterface(typeof(IMessageHandler).Name) == null)
			{
				throw new InvalidOperationException(string.Format("Type {0} must implement the IMessageHandler interface",type.Name));
			}
			base.Add(type);
		}

		private IMessageHandlerFactory _factory;

		public ApplicationBus(IMessageHandlerFactory factory)
		{
			_factory = factory;
		}

		public void Send(IEventMessage eventMessage)
		{
			foreach (var handler in GetHandlersForType(eventMessage.GetType()))
			{
				handler.Handle(eventMessage);
			}
		}

		public void SetMessageHandlerFactory(IMessageHandlerFactory factory)
		{
			_factory = factory;
		}

		public IEnumerable<IMessageHandler> GetHandlersForType(Type type)
		{
			foreach (Type handlerType in this)
			{
				var handler = _factory.Create(handlerType);
				if (handler.CanHandle(type))
				{
					yield return handler;
				}
			}			
		}
	}
}