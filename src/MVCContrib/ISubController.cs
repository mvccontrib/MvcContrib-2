using System;
using System.Web.Mvc;

namespace MvcContrib
{
	///<summary>
	/// Interface that represents a subcontroller
	///</summary>
	public interface ISubController : IController
	{
		///<summary>
		/// Gets and action that can later be invoked to produce the behavior of the subcontroller
		///</summary>
		///<param name="parentController">The controller depending on the subcontroller.  The subcontroller uses the ControllerContext of the parent to create a new RequestContext.</param>
		///<returns>System.Action</returns>
		Action GetResult(ControllerBase parentController);
	}

	///<summary>
	/// SubController with typed Model property for parameter passing from a parent controller.
	///</summary>
	///<typeparam name="T"></typeparam>
	public interface ISubController<T> : ISubController
	{
		///<summary>
		/// Gets and sets a model object that matches the generic type.
		///</summary>
		T Model { get; set; }
	}
}