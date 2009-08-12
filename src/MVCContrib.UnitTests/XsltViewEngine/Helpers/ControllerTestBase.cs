using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine.Helpers
{
	public class ControllerTestBase
	{
		private readonly MockRepository mockRepository = new MockRepository();

		protected void PrepareController(Controller controller)
		{
			controller.ControllerContext =
				new ControllerContext((HttpContextBase)mockRepository.DynamicMock(typeof(HttpContextBase)), new RouteData(), controller);
		}
	}
}
