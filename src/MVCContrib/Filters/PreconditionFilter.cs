using System;
using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <author>Troy DeMonbreun</author>
	/// <summary>
	/// This is an action attribute base class that defines a required RouteData or Request parameter
	/// "precondition" for the action. On precondition failure, the specified Exception type will be thrown.
    /// More info <see href="http://blog.troyd.net/ASPNET+MVC+Controller+Action+Precondition+Filter+V2+Now+Part+Of+MVCContrib+Project.aspx">here</see>.
	/// </summary>

	[AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Interface, AllowMultiple = true)]
	public abstract class PreconditionFilter : ActionFilterAttribute
	{

		protected string _paramName; //Name of key to validate
		protected ParamType _paramType; //Type of key to validate
		protected Type _exceptionToThrow; //Exception to throw on failed validation
		protected string _thrownExceptionMessage = String.Empty; //Exception message (to be set in subclasses)

		public enum ParamType
		{
			RouteData = 0,
			Request = 1
		}

		public override void OnActionExecuting(ActionExecutingContext executingContext)
		{

			if (FailedValidation(executingContext))
			{

				if (typeof(Exception).IsAssignableFrom(_exceptionToThrow))
				{
					var ex = (Exception)_exceptionToThrow.GetConstructor(new[] { typeof(String) }).Invoke(new object[] { _thrownExceptionMessage });
					throw ex;
				}

			}

		}

		protected abstract bool FailedValidation(ActionExecutingContext executingContext);

	}
}
