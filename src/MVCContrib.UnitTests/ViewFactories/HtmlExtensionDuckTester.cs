using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcContrib.ViewEngines;
using NUnit.Framework;
using NVelocity.Runtime;
using NVelocity.Util.Introspection;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class HtmlExtensionDuckTester
	{
		private MockRepository _mocks;
		private HtmlExtensionDuck _htmlHelperDuck;
		private HtmlHelper _htmlHelper;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			var httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
		    var routeData = new RouteData();
		    routeData.Values["controller"] = "testcontroller";
		    var requestContext = new RequestContext(httpContext,
		                                                       routeData
		                                                          );
			var controller = _mocks.DynamicMock<ControllerBase>();
			var controllerContext = new ControllerContext(requestContext, controller);
		    var view = _mocks.DynamicMock<IView>();
			_mocks.ReplayAll();
            var viewPage = new ViewPage();
            var viewContext = new ViewContext(controllerContext, view,new ViewDataDictionary(), new TempDataDictionary());

		    
		    _htmlHelper = new HtmlHelper(viewContext, viewPage);
			_htmlHelperDuck = new HtmlExtensionDuck(_htmlHelper) {Introspector = new Introspector(new Logger())};
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_HtmlHelper()
		{
			new HtmlExtensionDuck(null);
		}

		[Test]
		public void Invokes_Methods_On_HtmlHelper()
		{
			string expected = _htmlHelper.ActionLink("linkText", "actionName");
			var actual = _htmlHelperDuck.Invoke("ActionLink", new object[] {"linkText", "actionName"}) as string;

			Assert.AreEqual(expected, actual); 
		}

		[Test]
		public void Invokes_Methods_On_HtmlHelper_Extension_Classes()
		{
		    string expected = _htmlHelper.TextBox("htmlName");
			var actual = _htmlHelperDuck.Invoke("TextBox", new object[] {"htmlName"}) as string;

			Assert.AreEqual(expected, actual); 
		}

		[Test]
		public void Returns_Null_For_Unresolved_Methods()
		{
			Assert.IsNull(_htmlHelperDuck.Invoke("UnresolvedMethod", new object[] {}));
		}

		class Logger : IRuntimeLogger
		{
			public void Warn(object message)
			{
			}

			public void Info(object message)
			{
			}

			public void Error(object message)
			{
			}

			public void Debug(object message)
			{
			}
		}
	}
}
