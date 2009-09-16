using System.Web;

namespace MvcContrib
{
	public class HttpContextProvider : IHttpContextProvider
	{
		private readonly HttpContext _context;

		public HttpContextProvider(HttpContext context)
		{
			_context = context;
		}

		#region IHttpContextProvider Members

		public HttpContextBase Context
		{
			get { return new HttpContextWrapper(_context); }
		}

		public HttpRequestBase Request
		{
			get { return Context.Request; }
		}

		#endregion
	}
}