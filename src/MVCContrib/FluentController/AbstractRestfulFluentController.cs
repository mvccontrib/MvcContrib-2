using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.SimplyRestful;

namespace MvcContrib.FluentController
{
	/// <summary>
	/// <para>A Restful fluent controller to standardise the design of controllers and ease 
	/// the burden of testing.</para>
	/// 
	/// <para>If you need Dependency Injection (and you might want to for repositories) 
	/// do so in your own constructor.</para>
	/// 
	/// <code>
	///     public class UserController : AbstractRestfulFluentController
	///    {
	///        public UserController()
	///        {
	///            DIContainer.ResolveDependencies(this);
	///        }
	///    ...
	/// </code>
	/// 
	/// </summary>
	/// <returns></returns>
	public abstract class AbstractRestfulFluentController : AbstractFluentController, IRestfulController
	{
		public virtual ViewResult View(RestfulAction viewName, RestfulAction masterName, object model)
		{
			return View(viewName.ToString(), masterName.ToString(), model);
		}

		public virtual ViewResult View(RestfulAction viewName, object model)
		{
			return View(viewName.ToString(), model);
		}

		public virtual ViewResult View(RestfulAction viewName, RestfulAction masterName)
		{
			return View(viewName.ToString(), masterName.ToString());
		}

		public virtual ViewResult View(RestfulAction viewName)
		{
			return View(viewName.ToString());
		}

		public virtual RedirectToRouteResult RedirectToAction(RestfulAction actionName, string controllerName,
		                                                      RouteValueDictionary routeValues)
		{
			return RedirectToAction(actionName.ToString(), controllerName, routeValues);
		}

		public virtual RedirectToRouteResult RedirectToAction(RestfulAction actionName, string controllerName,
		                                                      object routeValues)
		{
			return RedirectToAction(actionName.ToString(), controllerName, routeValues);
		}

		public virtual RedirectToRouteResult RedirectToAction(RestfulAction actionName, string controllerName)
		{
			return RedirectToAction(actionName.ToString(), controllerName);
		}

		public virtual RedirectToRouteResult RedirectToAction(RestfulAction actionName, RouteValueDictionary routeValues)
		{
			return RedirectToAction(actionName.ToString(), routeValues);
		}

		public virtual RedirectToRouteResult RedirectToAction(RestfulAction actionName)
		{
			return RedirectToAction(actionName.ToString());
		}

		public virtual PartialViewResult PartialView(RestfulAction viewName, object model)
		{
			return PartialView(viewName.ToString(), model);
		}

		public virtual PartialViewResult PartialView(RestfulAction viewName)
		{
			return PartialView(viewName.ToString());
		}
	}
}