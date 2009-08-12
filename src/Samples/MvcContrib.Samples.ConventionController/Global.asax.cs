using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.Samples
{
	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			RouteTable.Routes.Add(new Route("{controller}/{action}/{id}", new MvcRouteHandler())
          	{
          		Defaults = new RouteValueDictionary(new {controller = "Shipment", action = "Index", id = ""}),
          	});
		}
	}
}