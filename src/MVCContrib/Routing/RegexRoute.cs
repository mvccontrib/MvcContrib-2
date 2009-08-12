using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace MvcContrib.Routing
{
	/// <summary>
	/// This class can be used to create routes from a regular expression. 
	/// It is also bidirectional and can be used to generate urls from a given route.
	/// </summary>
	public class RegexRoute : RouteBase
	{
		private readonly RouteValueDictionary _defaults;
		private readonly string[] _groupNames;
		private readonly Regex _regex;
		private readonly IRouteHandler _routeHandler;
		private readonly Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> _getVirtualPath;
		private readonly string _urlGenerator;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class given a regular expression and a route handler.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		public RegexRoute(string regex, IRouteHandler routeHandler) : this(regex, (string)null, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class given a regular expression and a route 
		/// handler and a url generator to use to generate urls that would be handled by this route.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="urlGenerator">The URL generator; used to generate urls for this route.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
			 Justification = "The urlGenerator variable is used as a string to generate the mvc url; it is not itself a Uri.")]
		public RegexRoute(string regex, string urlGenerator, IRouteHandler routeHandler) : this(regex, urlGenerator, null, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="defaults">Default route values for this route.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		public RegexRoute(string regex, RouteValueDictionary defaults, IRouteHandler routeHandler) : this(regex, (string)null, defaults, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="urlGenerator">The URL generator; used to generate urls for this route.</param>
		/// <param name="defaults">Default route values for this route.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
		public RegexRoute(string regex, string urlGenerator, RouteValueDictionary defaults, IRouteHandler routeHandler) : this(regex, urlGenerator, RealGetVirtualPath, defaults, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="getVirtualPath">A function to use to generate a virtual path given the request context, incoming route value dictionary and this <see cref="RegexRoute"/> instance.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		public RegexRoute(string regex, Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> getVirtualPath, IRouteHandler routeHandler) : this(regex, getVirtualPath, null, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="getVirtualPath">A function to use to generate a virtual path given the request context, incoming route value dictionary and this <see cref="RegexRoute"/> instance.</param>
		/// <param name="defaults">Default route values for this route.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		public RegexRoute(string regex, Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> getVirtualPath, RouteValueDictionary defaults, IRouteHandler routeHandler) : this(regex, null, getVirtualPath, defaults, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RegexRoute"/> class.
		/// </summary>
		/// <param name="regex">The regular expression that this route handler is supposed to handle.</param>
		/// <param name="urlGenerator">The URL generator; used to generate urls for this route.</param>
		/// <param name="getVirtualPath">A function to use to generate a virtual path given the request context, incoming route value dictionary and this <see cref="RegexRoute"/> instance.</param>
		/// <param name="defaults">Default route values for this route.</param>
		/// <param name="routeHandler">The route handler to handle this route.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
		protected RegexRoute(string regex, string urlGenerator, Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> getVirtualPath, RouteValueDictionary defaults, IRouteHandler routeHandler)
		{
			_getVirtualPath = getVirtualPath ?? RealGetVirtualPath;
			_urlGenerator = urlGenerator;
			_defaults = defaults;
			_routeHandler = routeHandler;
			_regex = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			_groupNames = _regex.GetGroupNames();
		}

		/// <summary>
		/// Gets the default route value dictionary containing the default route settings.
		/// </summary>
		/// <value>The default settings for this route.</value>
		public RouteValueDictionary Defaults
		{
			get { return _defaults; }
		}

		/// <summary>
		/// Gets the route handler handling this route.
		/// </summary>
		/// <value>The route handler that handles this route.</value>
		public IRouteHandler RouteHandler
		{
			get { return _routeHandler; }
		}

		/// <summary>
		/// Gets the string used to generate mvc urls for this route.
		/// </summary>
		/// <value>The string used to generate urls for this route.</value>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string UrlGenerator
		{
			get { return _urlGenerator; }
		}

		/// <summary>
		/// Used to get a virtual path for a given set of route settings.
		/// If the combination of defaults/values doesn't satisfy the url generator tokens: null is returned, 
		/// signaling that this route isn't correct for the given values.
		/// </summary>
		/// <remarks>
		/// This function is not used for figuring out the routes. It is only used for generating links for new routes.
		/// </remarks>
		/// <param name="requestContext">The request context.</param>
		/// <param name="values">The settings to use to generate this virtual path.</param>
		/// <returns>A virtual path that represents the given settings.</returns>
		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			return _getVirtualPath(requestContext, values, this);
		}

		private static VirtualPathData RealGetVirtualPath(RequestContext requestContext, RouteValueDictionary values, RegexRoute thisRoute)
		{
			var pathDictionary = new Dictionary<string, string>();
			if (thisRoute.Defaults != null)
			{
				foreach (var pair in thisRoute.Defaults)
				{
					pathDictionary.Add(pair.Key, pair.Value.ToString());
				}
			}
			if (values != null)
			{
				foreach (var pair in values)
				{
					pathDictionary[pair.Key] = pair.Value.ToString();
				}
			}
			string newUrl = thisRoute.UrlGenerator;
			foreach (var pair in pathDictionary)
			{
				newUrl = newUrl.Replace("{" + pair.Key + "}", pair.Value);
			}

			if (Regex.IsMatch(newUrl, @"\{\w+\}"))
			{
				return null;
			}
			return new VirtualPathData(thisRoute, newUrl);
		}

		/// <summary>
		/// Gets the route data from an incoming request; parses the incoming virtual 
		/// path and returns the route data that was inside the url.
		/// </summary>
		/// <param name="httpContext">The HTTP context containing the url data.</param>
		/// <returns>Route data gleaned from the context.</returns>
		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			string requestUrl = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
			return GetRouteData(requestUrl);
		}

		/// <summary>
		/// Gets the route data from the request portion of the url.
		/// </summary>
		/// <param name="request">The request string (the part after the virtual directory but before the query string).</param>
		/// <returns>Route data gleaned from the context.</returns>
		public virtual RouteData GetRouteData(string request)
		{
			Match match = _regex.Match(request);

			if (!match.Success)
			{
				return null;
			}

			RouteData data = GenerateDefaultRouteData();
			foreach (string groupName in _groupNames)
			{
				Group group = match.Groups[groupName];
				if (group.Success)
				{
					if (!string.IsNullOrEmpty(groupName) && !char.IsNumber(groupName, 0))
					{
						data.Values[groupName] = group.Value;
					}
				}
			}
			return data;
		}

		private RouteData GenerateDefaultRouteData()
		{
			var data = new RouteData(this, RouteHandler);
			if (Defaults != null)
			{
				foreach (var def in Defaults)
				{
					data.Values.Add(def.Key, def.Value);
				}
			}
			return data;
		}
	}
}