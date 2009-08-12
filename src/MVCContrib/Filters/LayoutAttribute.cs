using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <summary>
	/// Decorates a controller class to denote the default layout for all actions on the controller.
	/// Use:  define a layout at the class level (without an extension) and omit the layout from each return View() call.
	/// This will override the masterName selection from your individual actions.  To override this attribute at the action level, 
	/// apply the attribute to that action.
	/// 
	/// To specify no layout, use [Layout(null)] or [Layout("")].
	/// </summary>
	public class LayoutAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// The default layout.  Do not specify the extension.
		/// </summary>
		public string Layout { get; private set; }

		/// <summary>
		/// Creates a LayoutAttribute.
		/// </summary>
		/// <param name="layout">The default layout to use.  Do not specify the extension.</param>
		public LayoutAttribute(string layout)
		{
			Layout = layout;
		}

		/// <summary>
		/// Captures the result executing to inject a default layout.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			base.OnResultExecuting(filterContext);

			//are we rendering a view?
			var viewResult = filterContext.Result as ViewResult;
			if(viewResult != null)
			{
                //override what the view (might have) set
			    viewResult.MasterName = Layout;
			}
		}
	}
}