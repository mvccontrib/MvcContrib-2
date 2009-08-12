using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.MenuBuilder;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Used internally to create the menu, use MvcContrib.UI.MenuBuilder.Menu to create a menu.
	///</summary>
	public class SecureActionMenuItem<T> : ActionMenuItem<T> where T : Controller
	{
        
		public override void Prepare(ControllerContext controllerContext)
		{
			if(MenuAction == null)
				throw new InvalidOperationException("MenuAction must be defined prior to using a secure menu item");
			MethodCallExpression callExpression = MenuAction.Body as MethodCallExpression;
			if (callExpression == null)
				throw new InvalidOperationException("Expression must be a method call");

			var attributes = GetAuthorizeAttributes(callExpression.Method);
			internalDisabled = Disabled || !CanAddItem(attributes, controllerContext);
            base.Prepare(controllerContext);
		}

		protected virtual bool CanAddItem(IEnumerable<AuthorizeAttribute> attributes, ControllerContext context)
		{
			foreach (AuthorizeAttribute attribute in attributes)
			{
				if (attribute != null && !attribute.Authorized(context))
					return false;
			}
			return true;
		}

        protected virtual AuthorizeAttribute[] GetAuthorizeAttributes(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			AuthorizeAttribute[] typeFilters = (AuthorizeAttribute[])methodInfo.ReflectedType.GetCustomAttributes(typeof(AuthorizeAttribute), true /* inherit */);
			AuthorizeAttribute[] methodFilters = (AuthorizeAttribute[])methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true /* inherit */);
			AuthorizeAttribute[] orderedFilters = typeFilters.Concat(methodFilters).OrderBy(attr => attr.Order).ToArray();
			return orderedFilters;
		}
	}
}