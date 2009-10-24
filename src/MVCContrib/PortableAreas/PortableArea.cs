using MvcContrib.PortableAreas;

namespace MvcContrib.PortableAreas
{
	public class PortableArea
	{
		static PortableArea()
		{
			Bus=new ApplicationBus(new MessageHandlerFactory());
		}

		public static IApplicationBus Bus { get; protected set;}
	}
}