using System;
using MvcContrib.PortableAreas;
using NUnit.Framework;

namespace MvcContrib.UnitTests.PortableAreas
{
	[TestFixture]
	public class MessageHandlerFactoryTester
	{
		[Test]
		public void Should_create_a_handler()
		{
			var factory = new MessageHandlerFactory();
			var result = factory.Create(typeof(foo));
			result.ShouldNotBeNull();
			
		}

		private class foo:IMessageHandler {
			public void Handle(object message)
			{
				throw new NotImplementedException();
			}

			public bool CanHandle(Type type)
			{
				throw new NotImplementedException();
			}
		}	
	}

	
}