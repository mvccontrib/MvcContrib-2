using MvcContrib.PortableAreas;

namespace MvcContrib.PortableAreas
{
	public class PortableArea
	{
		private static IApplicationBus _bus;
		private static object _busLock = new object();

		public static IApplicationBus Bus { 
		get
		{
			InitializeTheDefaultBus();
			return _bus;
		}
			set
			{
				_bus=value;					
			}
		}

		private static void InitializeTheDefaultBus()
		{
			if(_bus==null)
			{
				lock(_busLock)
				{
					if(_bus==null)
					{
						_bus = new ApplicationBus(new MessageHandlerFactory());
					}
				}
			}
		}
	}
}