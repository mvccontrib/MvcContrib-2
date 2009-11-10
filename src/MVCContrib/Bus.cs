using System;
using MvcContrib.PortableAreas;

namespace MvcContrib
{
	public class Bus
	{
		private static IApplicationBus _instance;
		private static object _busLock = new object();

		public static IApplicationBus Instance { 
			get
			{
				InitializeTheDefaultBus();
				return _instance;
			}
			set
			{
				_instance=value;					
			}
		}

		private static void InitializeTheDefaultBus()
		{
			if(_instance==null)
			{
				lock(_busLock)
				{
					if(_instance==null)
					{
						_instance = new ApplicationBus(new MessageHandlerFactory());
					}
				}
			}
		}
		
		public static void Send(IEventMessage eventMessage)
		{
			Instance.Send(eventMessage);
		}
		
		public static void AddMessageHandler(Type type)
		{
			Instance.Add(type);
		}
	}
}