using System;
using System.Linq;
using NUnit.Framework;
using MvcContrib.PortableAreas;

namespace MvcContrib.UnitTests.PortableAreas
{

	[TestFixture]
	public class ApplicationBusTester
	{
		[Test]
		public void bus_should_only_add_message_handlers()
		{
			var bus = new ApplicationBus(new MessageHandlerFactory());
					
			try
			{
				bus.Add(this.GetType());	
			}
			catch(InvalidOperationException ex)
			{
				ex.Message.EndsWith("must implement the IMessageHandler interface").ShouldBeTrue();
				return;
			}
			Assert.Fail("Add should throw exception when adding invalid message handler types");			
		}

		[Test]
		public void bus_should_message_to_handlers()
		{
			fooHandler.Sent = false;
			var bus = new ApplicationBus(new MessageHandlerFactory());
			bus.Add(typeof(fooHandler));
			bus.Add(typeof(barHandler));
			bus.Send(new foo());
			fooHandler.Sent.ShouldBeTrue();
		}



		[Test]
		public void bus_should_find_handlers_for_a_message_type()
		{
			var bus = new ApplicationBus(new MessageHandlerFactory());
			bus.Add(typeof(fooHandler));
			bus.Add(typeof(barHandler));
			var results = bus.GetHandlersForType(typeof(foo)).ToList();
			results.Count.ShouldEqual(1);
		}
		public class fooHandler:MessageHandler<foo>
		{
			public override void Handle(foo message)
			{
				Sent = true;
			}

			public static bool Sent { get; set; }
		}
		public class barHandler : IMessageHandler
		{
			public void Handle(object message)
			{
				throw new NotImplementedException();
			}

			public bool CanHandle(Type type)
			{
				return false;
			}
		}
		public class foo : IEventMessage { }
	}

	
}