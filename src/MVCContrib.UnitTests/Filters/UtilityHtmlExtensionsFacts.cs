using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Filters;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class UtilityHtmlExtensionsFacts
	{
		private readonly HtmlHelper _html;
		private readonly HttpRequestBase _mockRequest;
		private readonly MockRepository _mocks;
		private readonly ViewDataDictionary _viewData;

		public UtilityHtmlExtensionsFacts()
		{
			_mocks = new MockRepository();
			_viewData = new ViewDataDictionary();
			_mockRequest = _mocks.DynamicMock<HttpRequestBase>();
			_html = WebTestUtility.BuildHtmlHelper(_mocks, _viewData, _mockRequest);
			_mocks.ReplayAll();
		}

		public static IEnumerable<object[]> DebugMode
		{
			get
			{
				yield return new object[] { new NameValueCollection(), new HttpCookieCollection(), false };
				yield return new object[] { new NameValueCollection { { "debug", null } }, new HttpCookieCollection(), false };
				yield return new object[] { new NameValueCollection { { "debug", "0" } }, new HttpCookieCollection(), false };
				yield return new object[] { new NameValueCollection { { "debug", "1" } }, new HttpCookieCollection(), true };

				yield return new object[] { new NameValueCollection(), new HttpCookieCollection { new HttpCookie("foo") }, false };
				yield return new object[] { new NameValueCollection(), new HttpCookieCollection { new HttpCookie("debug", "0") }, false };
				yield return new object[] { new NameValueCollection(), new HttpCookieCollection { new HttpCookie("debug", "1") { Expires = DateTime.UtcNow.AddDays(-1) } }, false };
				yield return new object[] { new NameValueCollection(), new HttpCookieCollection { new HttpCookie("debug", "0") { Expires = DateTime.UtcNow.AddDays(1) } }, false };
				yield return new object[] { new NameValueCollection(), new HttpCookieCollection { new HttpCookie("debug", "1") { Expires = DateTime.UtcNow.AddDays(1) } }, true };
			}
		}

		[Theory]
		[PropertyData("DebugMode")]
		public void IsInDebugMode_ShouldBeCorrect(NameValueCollection queryString, HttpCookieCollection cookies, bool expected)
		{
			_mockRequest.Expect(r => r.QueryString).Return(queryString);
			_mockRequest.Expect(r => r.Cookies).Return(cookies);

			Assert.AreEqual(expected, _html.IsInDebugMode());
		}
	}
}