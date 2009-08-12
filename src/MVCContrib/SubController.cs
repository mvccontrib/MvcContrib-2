using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib;
using MvcContrib.Filters;

namespace MvcContrib
{
	///<summary>
	/// SubController base class for subcontrollers.  SubControllers can  be infinitely nested.
	///</summary>
	[SubControllerActionToViewData]
	public class SubController : Controller, ISubController
	{
		public virtual Action GetResult(ControllerBase parentController)
		{
			RequestContext requestContext = GetNewRequestContextFromController(parentController);
			return () => Execute(requestContext);
		}

		///<summary>
		/// Gets new RequestContext using objects from parent controller.  This ensures subcontrollers have their own state that doesn't conflict with the parent.
		///</summary>
		///<param name="parentController">Parent controller</param>
		///<returns>RequestContext</returns>
		public RequestContext GetNewRequestContextFromController(ControllerBase parentController)
		{
			RouteData parentRouteData = parentController.ControllerContext.RouteData;
			var routeData = new RouteData(parentRouteData.Route, parentRouteData.RouteHandler);
			string controllerName = GetControllerName();
			routeData.Values["controller"] = controllerName;
			routeData.Values["action"] = controllerName;
			return new RequestContext(parentController.ControllerContext.HttpContext, routeData);
		}

		///<summary>
		/// Gets the name from the type by trimming "controller" and "subcontroller" from the type name.  The subcontroller action must match the controller name.
		///</summary>
		///<returns></returns>
		public string GetControllerName()
		{
			string typeNameLowered = GetType().Name.ToLowerInvariant();
			int index = typeNameLowered.IndexOf("subcontroller");
			if(index == -1)
			{
				index = typeNameLowered.IndexOf("controller");
			}

			if(index == -1)
			{
				return typeNameLowered;
			}

			return typeNameLowered.Substring(0, index);
		}
	}

	///<summary>
	/// SubController with generic Model property.
	///</summary>
	///<typeparam name="T"></typeparam>
	public class SubController<T> : SubController, ISubController<T>
	{
		public virtual T Model { get; set; }
	}
}