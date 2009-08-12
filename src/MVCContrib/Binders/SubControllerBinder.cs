using System;
using System.Web.Mvc;

namespace MvcContrib.Binders
{
	///<summary>
	/// Binder that creates SubControllers that are needed for an action method
	///</summary>
	public class SubControllerBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if(typeof(ISubController).IsAssignableFrom(bindingContext.ModelType))
			{
				object instance = CreateSubController(bindingContext.ModelType);
				if(instance == null)
				{
					throw new InvalidOperationException(bindingContext.ModelType + " not created properly.");
				}

				return instance;
			}

			return base.BindModel(controllerContext, bindingContext);
		}

		///<summary>
		/// Creates the subcontroller given its type.  Override this method to wire into an IoC container
		///</summary>
		///<param name="destinationType">The type of subcontroller</param>
		///<returns>an object instance</returns>
		public virtual object CreateSubController(Type destinationType)
		{
			return Activator.CreateInstance(destinationType, true);
		}
	}
}