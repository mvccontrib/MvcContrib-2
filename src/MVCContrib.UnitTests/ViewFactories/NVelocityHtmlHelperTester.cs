using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ViewEngines;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web.Mvc.Html;
namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class NVelocityHtmlHelperTester
	{
		private MockRepository _mocks;
		private HtmlHelper _htmlHelper;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			var httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
			var httpServer = _mocks.DynamicMock<HttpServerUtilityBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpContext.Server).Return(httpServer);
			SetupResult.For(httpServer.HtmlEncode(null)).IgnoreArguments().Return(string.Empty);
			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<ControllerBase>();
			var controllerContext = new ControllerContext(requestContext, controller);
		    var view = _mocks.DynamicMock<IView>();
			_mocks.ReplayAll();
			var viewContext =
				new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary());

			_htmlHelper = new HtmlHelper(viewContext, new ViewPage());
		}

		[Test]
		public void TextBox_Passes_Through_Attributes()
		{
			var htmlAttributes = new Hashtable();
			htmlAttributes["attr"] = "value";

			var expected = _htmlHelper.TextBox("htmlName", string.Empty, new { attr = "value" }).ToHtmlString();
			var actual = _htmlHelper.TextBox("htmlName", htmlAttributes);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TextBox_Passes_Through_Attributes_With_Value()
		{
			var htmlAttributes = new Hashtable();
			htmlAttributes["attr"] = "value";

			var expected = _htmlHelper.TextBox("htmlName", "value", new { attr = "value" }).ToHtmlString();
			var actual = _htmlHelper.TextBox("htmlName", "value", htmlAttributes);
			Assert.AreEqual(expected, actual);
		}
	}
}
