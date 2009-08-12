using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <summary>
	/// Filter attribute for handling errors.
	/// When an error occurs, the RescueAttribute will first search for a view that matches the exception name,
	/// then render the specified view if no matching exception view is found.
	/// The rescue attribute can be limited to certain exception types.
	/// <example>
	/// <![CDATA[
	/// [Rescue("MyErrorView", typeof(InvalidOperationException)]
	/// [Rescue("DatabaseError", typeof(SqlException)]
	/// public class HomeController  {
	/// 
	///     public ActionResult Action()
	///     {
	///        throw new SqlException();
	///       //will look for SqlException.aspx, then DatabaseError.aspx
	///     
	///        throw new InvalidOperationException();
	///       //will look for InvalidOperationException.aspx then MyErrorView.aspx 
	///     }
	/// 
	/// }
	/// 
	/// [Rescue("DefaultRescue")] 
	/// public ActionResult ControllerAction  {
	///     throw new CookieException();
	/// 
	///     //this will look for CookieException.aspx
	///     //then call DefaultRescue.aspx if not found
	/// }
	/// ]]>
	/// 
	/// 
	/// </example>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class RescueAttribute : FilterAttribute, IExceptionFilter
	{
		private string _view;
		private readonly Type[] _exceptionsTypes;

		/// <summary>
		/// Creates a new instance of the RescueAttribute class.
		/// </summary>
		/// <param name="view">The name of the view to render when an exception is thrown if no matching view is found.</param>
		public RescueAttribute(string view)
			: this(view, typeof(Exception)) {}

		/// <summary>
		/// Creates a new instance of the RescueAttribute class.
		/// </summary>
		/// <param name="view">The name of the view to render when an exception is thrown if no matching view is found.</param>
		/// <param name="exceptionTypes">The types of exception that this attribute will be restricted in catching.</param>
		public RescueAttribute(string view, params Type[] exceptionTypes)
		{
			if(string.IsNullOrEmpty(view))
			{
				throw new ArgumentException("view is required", "view");
			}

			_view = view;

			if(exceptionTypes != null)
			{
				_exceptionsTypes = exceptionTypes;
			}

			IgnoreTypes = new Type[0];
			AutoLocate = true;
			IgnoreAjax = true;
		}


		/// <summary>
		/// After the action has been executed, the Rescue will be invoked if the filterContext has an Exception.
		/// </summary>
		/// <param name="filterContext">The current filter context.</param>
		public virtual void OnException(ExceptionContext filterContext)
		{
			Type baseExceptionType = filterContext.Exception.GetBaseException().GetType();

			if(IgnoreAjax && filterContext.HttpContext.Request.IsAjax())
			{
				return;
			}

			if(IgnoreTypes != null && IgnoreTypes.Contains(baseExceptionType))
			{
				return;
			}

			foreach(var expectedExceptionType in ExceptionsTypes)
			{
				if(expectedExceptionType.IsAssignableFrom(baseExceptionType))
				{
					if(AutoLocate)
					{
						if(ViewExists(baseExceptionType, filterContext))
						{
							ViewName = baseExceptionType.Name;
							filterContext.Result = CreateActionResult(filterContext.Exception, filterContext);
							filterContext.ExceptionHandled = true;
							return;
						}

						if(ViewExists(expectedExceptionType, filterContext))
						{
							ViewName = expectedExceptionType.Name;
							filterContext.Result = CreateActionResult(filterContext.Exception, filterContext);
							filterContext.ExceptionHandled = true;
							return;
						}
					}
					filterContext.Result = CreateActionResult(filterContext.Exception, filterContext);
					filterContext.ExceptionHandled = true;
					return;
				}
			}
		}

		protected virtual ActionResult CreateActionResult(Exception exception, ExceptionContext context)
		{
			var controller = (string)context.RouteData.Values["controller"];
			var action = (string)context.RouteData.Values["action"];

			var viewData = new ViewDataDictionary<HandleErrorInfo>(new HandleErrorInfo(exception, controller, action));

			return new ViewResult
			{
				ViewName = ViewName,
				//MasterName = this.Master,
				ViewData = viewData,
				TempData = context.Controller.TempData
			};
		}

		protected virtual ViewDataDictionary CreateViewData(Exception exception, ExceptionContext context)
		{
			var controller = (string)context.RouteData.Values["controller"];
			var action = (string)context.RouteData.Values["action"];

			return new ViewDataDictionary<HandleErrorInfo>(new HandleErrorInfo(exception, controller, action));
		}

		/// <summary>
		/// Determines if the view exists. Override in an inherited class to implement a custom view finding scheme. 
		/// </summary>
		/// <param name="exceptionType">The type of exception that was thrown.</param>
		/// <param name="controllerContext">The current controllercontext.</param> 
		/// <returns>True if the view is found, otherwise false.</returns>
		protected virtual bool ViewExists(Type exceptionType, ControllerContext controllerContext)
		{
			string viewName = "Rescues/" + exceptionType.Name;
			var viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
			return viewResult.View != null;
		}

		/// <summary>
		/// Creates the ViewContext to be used when rendering the rescue.
		/// </summary>
		/// <param name="exception">The exception which will become the ViewData.</param>
		/// <param name="controllerContext">The current controllercpontext.</param>
		/// <returns>A ViewContext object.</returns>
		protected virtual ViewContext CreateViewContext(Exception exception, ControllerContext controllerContext)
		{
			//todo  determin how to call into the view engine to locate a view. changed from string viewname to IView
			return new ViewContext(controllerContext, null, new ViewDataDictionary(exception),
			                       controllerContext.Controller.TempData);
		}

		/// <summary>
		/// The view to render.
		/// </summary>
		public string ViewName
		{
			get { return "Rescues/" + _view; }
			protected set { _view = value; }
		}

		/// <summary>
		/// The exception types used by this rescue.
		/// </summary>
		public Type[] ExceptionsTypes
		{
			get { return _exceptionsTypes; }
		}

		/// <summary>
		/// When false, only the specified rescue will be called.
		/// When true, rescues with a name matching the exception will be called.
		/// </summary>
		public bool AutoLocate { get; set; }

		/// <summary>
		/// When true, ajax calls are not rescued (default).
		/// When false, rescue will handle ajax calls.
		/// </summary>
		public bool IgnoreAjax { get; set; }

		/// <summary>
		/// Collection of exception types to be explicitly ignored.
		/// </summary>
		public Type[] IgnoreTypes { get; set; }
	}
}