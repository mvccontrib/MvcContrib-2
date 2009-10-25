using System;
using System.Collections.Generic;

namespace MvcContrib.PortableAreas
{
	public interface IApplicationBus:IList<Type>
	{
		void Send(IEventMessage eventMessage);
		void SetMessageHandlerFactory(IMessageHandlerFactory factory);
	}
}