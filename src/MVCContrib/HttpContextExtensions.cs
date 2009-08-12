using System;
using System.Web;

namespace MvcContrib
{
	/// <summary>
	/// Static class containing extenstion to HttpContextBase
	/// </summary>
	public static class HttpContextExtensions
	{
		/// <summary>
		/// Determines if the current request is an ajax request
		/// </summary>
		/// <param name="request">Instance of the HttpRequestBase for the request</param>
		/// <returns></returns>
		public static bool IsAjax(this HttpRequestBase request)
		{
			return (!string.IsNullOrEmpty(request.Headers["Ajax"]) ||
				"XMLHttpRequest".Equals(request.Headers["X-Requested-With"],
				StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
