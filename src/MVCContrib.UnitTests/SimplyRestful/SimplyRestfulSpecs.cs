using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using MvcContrib.SimplyRestful;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.SimplyRestful
{
	[TestFixture]
	[Category("SimplyRestfulSpecs")]
	public class When_The_Form_Is_Posted_With_A_Form_Field_Named_Method_And_A_Value_Of_PUT
		: BaseRouteHandlerTestFixture
	{
		[SetUp]
		protected override void GivenSetupContext()
		{
			base.GivenSetupContext();
			form.Add("_method", "PUT");
		}

		[Test]
		public void Then_The_Route_Action_Should_Be_Set_To_Update()
		{
			RestfulAction action = RestfulAction.None;
			IRestfulActionResolver resolver = new RestfulActionResolver();

			using(mocks.Record())
			{
				SetupResult.For(httpContext.Request).Return(httpRequest);
				SetupResult.For(httpRequest.HttpMethod).Return("POST");
				SetupResult.For(httpRequest.Form).Return(form);
				requestContext = new RequestContext(httpContext, routeData);
			}

			using(mocks.Playback())
			{
				action = resolver.ResolveAction(requestContext);
				Assert.That(action, Is.EqualTo(RestfulAction.Update));
			}
		}
	}

	[TestFixture]
	[Category("SimplyRestfulSpecs")]
	public class When_The_Form_Is_Posted_With_A_Form_Field_Named_Method_And_A_Value_Of_DELETE
		: BaseRouteHandlerTestFixture
	{
		[SetUp]
		protected override void GivenSetupContext()
		{
			base.GivenSetupContext();
			form.Add("_method", "DELETE");
		}

		[Test]
		public void Then_The_Route_Action_Should_Be_Set_To_Destroy()
		{
			RestfulAction action = RestfulAction.None;
			IRestfulActionResolver resolver = new RestfulActionResolver();

			using(mocks.Record())
			{
				SetupResult.For(httpContext.Request).Return(httpRequest);
				SetupResult.For(httpRequest.HttpMethod).Return("POST");
				SetupResult.For(httpRequest.Form).Return(form);
				requestContext = new RequestContext(httpContext, routeData);
			}

			using(mocks.Playback())
			{
				action = resolver.ResolveAction(requestContext);
				Assert.That(action, Is.EqualTo(RestfulAction.Destroy));
			}
		}
	}

	public abstract class BaseRouteHandlerTestFixture
	{
		protected MockRepository mocks;
		protected HttpContextBase httpContext;
		protected HttpRequestBase httpRequest;
		protected RouteData routeData;
		protected RequestContext requestContext;
		protected NameValueCollection form;

		protected virtual void GivenSetupContext()
		{
			mocks = new MockRepository();
			httpContext = mocks.DynamicMock<HttpContextBase>();
			httpRequest = mocks.DynamicMock<HttpRequestBase>();

			routeData = new RouteData();
			routeData.Values.Add("controller", "testcontroller");
			routeData.Values.Add("action", "update");

			form = new NameValueCollection();
		}
	}
}
