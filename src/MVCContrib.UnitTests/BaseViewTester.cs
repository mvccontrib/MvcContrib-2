using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	public class BaseViewTester
	{
		protected MockRepository mocks;
		protected StringWriter _output;
		protected ViewContext _viewContext;

		protected virtual void Setup()
		{
			_output = new StringWriter();

			mocks = new MockRepository();
			var httpContext = mocks.DynamicMock<HttpContextBase>();
			var httpRequest = mocks.DynamicMock<HttpRequestBase>();
			var httpResponse = mocks.DynamicMock<HttpResponseBase>();
			var httpSessionState = mocks.DynamicMock<HttpSessionStateBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Request).Return(httpRequest);
			SetupResult.For(httpRequest.ApplicationPath).Return("/");
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpResponse.Output).Return(_output);
			SetupResult.For(httpResponse.ContentEncoding).Return(Encoding.UTF8);
			SetupResult.For(httpContext.Items).Return(new Hashtable());
			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = mocks.DynamicMock<ControllerBase>();
			var controllerContext = new ControllerContext(requestContext, controller);
		    var view = mocks.DynamicMock<IView>();
			mocks.ReplayAll();

			_viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
		}
	}
}
