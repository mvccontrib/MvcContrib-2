using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;
using HttpSessionStateBase = System.Web.HttpSessionStateBase;
using System.Security.Principal;

namespace MvcContrib.UnitTests
{
	public static class MvcMockHelpers
	{
		public static HttpContextBase DynamicHttpContextBase()
		{
			var mocks = new MockRepository();
			var context = mocks.DynamicHttpContextBase();
			mocks.ReplayAll();
			return context;
		}

        public static HttpContextBase DynamicHttpContextBase(string requestPath)
        {
            var mocks = new MockRepository();
			var context = mocks.DynamicHttpContextBase();
            SetupResult.For(context.Request.Path).Return(requestPath);
            mocks.ReplayAll();
            return context;
        }

		public static HttpContextBase DynamicHttpContextBase(this MockRepository mocks)
		{
			return mocks.DynamicHttpContextBase
				(mocks.DynamicHttpRequestBase(),
				 mocks.DynamicHttpResponseBase(),
				 mocks.DynamicHttpSessionStateBase(),
				 mocks.DynamicHttpServerUtilityBase(),
				 mocks.DynamicIPrincipal());
		}

		public static HttpContextBase DynamicHttpContextBase(this MockRepository mocks,
			HttpRequestBase request,
			HttpResponseBase response,
			HttpSessionStateBase session,
			HttpServerUtilityBase server,
			IPrincipal user)
		{
			var context = mocks.DynamicMock<HttpContextBase>();
			SetupResult.For(context.User).Return(user);
			SetupResult.For(context.Request).Return(request);
			SetupResult.For(context.Response).Return(response);
			SetupResult.For(context.Session).Return(session);
			SetupResult.For(context.Server).Return(server);
			mocks.Replay(context);
			return context;
		}

        public static HttpRequestBase DynamicHttpRequestBase(this MockRepository mocks)
		{
			var request = mocks.DynamicMock<HttpRequestBase>();
            var browser = mocks.DynamicMock<HttpBrowserCapabilitiesBase>();
						var form = new NameValueCollection();
						var queryString = new NameValueCollection();
						var cookies = new HttpCookieCollection();
						var serverVariables = new NameValueCollection();
						var headers = new NameValueCollection();

			SetupResult.For(request.Form).Return(form);
			SetupResult.For(request.QueryString).Return(queryString);
			SetupResult.For(request.Cookies).Return(cookies);
			SetupResult.For(request.ServerVariables).Return(serverVariables);
			SetupResult.For(request.Params).Do((Func<NameValueCollection>)(() => CreateParams(queryString, form, cookies, serverVariables)));
		    SetupResult.For(request.Browser).Return(browser);
			SetupResult.For(request.Headers).Return(headers);

            return request;
		}

		public static NameValueCollection CreateParams(NameValueCollection queryString, NameValueCollection form, HttpCookieCollection cookies, NameValueCollection serverVariables)
		{
			NameValueCollection parms = new NameValueCollection(48);
			parms.Add(queryString);
			parms.Add(form);
			for( var i=0; i<cookies.Count; i++)
			{
				var cookie = cookies.Get(i);
				parms.Add(cookie.Name, cookie.Value);
			}
			parms.Add(serverVariables);
			return parms;
		}

		public static HttpResponseBase DynamicHttpResponseBase(this MockRepository mocks)
		{
			var response = mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(response.OutputStream).Return(new MemoryStream());
			SetupResult.For(response.Output).Return(new StringWriter());
			SetupResult.For(response.ContentType).PropertyBehavior();
            SetupResult.For(response.StatusCode).PropertyBehavior();
            SetupResult.For(response.RedirectLocation).PropertyBehavior();

            return response;
		}
		public static HttpSessionStateBase DynamicHttpSessionStateBase(this MockRepository mocks)
		{
			var session = mocks.DynamicMock<HttpSessionStateBase>();
			return session;
		}
		public static HttpServerUtilityBase DynamicHttpServerUtilityBase(this MockRepository mocks)
		{
			var server = mocks.DynamicMock<HttpServerUtilityBase>();
			return server;
		}
		public static IPrincipal DynamicIPrincipal(this MockRepository mocks)
		{
			var principal = mocks.DynamicMock<IPrincipal>();
			return principal;
		}

        public static ViewContext DynamicViewContext(this MockRepository mocks, string viewName)
        {
            var httpContext = DynamicHttpContextBase(mocks);
            var controller = mocks.DynamicMock<ControllerBase>();
            var view = mocks.DynamicMock<IView>();
            mocks.Replay(controller);

            var controllerContext = new ControllerContext(httpContext, new RouteData(), controller);            
            
            return new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
        }

		public static TController SetupControllerContext<TController>(this TController controller)  where TController : Controller
		{
			var controllerContext = new ControllerContext(DynamicHttpContextBase(), new RouteData(), controller);
			controller.ControllerContext = controllerContext;
			return controller;
		}
	}
}
