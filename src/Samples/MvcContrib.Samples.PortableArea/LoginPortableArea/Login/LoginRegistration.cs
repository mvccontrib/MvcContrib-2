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

            context.MapRoute("logoff", "login/iamouttahere",
		                     new {controller = "login", action = "LogOff"});
            context.MapRoute("login", "login/signmein",
		                     new {controller = "login", action = "Index"});
			context.MapRoute(
				"loginarea",
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