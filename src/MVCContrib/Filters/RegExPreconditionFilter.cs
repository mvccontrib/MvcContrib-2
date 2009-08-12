using System;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace MvcContrib.Filters
{
	/// <author>Troy DeMonbreun</author>
	/// <summary>
	/// This is an action attribute that defines (via a regular expression) a required RouteData or Request
	/// parameter "precondition" for the action. On precondition failure, the specified Exception type will be thrown.
    /// More info <see href="http://blog.troyd.net/ASPNET+MVC+Controller+Action+Precondition+Filter+V2+Now+Part+Of+MVCContrib+Project.aspx">here</see>.
	/// </summary>
	/// <example>
	/// <code>
	/// [RegExPreconditionFilter("id", PreconditionFilter.ParamType.RouteData, "^[1-9][0-9]*$", typeof(ArgumentOutOfRangeException))]
	/// OR
	/// [RegExPreconditionFilter("id", PreconditionFilter.ParamType.Request, "^[1-9][0-9]*$", typeof(ArgumentOutOfRangeException))]
	/// </code>
	/// </example>

	public class RegExPreconditionFilter : PreconditionFilter
	{

		protected string _regExPattern;

		/// <summary>
		/// Attribute constructor
		/// </summary>
		/// <param name="paramName">Name of key to validate</param>
		/// <param name="paramType">Type of key to validate</param>
		/// <param name="regExPattern">Regular expression to be matched</param>
		/// <param name="exceptionToThrow">Exception to throw on failed validation</param>
		public RegExPreconditionFilter(string paramName, ParamType paramType, string regExPattern, Type exceptionToThrow)
		{
			_paramName = paramName;
			_paramType = paramType;
			_regExPattern = regExPattern;
			_exceptionToThrow = exceptionToThrow;
			_thrownExceptionMessage = Enum.GetName(typeof(ParamType), paramType) + " parameter '" + paramName + "' does not match regex " + regExPattern;
		}

		/// <summary>
		/// Signals failure if RouteData key does not exist, RouteData key value is null or empty string,
		/// or regular expression does not match RouteData key value
		/// </summary>
		protected override bool FailedValidation(ActionExecutingContext executingContext)
		{

			switch (_paramType)
			{
				case ParamType.RouteData:

					return !executingContext.RouteData.Values.ContainsKey(_paramName)
					   || executingContext.RouteData.Values[_paramName] == null
					   || !Regex.IsMatch(executingContext.RouteData.Values[_paramName].ToString(), _regExPattern);

				case ParamType.Request:

					return executingContext.HttpContext.Request.Params[_paramName] == null
					   || !Regex.IsMatch(executingContext.HttpContext.Request.Params[_paramName], _regExPattern);

				default:

					return false;
			}


		}

	}

}
