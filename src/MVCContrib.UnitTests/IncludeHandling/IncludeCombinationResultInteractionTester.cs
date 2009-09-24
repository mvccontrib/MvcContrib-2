using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeCombinationResultInteractionTester
	{
		private ControllerContext _controllerContext;
		private IncludeCombination _cssCombination;
		private HttpCachePolicyBase _mockCachePolicy;
		private ControllerBase _mockController;
		private HttpContextBase _mockHttpContext;
		private HttpRequestBase _mockRequest;
		private HttpResponseBase _mockResponse;
		private MockRepository _mocks;
		private IIncludeCombiner _stubCombiner;

		[Datapoint]
		public Req AnActualBrowserHandlingGzip;
		[Datapoint] 
		public Req AnActualBrowserHandlingGzipDeflateInThatOrder;

		[Datapoint] public Req AnActualBrowserHandlingDeflateGzipInThatOrder;
		[Datapoint] public Req AnActualBrowserHandlingDeflate;
		[Datapoint] public Req IeHandlingGzipAndFailingMiserably;
		[Datapoint] public Req AnyBrowserHandlingMangledAcceptEncodingHeader;

		public IncludeCombinationResultInteractionTester()
		{
			AnActualBrowserHandlingGzip = new Req {AcceptEncoding = "gzip", ExpectedContentEncoding = "gzip", Browser = "AnActualBrowser", BrowserMajorVersion = 3};
			AnActualBrowserHandlingGzipDeflateInThatOrder = new Req { AcceptEncoding = "gzip,deflate", ExpectedContentEncoding = "gzip", Browser = "AnActualBrowser", BrowserMajorVersion = 3 };
			AnActualBrowserHandlingDeflateGzipInThatOrder = new Req { AcceptEncoding = "deflate,gzip", ExpectedContentEncoding = "gzip", Browser = "AnActualBrowser", BrowserMajorVersion = 3 };
			AnActualBrowserHandlingDeflate = new Req { AcceptEncoding = "deflate", ExpectedContentEncoding = "deflate", Browser = "AnActualBrowser", BrowserMajorVersion = 3 };
			IeHandlingGzipAndFailingMiserably = new Req { AcceptEncoding = "gzip", ExpectedContentEncoding = null, Browser = "IE", BrowserMajorVersion = 5 };
			AnyBrowserHandlingMangledAcceptEncodingHeader = new Req { AcceptEncoding = "mangled", ExpectedContentEncoding = null, Browser = "Anything", BrowserMajorVersion = 3 };
		}

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_mockHttpContext = _mocks.StrictMock<HttpContextBase>();
			_mockController = _mocks.StrictMock<ControllerBase>();
			_mockResponse = _mocks.StrictMock<HttpResponseBase>();
			_mockRequest = _mocks.StrictMock<HttpRequestBase>();
			_mockCachePolicy = _mocks.StrictMock<HttpCachePolicyBase>();
			_controllerContext = new ControllerContext(_mockHttpContext, new RouteData(), _mockController);
			_stubCombiner = _mocks.Stub<IIncludeCombiner>();
			_mocks.ReplayAll();
			_cssCombination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#foo{color:red}", DateTime.UtcNow, new CssTypeElement());
		}

		[Test]
		public void WhenNoCombinationExists_ResponseCodeShouldBe404()
		{
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockResponse.Expect(r => r.ContentEncoding = Encoding.UTF8);
			_mockResponse.Expect(r => r.StatusCode = (int) HttpStatusCode.NotFound);
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(null);

			var result = new IncludeCombinationResult(_stubCombiner, "foo", DateTime.UtcNow);
			result.ExecuteResult(_controllerContext);

			_mocks.VerifyAll();
		}

		[Test]
		public void WhenCombinationExists_ShouldCorrectlySetUpResponse()
		{
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
			_mockRequest.Expect(r => r.Headers[HttpHeaders.AcceptEncoding]).Return("");
			_mockResponse.Expect(r => r.ContentEncoding = Encoding.UTF8);
			_mockResponse.Expect(r => r.ContentType = MimeTypes.TextCss);
			_mockResponse.Expect(r => r.AddHeader(HttpHeaders.ContentLength, "16"));
			_mockResponse.Expect(r => r.OutputStream).Return(new MemoryStream(8092)).Repeat.Twice();
			_mockResponse.Expect(r => r.Cache).Return(_mockCachePolicy);
			_mockCachePolicy.Expect(cp => cp.SetETag(Arg<string>.Matches(etag => etag.StartsWith("foo") && etag.EndsWith(_cssCombination.LastModifiedAt.Ticks.ToString()))));
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(_cssCombination);

			var result = new IncludeCombinationResult(_stubCombiner, "foo", DateTime.UtcNow);
			result.ExecuteResult(_controllerContext);

			_mocks.VerifyAll();
		}

		public class Req
		{
			public string AcceptEncoding { get; set; }
			public string ExpectedContentEncoding { get; set; }
			public string Browser { get; set; }
			public int BrowserMajorVersion { get; set; }
		}

		[Theory]
		public void WhenRequestAcceptsCompression_ShouldAppendContentEncodingHeader(Req data)
		{
			var lastModifiedAt = DateTime.UtcNow;
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
			_mockRequest.Expect(r => r.Headers[HttpHeaders.AcceptEncoding]).Return(data.AcceptEncoding);
			var stubBrowser = MockRepository.GenerateStub<HttpBrowserCapabilitiesBase>();
			stubBrowser.Expect(b => b.Type).Return(data.Browser);
			stubBrowser.Expect(b => b.MajorVersion).Return(data.BrowserMajorVersion);
			stubBrowser.Replay();
			_mockRequest.Expect(r => r.Browser).Return(stubBrowser);
			_mockResponse.Expect(r => r.ContentEncoding = Encoding.UTF8);
			_mockResponse.Expect(r => r.ContentType = MimeTypes.TextCss);
			_mockResponse.Expect(r => r.AddHeader(Arg<string>.Is.Equal(HttpHeaders.ContentLength), Arg<string>.Is.NotNull));
			_mockResponse.Expect(r => r.OutputStream).Return(new MemoryStream(8092)).Repeat.Twice();
			_mockResponse.Expect(r => r.Cache).Return(_mockCachePolicy);
			if (data.ExpectedContentEncoding != null)
			{
				_mockResponse.Expect(r => r.AppendHeader(HttpHeaders.ContentEncoding, data.ExpectedContentEncoding));
			}
			_mockCachePolicy.Expect(cp => cp.SetETag(Arg<string>.Matches(etag => etag.StartsWith("foo") && etag.EndsWith(_cssCombination.LastModifiedAt.Ticks.ToString()))));
			var cacheFor = TimeSpan.FromMinutes(30);
			_mockCachePolicy.Expect(cp => cp.SetCacheability(HttpCacheability.Public));
			_mockCachePolicy.Expect(cp => cp.SetExpires(DateTime.UtcNow.Add(cacheFor))).IgnoreArguments();
			_mockCachePolicy.Expect(cp => cp.SetMaxAge(cacheFor)).IgnoreArguments();
			_mockCachePolicy.Expect(cp => cp.SetValidUntilExpires(true));
			_mockCachePolicy.Expect(cp => cp.SetLastModified(lastModifiedAt)).IgnoreArguments();
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(_cssCombination);

			var stubSettings = _mocks.Stub<IIncludeHandlingSettings>();
			stubSettings.Expect(x => x.Types[IncludeType.Css]).Return(new CssTypeElement());
			stubSettings.Replay();
			var result = new IncludeCombinationResult(_stubCombiner, "foo", DateTime.UtcNow, stubSettings);
			result.ExecuteResult(_controllerContext);

			_mocks.VerifyAll();
		}

		[Test]
		public void WhenCacheForIsSet_ShouldAppendCacheHeaders()
		{
			var lastModifiedAt = DateTime.UtcNow;
			var combination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#Foo{color:red;}", lastModifiedAt, new CssTypeElement());
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
			_mockRequest.Expect(r => r.Headers[HttpHeaders.AcceptEncoding]).Return("");
			_mockResponse.Expect(r => r.ContentEncoding = Encoding.UTF8);
			_mockResponse.Expect(r => r.ContentType = MimeTypes.TextCss);
			_mockResponse.Expect(r => r.AddHeader(HttpHeaders.ContentLength, "16"));
			_mockResponse.Expect(r => r.OutputStream).Return(new MemoryStream(8092)).Repeat.Twice();
			_mockResponse.Expect(r => r.Cache).Return(_mockCachePolicy);
			_mockCachePolicy.Expect(cp => cp.SetETag(Arg<string>.Matches(etag => etag.StartsWith("foo") && etag.EndsWith(combination.LastModifiedAt.Ticks.ToString()))));
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(combination);

			var cacheFor = TimeSpan.FromMinutes(30);
			_mockCachePolicy.Expect(cp => cp.SetCacheability(HttpCacheability.Public));
			_mockCachePolicy.Expect(cp => cp.SetExpires(lastModifiedAt.Add(cacheFor)));
			_mockCachePolicy.Expect(cp => cp.SetMaxAge(cacheFor));
			_mockCachePolicy.Expect(cp => cp.SetValidUntilExpires(true));
			_mockCachePolicy.Expect(cp => cp.SetLastModified(lastModifiedAt));
			var stubSettings = _mocks.Stub<IIncludeHandlingSettings>();
			var stubCss = _mocks.Stub<IIncludeTypeSettings>();
			stubSettings.Replay();
			stubCss.Replay();
			stubSettings.Expect(s => s.Types[IncludeType.Css]).Return(stubCss);
			stubCss.Expect(s => s.CacheFor).Return(cacheFor);

			var result = new IncludeCombinationResult(_stubCombiner, "foo", lastModifiedAt, stubSettings);
			result.ExecuteResult(_controllerContext);

			_mocks.VerifyAll();
		}
	}
}