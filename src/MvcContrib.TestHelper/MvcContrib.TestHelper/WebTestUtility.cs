using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace MvcContrib.TestHelper
{
	public static class WebTestUtility
	{
		public static HtmlHelper BuildHtmlHelper(MockRepository mocks, ViewDataDictionary viewData, HttpRequestBase request)
		{
			var mockHttpContext = mocks.DynamicMock<HttpContextBase>();
			var mockControllerContext = mocks.DynamicMock<ControllerContext>(
				mockHttpContext,
				new RouteData(),
				mocks.DynamicMock<ControllerBase>());

			var mockView = mocks.DynamicMock<IView>();
			var mockViewContext = mocks.DynamicMock<ViewContext>(
				mockControllerContext,
				mockView,
				viewData,
				new TempDataDictionary());

			var mockViewDataContainer = mocks.DynamicMock<IViewDataContainer>();

			mockViewDataContainer.Expect(v => v.ViewData).Return(viewData);
			if (request != null)
			{
				mockViewContext.Expect(vc => vc.View).Return(mockView);
				mockViewContext.Expect(vc => vc.HttpContext).Return(mockHttpContext);
				mockHttpContext.Expect(hc => hc.Request).Return(request);
			}

			return new HtmlHelper(mockViewContext, mockViewDataContainer);
		}
	}
}