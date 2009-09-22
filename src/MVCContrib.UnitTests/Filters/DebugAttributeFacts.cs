using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class DebugAttributeFacts
	{
		private DebugAttribute _filter;
		private ActionExecutedContext _mockFilterContext;
		private HttpContextBase _mockHttpContext;
		private HttpRequestBase _mockRequest;
		private HttpResponseBase _mockResponse;
		private MockRepository _mocks;
		[Datapoint]
		public CookiesData DebugPresent;
		[Datapoint]public CookiesData DebugNotPresent;

		public DebugAttributeFacts()
		{
			DebugPresent = new CookiesData {Cookies = new HttpCookieCollection {new HttpCookie("debug", "1")},DebugCookieWasPresent = true};
			DebugNotPresent = new CookiesData {Cookies = new HttpCookieCollection(), DebugCookieWasPresent = false};
		}

		[SetUp]
		public void TestSetup() {
		_mocks = new MockRepository();
			_mockHttpContext = _mocks.StrictMock<HttpContextBase>();
			_mockRequest = _mocks.StrictMock<HttpRequestBase>();
			_mockResponse = _mocks.StrictMock<HttpResponseBase>();
			_mockFilterContext = _mocks.StrictMock<ActionExecutedContext>();
			_filter = new DebugAttribute();
			_mocks.ReplayAll();
			_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
			_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
		}

		[Test]
		public void OnActionExecuting_DoesNothing()
		{
			var context = _mocks.Stub<ActionExecutingContext>();
			context.Replay();
			_filter.OnActionExecuting(context);
			Assert.IsTrue(true);
		}

		[Test]
		public void WhenDebugIsNullOrEmptyInQueryString_NothingShouldHappen()
		{
			_mockRequest.Expect(r => r.QueryString).Return(new NameValueCollection());
			_filter.OnActionExecuted(_mockFilterContext);
			_mocks.VerifyAll();
		}

		public class CookiesData
		{
			public HttpCookieCollection Cookies { get; set; }
			public bool DebugCookieWasPresent { get; set; }
		}

		[Theory]
		public void WhenDebugIs1InQueryString_CookieShouldBeCreatedAndAddedToResponse(CookiesData data)
		{
			_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
			_mockRequest.Expect(r => r.QueryString).Return(new NameValueCollection { { "debug", "1" } });
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockResponse.Expect(r => r.Cookies).Return(data.Cookies);
			if (!data.DebugCookieWasPresent)
			{
				_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext).Repeat.Times(2);
				_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
				_mockRequest.Expect(r => r.Url).Return(new Uri("http://foo.com"));
				_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
				_mockResponse.Expect(r => r.Cookies).Return(data.Cookies);
			}

			_filter.OnActionExecuted(_mockFilterContext);
			
			if (!data.DebugCookieWasPresent)
			{
				var result = data.Cookies["debug"];
				Assert.IsNotNull(result);
				Assert.AreEqual("1", result.Value);
			}
			_mocks.VerifyAll();
		}

		[Theory]
		public void WhenDebugIs0InQueryString_CookieShouldBeRemovedFromResponseIfPresent(CookiesData data)
		{
			_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
			_mockRequest.Expect(r => r.QueryString).Return(new NameValueCollection { { "debug", "0" } });
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockResponse.Expect(r => r.Cookies).Return(data.Cookies);
			if (data.DebugCookieWasPresent)
			{
				_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
				_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
				_mockResponse.Expect(r => r.Cookies).Return(data.Cookies);
			}
			
			_filter.OnActionExecuted(_mockFilterContext);

			if (data.DebugCookieWasPresent)
			{
				Assert.IsNull(data.Cookies["debug"]);
			}
			_mocks.VerifyAll();
		}
	}
}