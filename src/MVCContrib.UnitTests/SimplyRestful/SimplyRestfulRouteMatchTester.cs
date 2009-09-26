using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using MvcContrib.SimplyRestful;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.SimplyRestful
{
	public class SimplyRestfulRouteMatchTester
	{
		[TestFixture]
		public class WhenThereAreNoAreas : BaseSimplyRestfulRouteMatchFixture
		{
			protected override string ControllerPath
			{
				get { return "{controller}"; }
			}
		}

		[TestFixture]
		public class WhenThereIsAnAdminArea : BaseSimplyRestfulRouteMatchFixture
		{
			protected override string ControllerPath
			{
				get { return "admin/{controller}"; }
			}

			protected override string UrlFormat
			{
				get { return "admin/{0}"; }
			}
		}

		[TestFixture]
		public class WhenThereIsAnAreaAndSpecificController : BaseSimplyRestfulRouteMatchFixture
		{
			protected override string UrlFormat
			{
				get { return "admin/products/specials/today"; }
			}

			protected override string ControllerPath
			{
				get { return "admin/products/specials/today"; }
			}

			protected override string ControllerName
			{
				get { return "ProductSpecials"; }
			}
		}

		public abstract class BaseSimplyRestfulRouteMatchFixture
		{
			protected HttpContextBase httpContext;
			protected HttpRequestBase httpRequest;
			protected MockRepository mocks;
			protected RouteCollection routeCollection;

			protected virtual string ControllerPath
			{
				get { return "{controller}"; }
			}

			protected virtual string ControllerName
			{
				get { return null; }
			}

			protected virtual string UrlFormat
			{
				get { return "{0}"; }
			}

			[SetUp]
			public virtual void Setup()
			{
				mocks = new MockRepository();
				httpContext = mocks.DynamicMock<HttpContextBase>();
				httpRequest = mocks.DynamicMock<HttpRequestBase>();
				routeCollection = new RouteCollection();
				BuildRoutes(routeCollection);
			}

			protected virtual void BuildRoutes(RouteCollection routes)
			{
				SimplyRestfulRouteHandler.BuildRoutes(routes, ControllerPath, SimplyRestfulRouteHandler.MatchPositiveInteger, ControllerName);
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingAGetRequest_SetsTheShowAction()
			{
				using(mocks.Record())
				{
					SetupContext("/123", "GET", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("show").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdAndEditActionUsingAGetRequest_SetsTheEditAction()
			{
				using(mocks.Record())
				{
					SetupContext("/123/edit", "GET", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("edit").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdAndNewActionUsingAGetRequest_SetsTheNewAction()
			{
				using(mocks.Record())
				{
					SetupContext("/new", "GET", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("new").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdAndDeleteActionUsingAGetRequest_SetsTheDeleteAction()
			{
				using(mocks.Record())
				{
					SetupContext("/123/delete", "GET", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("delete").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerUsingAGetRequest_SetsTheIndexAction()
			{
				using(mocks.Record())
				{
					SetupContext("", "GET", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("index").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerUsingAPostRequest_SetsTheCreateAction()
			{
				using(mocks.Record())
				{
					SetupContext("", "POST", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("create").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingHttpPut_SetsTheUpdateAction()
			{
				using(mocks.Record())
				{
					SetupContext("/123", "PUT", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("update").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingHttpMethodDelete_SetsTheDestroyAction()
			{
				using(mocks.Record())
				{
					SetupContext("/123", "DELETE", null);
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.Values["action"], Is.EqualTo("destroy").IgnoreCase);
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingFormMethodPut_UsesSimplyRestfulHandler()
			{
				using(mocks.Record())
				{
					SetupContext("/123", "POST", "PUT");
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.RouteHandler, Is.TypeOf(typeof(SimplyRestfulRouteHandler)));
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingFormMethodDelete_UsesSimplyRestfulHandler()
			{
				using(mocks.Record())
				{
					SetupContext("/123", "POST", "DELETE");
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.RouteHandler, Is.TypeOf(typeof(SimplyRestfulRouteHandler)));
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingHttpGetWithAnyIdValidator_SetsTheShowAction()
			{
				routeCollection = new RouteCollection();
				SimplyRestfulRouteHandler.BuildRoutes(routeCollection, ControllerPath, SimplyRestfulRouteHandler.MatchAny, ControllerName);

				using(mocks.Record())
				{
					SetupContext("/123", "POST", "DELETE");
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData.RouteHandler, Is.TypeOf(typeof(SimplyRestfulRouteHandler)));
					AssertController(routeData);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingHttpGetWithAStringId_DoesNotMatch()
			{
				using(mocks.Record())
				{
					SetupContext("/Goose-Me", "POST", "DELETE");
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData, Is.Null);
				}
			}

			[Test]
			public virtual void GetRouteData_WithAControllerAndIdUsingHttpGetWithStringIdValidatorAndANumericId_DoesNotMatch()
			{
				routeCollection = new RouteCollection();
				SimplyRestfulRouteHandler.BuildRoutes(routeCollection, ControllerPath, "[a-zA-Z]+", ControllerName);

				using(mocks.Record())
				{
					SetupContext("/123", "POST", "DELETE");
				}
				using(mocks.Playback())
				{
					RouteData routeData = routeCollection.GetRouteData(httpContext);
					Assert.That(routeData, Is.Null);
				}
			}

			protected virtual void AssertController(RouteData data)
			{
				if(!string.IsNullOrEmpty(ControllerName))
					Assert.That(data.Values["controller"], Is.EqualTo(ControllerName));
			}

			protected virtual void SetupContext(string url, string httpMethod, string formMethod)
			{
				SetupResult.For(httpContext.Request).Return(httpRequest);
				
				SetupResult.For(httpRequest.AppRelativeCurrentExecutionFilePath)
					.Return("~/" + string.Format(UrlFormat, ControllerName ?? "products") + url);

				SetupResult.For(httpRequest.HttpMethod).Return(httpMethod);
				
				if(!string.IsNullOrEmpty(formMethod))
				{
					var form = new NameValueCollection {{"_method", formMethod}};
				    SetupResult.For(httpRequest.Form).Return(form);
				}
			}
		}
	}
}
