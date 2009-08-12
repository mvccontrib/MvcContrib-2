using System;
using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <author>Troy DeMonbreun</author>
	/// <summary>
	/// This is an action attribute that defines (via a predicate method) a required RouteData or Request
	/// parameter "precondition" for the action. On precondition failure, the specified Exception type will be thrown.
    /// More info <see href="http://blog.troyd.net/ASPNET+MVC+Controller+Action+Precondition+Filter+V2+Now+Part+Of+MVCContrib+Project.aspx">here</see>.
	/// </summary>
	/// <example>
	/// <code>
	/// [PredicatePreconditionFilter("id", PreconditionFilter.ParamType.RouteData, "IsGreaterThanZero", typeof(ArgumentOutOfRangeException))]
	/// OR
	/// [PredicatePreconditionFilter("id", PreconditionFilter.ParamType.Request, "IsGreaterThanZero", typeof(ArgumentOutOfRangeException))]
	/// </code>
	/// </example>

	public class PredicatePreconditionFilter : PreconditionFilter
	{

		protected string _predicateMethod;

		/// <summary>
		/// Attribute constructor
		/// </summary>
		/// <param name="paramName">Name of key to validate</param>
		/// <param name="paramType">Type of key to validate</param>
		/// <param name="predicateMethod">Predicate&lt;object&gt; method that encapsulates validation logic</param>
		/// <param name="exceptionToThrow">Exception to throw on failed validation</param>
		public PredicatePreconditionFilter(string paramName, ParamType paramType, string predicateMethod, Type exceptionToThrow)
		{
			_paramName = paramName;
			_paramType = paramType;
			_predicateMethod = predicateMethod;
			_exceptionToThrow = exceptionToThrow;
			_thrownExceptionMessage = Enum.GetName(typeof(ParamType), paramType) + " parameter '" + paramName + "' does not satisfy predicate method " + predicateMethod;
		}

		/// <summary>
		/// Signals failure if RouteData key does not exist, RouteData key value is null,
		/// or predicate evaluates to false
		/// </summary>
		protected override bool FailedValidation(ActionExecutingContext executingContext)
		{

			//convert predicate into callable form
			var predicate = (Predicate<object>)Delegate.CreateDelegate(typeof(Predicate<object>), executingContext.Controller, _predicateMethod);

			switch (_paramType)
			{
				case ParamType.RouteData: //default

					return !executingContext.RouteData.Values.ContainsKey(_paramName)
						|| executingContext.RouteData.Values[_paramName] == null
						|| !predicate(executingContext.RouteData.Values[_paramName]);

				case ParamType.Request:

					return executingContext.HttpContext.Request.Params[_paramName] == null
						|| !predicate(executingContext.HttpContext.Request.Params[_paramName]);

				default:

					return false;
			}

		}

	}

}
