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
	public class UtilityHtmlExtensionsTester
	{
		[Datapoint] public DebugMode NothingInQueryString;
		[Datapoint]
		public DebugMode BlankInQueryString;
		[Datapoint]
		public DebugMode ZeroInQueryString;
		[Datapoint]
		public DebugMode OneInQueryString;
		[Datapoint]
		public DebugMode JunkInCookies;
		[Datapoint]
		public DebugMode ZeroInCookies;
		[Datapoint]
		public DebugMode ExpiredCookieButOne;
		[Datapoint]
		public DebugMode NotExpiredZeroInCookies;
		[Datapoint]
		public DebugMode NotExpiredOneInCookies;

		public UtilityHtmlExtensionsTester()
		{
			NothingInQueryString = new DebugMode { QueryString = new NameValueCollection(), Cookies = new HttpCookieCollection(), Expected = false };
			BlankInQueryString = new DebugMode { QueryString = new NameValueCollection { { "debug", null } }, Cookies = new HttpCookieCollection(), Expected = false };
			ZeroInQueryString = new DebugMode { QueryString = new NameValueCollection { { "debug", "0" } }, Cookies = new HttpCookieCollection(), Expected = false };
			OneInQueryString = new DebugMode { QueryString = new NameValueCollection { { "debug", "1" } }, Cookies = new HttpCookieCollection(), Expected = true };

			JunkInCookies = new DebugMode { QueryString = new NameValueCollection(), Cookies = new HttpCookieCollection { new HttpCookie("foo") }, Expected = false };
			ZeroInCookies = new DebugMode { QueryString = new NameValueCollection(), Cookies = new HttpCookieCollection { new HttpCookie("debug", "0") }, Expected = false };
			ExpiredCookieButOne = new DebugMode
					{
						QueryString = new NameValueCollection(),
						Cookies = new HttpCookieCollection { new HttpCookie("debug", "1") { Expires = DateTime.UtcNow.AddDays(-1) } },
						Expected = false
					};
			NotExpiredZeroInCookies = new DebugMode
					{
						QueryString = new NameValueCollection(),
						Cookies = new HttpCookieCollection { new HttpCookie("debug", "0") { Expires = DateTime.UtcNow.AddDays(1) } },
						Expected = false
					};
			NotExpiredOneInCookies = new DebugMode
			{
				QueryString = new NameValueCollection(),
				Cookies = new HttpCookieCollection { new HttpCookie("debug", "1") { Expires = DateTime.UtcNow.AddDays(1) } },
				Expected = true
					};

		}

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_viewData = new ViewDataDictionary();
			_mockRequest = _mocks.DynamicMock<HttpRequestBase>();
			_html = WebTestUtility.BuildHtmlHelper(_mocks, _viewData, _mockRequest);
			_mocks.ReplayAll();
		}

		private HtmlHelper _html;
		private HttpRequestBase _mockRequest;
		private MockRepository _mocks;
		private ViewDataDictionary _viewData;

		public class DebugMode
		{
			public NameValueCollection QueryString { get; set; }
			public HttpCookieCollection Cookies { get; set; }
			public bool Expected { get; set; }
		}
		[Theory]
		public void IsInDebugMode_ShouldBeCorrect(DebugMode data)
		{
			_mockRequest.Expect(r => r.QueryString).Return(data.QueryString);
			_mockRequest.Expect(r => r.Cookies).Return(data.Cookies);

			Assert.AreEqual(data.Expected, _html.IsInDebugMode());
		}
	}
}