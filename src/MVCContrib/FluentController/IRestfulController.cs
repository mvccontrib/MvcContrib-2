using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.SimplyRestful;

namespace MvcContrib.FluentController
{
	/// <summary>
	/// Extends the <see cref="Controller"/> to allow for <see cref="RestfulAction"/> to be passed around instead of strings.
	/// </summary>
	internal interface IRestfulController
	{
		ViewResult View(RestfulAction viewName, RestfulAction masterName, object model);
		ViewResult View(RestfulAction viewName, object model);
		ViewResult View(RestfulAction viewName, RestfulAction masterName);
		ViewResult View(RestfulAction viewName);
		RedirectToRouteResult RedirectToAction(RestfulAction actionName, string controllerName, RouteValueDictionary routeValues);
		RedirectToRouteResult RedirectToAction(RestfulAction actionName, string controllerName, object routeValues);
		RedirectToRouteResult RedirectToAction(RestfulAction actionName, string controllerName);
		RedirectToRouteResult RedirectToAction(RestfulAction actionName, RouteValueDictionary routeValues);
		RedirectToRouteResult RedirectToAction(RestfulAction actionName);
		PartialViewResult PartialView(RestfulAction viewName, object model);
		PartialViewResult PartialView(RestfulAction viewName);
	}
}