using System.Web.Routing;
using System.Web;

namespace MvcContrib.Routing
{
	/// <summary>
	/// HTTP Handler that outputs route debugging information.
	/// </summary>
	public class DebugHttpHandler : IHttpHandler
	{
		public RequestContext RequestContext { get; set; }

		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			ProcessRequest(new HttpContextWrapper(context), RouteTable.Routes);
		}

		public void ProcessRequest(HttpContextBase context, RouteCollection routeTable)
		{
			string htmlFormat = @"<html>
<head>
    <title>Route Tester</title>
    <style>
        body, td, th {{font-family: verdana; font-size: small;}}
        .message {{font-size: .9em;}}
        caption {{font-weight: bold;}}
        tr.header {{background-color: #ffc;}}
        label {{font-weight: bold; font-size: 1.1em;}}
        .false {{color: #c00;}}
        .true {{color: #0c0;}}
    </style>
</head>
<body>
<h1>Route Tester</h1>
<div id=""main"">
    <p class=""message"">
        Type in a url in the address bar to see which defined routes match it. 
        A {{*catchall}} route is added to the list of routes automatically in 
        case none of your routes match.
    </p>
    <p><label>Route</label>: {1}</p>
    <table border=""1"" cellpadding=""3"" cellspacing=""0"">
        <caption>Route Data</caption>
        <tr class=""header""><th>Key</th><th>Value</th></tr>
        {0}
    </table>
    <hr />
    <table border=""1"" cellpadding=""3"" cellspacing=""0"">
        <caption>All Routes</caption>
        <tr class=""header"">
            <th>Matches Current Request</th>
            <th>Url</th>
            <th>Defaults</th>
            <th>Constraints</th>
            <th>DataTokens</th>
        </tr>
        {2}
    </table>
</div>
</body>
</html>";
			string routeDataRows = string.Empty;

			RouteData routeData = RequestContext.RouteData;
			RouteValueDictionary routeValues = routeData.Values;
			RouteBase matchedRouteBase = routeData.Route;

			string routes = string.Empty;
			using (routeTable.GetReadLock())
			{
				foreach (var routeBase in routeTable)
				{
					bool matchesCurrentRequest = (routeBase.GetRouteData(RequestContext.HttpContext) != null);
					string matchText = string.Format(@"<span class=""{0}"">{0}</span>", matchesCurrentRequest);
					string url = "n/a";
					string defaults = "n/a";
					string constraints = "n/a";
					string dataTokens = "n/a";

					var route = routeBase as Route;
					if (route != null)
					{
						url = route.Url;
						defaults = FormatRouteValueDictionary(route.Defaults);
						constraints = FormatRouteValueDictionary(route.Constraints);
						dataTokens = FormatRouteValueDictionary(route.DataTokens);
					}

					routes += string.Format(@"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>"
							, matchText
							, url
							, defaults
							, constraints
							, dataTokens);
				}
			}

			string matchedRouteUrl = "n/a";

			if (!(matchedRouteBase is DebugRoute))
			{
				foreach (var key in routeValues.Keys)
				{
					routeDataRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeValues[key]);
				}

				var matchedRoute = matchedRouteBase as Route;

				if (matchedRoute != null)
					matchedRouteUrl = matchedRoute.Url;
			}
			else
			{
				matchedRouteUrl = "<strong class=\"false\">NO MATCH!</strong>";
			}

			context.Response.Output.Write(string.Format(htmlFormat
				, routeDataRows
				, matchedRouteUrl
				, routes));
		}

		private static string FormatRouteValueDictionary(RouteValueDictionary values)
		{
			if (values == null)
				return "(null)";

			string display = string.Empty;
			foreach (var key in values.Keys)
				display += string.Format("{0} = {1}, ", key, values[key]);
			if (display.EndsWith(", "))
				display = display.Substring(0, display.Length - 2);
			return display;
		}
	}
}
