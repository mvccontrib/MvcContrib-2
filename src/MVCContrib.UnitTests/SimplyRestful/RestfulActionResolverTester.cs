using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using MvcContrib.SimplyRestful;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.SimplyRestful
{
	[TestFixture]
	public class RestfulActionResolverTester
	{
		private HttpContextBase _httpContext;
		private HttpRequestBase _httpRequest;
		private RouteData _routeData;
		private NameValueCollection _form;
		private RequestContext _requestContext;
		private IRestfulActionResolver resolver;

		[SetUp]
		public void Setup()
		{
			_httpContext = MockRepository.GenerateStub<HttpContextBase>();
			_httpRequest = MockRepository.GenerateStub<HttpRequestBase>();
			resolver = new RestfulActionResolver();
		}

		[Test, ExpectedException(typeof(NullReferenceException))]
		public void ResolveAction_WithNullRequest_Throws()
		{
			_httpContext.Stub(r => r.Request).Return(null);
			_requestContext = new RequestContext(_httpContext, new RouteData());

			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
		}

		[Test]
		public void ResolveAction_WithEmptyRequestHttpMethod_ReturnsNoAction()
		{
			GivenContext("", null);
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
		}

		[Test]
		public void ResolveAction_WithNonPostRequest_ReturnsNoAction()
		{
			GivenContext("GET", null);
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
		}

		[Test]
		public void ResolveAction_WithPostRequestAndNullForm_ReturnsNoAction()
		{
			GivenContext("POST", null);
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
		}

		[Test]
		public void ResolveAction_WithPostRequestAndEmptyFormMethodValue_ReturnsNoAction()
		{
			GivenContext("POST", "");
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
		}

		[Test]
		public void ResolveAction_WithPostRequestAndInvalidFormMethodValue_ReturnsNoAction()
		{
			GivenContext("POST", "GOOSE");
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
		}

		[Test]
		public void ResolveAction_WithPostRequestAndFormMethodValuePUT_ReturnsUpdateAction()
		{
			GivenContext("POST", "PUT");
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.Update));
		}

		[Test]
		public void ResolveAction_WithPostRequestAndFormMethodValueDELETE_ReturnsDestroyAction()
		{
			GivenContext("POST", "DELETE");
			Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.Destroy));
		}

		private void GivenContext(string httpMethod, string formMethod)
		{
			_httpContext.Stub(c => c.Request).Return(_httpRequest).Repeat.Any();
			_httpRequest.Stub(r => r.HttpMethod).Return(httpMethod).Repeat.Any();

			_routeData = new RouteData();
			_routeData.Values.Add("controller", "testcontroller");
			_routeData.Values.Add("action", "SomeWeirdAction");

			if(formMethod != null)
			{
				_form = new NameValueCollection {{"_method", formMethod}};
				_httpRequest.Stub(r => r.Form).Return(_form).Repeat.Any();
			}

			_requestContext = new RequestContext(_httpContext, _routeData);
		}
	}
}