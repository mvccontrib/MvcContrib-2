using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Nrws.Web;
using Rhino.Mocks;
using Xunit;
using Xunit.Extensions;

namespace Nrws.Unit.Tests.Web
{
	public class DebugFilterAttributeFacts
	{
		private readonly DebugFilterAttribute _filter;
		private readonly ActionExecutedContext _mockFilterContext;
		private readonly HttpContextBase _mockHttpContext;
		private readonly HttpRequestBase _mockRequest;
		private readonly HttpResponseBase _mockResponse;
		private readonly MockRepository _mocks;

		public DebugFilterAttributeFacts()
		{
			_mocks = new MockRepository();
			_mockHttpContext = _mocks.StrictMock<HttpContextBase>();
			_mockRequest = _mocks.StrictMock<HttpRequestBase>();
			_mockResponse = _mocks.StrictMock<HttpResponseBase>();
			_mockFilterContext = _mocks.StrictMock<ActionExecutedContext>();
			_filter = new DebugFilterAttribute();
			_mocks.ReplayAll();
			_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
			_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
		}

		public static IEnumerable<object[]> Cookies
		{
			get
			{
				yield return new object[] { new HttpCookieCollection { new HttpCookie("debug", "1") }, true };
				yield return new object[] { new HttpCookieCollection(), false };
			}
		}

		[Fact]
		public void OnActionExecuting_DoesNothing()
		{
			Assert.DoesNotThrow(() => _filter.OnActionExecuting(_mocks.Stub<ActionExecutingContext>()));
		}

		[Fact]
		public void WhenDebugIsNullOrEmptyInQueryString_NothingShouldHappen()
		{
			_mockRequest.Expect(r => r.QueryString).Return(new NameValueCollection());
			Assert.DoesNotThrow(() => _filter.OnActionExecuted(_mockFilterContext));
			_mocks.VerifyAll();
		}

		[Theory]
		[PropertyData("Cookies")]
		public void WhenDebugIs1InQueryString_CookieShouldBeCreatedAndAddedToResponse(HttpCookieCollection cookies, bool debugCookieWasPresent)
		{
			_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
			_mockRequest.Expect(r => r.QueryString).Return(new NameValueCollection { { "debug", "1" } });
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockResponse.Expect(r => r.Cookies).Return(cookies);
			if (!debugCookieWasPresent)
			{
				_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext).Repeat.Times(2);
				_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
				_mockRequest.Expect(r => r.Url).Return(new Uri("http://foo.com"));
				_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
				_mockResponse.Expect(r => r.Cookies).Return(cookies);
			}
			Assert.DoesNotThrow(() => _filter.OnActionExecuted(_mockFilterContext));
			if (!debugCookieWasPresent)
			{
				var result = cookies["debug"];
				Assert.NotNull(result);
				Assert.Equal("1", result.Value);
			}
			_mocks.VerifyAll();
		}

		[Theory]
		[PropertyData("Cookies")]
		public void WhenDebugIs0InQueryString_CookieShouldBeRemovedFromResponseIfPresent(HttpCookieCollection cookies, bool debugCookieWasPresent)
		{
			_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
			_mockRequest.Expect(r => r.QueryString).Return(new NameValueCollection { { "debug", "0" } });
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockResponse.Expect(r => r.Cookies).Return(cookies);
			if (debugCookieWasPresent)
			{
				_mockFilterContext.Expect(fc => fc.HttpContext).Return(_mockHttpContext);
				_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
				_mockResponse.Expect(r => r.Cookies).Return(cookies);
			}
			Assert.DoesNotThrow(() => _filter.OnActionExecuted(_mockFilterContext));
			if (debugCookieWasPresent)
			{
				Assert.Null(cookies["debug"]);
			}
			_mocks.VerifyAll();
		}
	}
}