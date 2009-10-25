using System.Web.Mvc;
using LoginPortableArea.Login.Messages;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login
{
	public class LoginRegistration : PortableAreaRegistration
	{
		public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
		{
			bus.Send(new RegistrationMessage("Registering Login Portable Area"));

			context.MapRoute(
				"login",
				"login/{controller}/{action}",
				new {controller = "login", action = "index"});

			RegisterTheViewsInTheEmbeddedViewEngine(GetType());
		}

		public override string AreaName
		{
			get { return "Login"; }
		}
	}
}