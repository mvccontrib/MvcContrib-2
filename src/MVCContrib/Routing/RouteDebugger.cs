using System.Web.Routing;

namespace MvcContrib.Routing
{
	/// <summary>
	/// Helper class for switching the default route handler to the debug route handler.
	/// </summary>
	public static class RouteDebugger
	{
		/// <summary>
		/// Iterates over each route in the route collection and replaces the default route handler with a DebugRouteHandler.
		/// </summary>
		/// <param name="routes">The route table</param>
		public static void RewriteRoutesForTesting(RouteCollection routes)
		{
			using (routes.GetReadLock())
			{
				bool foundDebugRoute = false;
				foreach (var routeBase in routes)
				{
					var route = routeBase as Route;
					if (route != null)
					{
						route.RouteHandler = new DebugRouteHandler();
					}

					if (route == DebugRoute.Singleton)
						foundDebugRoute = true;

				}
				if (!foundDebugRoute)
				{
					routes.Add(DebugRoute.Singleton);
				}
			}


		}

	}
}
