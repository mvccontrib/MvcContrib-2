using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using Rhino.Mocks;
using Xunit;
using Xunit.Extensions;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class IncludeCombinationResultInteractionFacts
	{
		private readonly ControllerContext _controllerContext;
		private readonly IncludeCombination _cssCombination;
		private readonly HttpCachePolicyBase _mockCachePolicy;
		private readonly ControllerBase _mockController;
		private readonly HttpContextBase _mockHttpContext;
		private readonly HttpRequestBase _mockRequest;
		private readonly HttpResponseBase _mockResponse;
		private readonly MockRepository _mocks;
		private readonly IIncludeCombiner _stubCombiner;

		public IncludeCombinationResultInteractionFacts()
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
			_cssCombination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#foo{color:red}", Clock.UtcNow, new CssTypeElement());
		}

		[Fact]
		public void WhenNoCombinationExists_ResponseCodeShouldBe404()
		{
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockResponse.Expect(r => r.ContentEncoding = Encoding.UTF8);
			_mockResponse.Expect(r => r.StatusCode = (int) HttpStatusCode.NotFound);
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(null);

			var result = new IncludeCombinationResult(_stubCombiner, "foo", Clock.UtcNow);
			Assert.DoesNotThrow(() => result.ExecuteResult(_controllerContext));

			_mocks.VerifyAll();
		}

		[Fact]
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

			var result = new IncludeCombinationResult(_stubCombiner, "foo", Clock.UtcNow);
			Assert.DoesNotThrow(() => result.ExecuteResult(_controllerContext));

			_mocks.VerifyAll();
		}

		[Theory]
		[FreezeClock(2009,10,1,1,1,1)]
		[InlineData("gzip", "gzip", "AnActualBrowser", 3)]
		[InlineData("gzip,deflate", "gzip", "AnActualBrowser", 3)]
		[InlineData("deflate,gzip", "gzip", "AnActualBrowser", 3)]
		[InlineData("deflate", "deflate", "AnActualBrowser", 3)]
		[InlineData("gzip", null, "IE", 5)]
		[InlineData("mangled", null, "Anything", 3)]
		public void WhenRequestAcceptsCompression_ShouldAppendContentEncodingHeader(string acceptEncoding, string expectedContentEncoding, string browser, int browserMajorVersion)
		{
			var lastModifiedAt = Clock.UtcNow;
			_mockHttpContext.Expect(hc => hc.Response).Return(_mockResponse);
			_mockHttpContext.Expect(hc => hc.Request).Return(_mockRequest);
			_mockRequest.Expect(r => r.Headers[HttpHeaders.AcceptEncoding]).Return(acceptEncoding);
			var stubBrowser = MockRepository.GenerateStub<HttpBrowserCapabilitiesBase>();
			stubBrowser.Expect(b => b.Type).Return(browser);
			stubBrowser.Expect(b => b.MajorVersion).Return(browserMajorVersion);
			stubBrowser.Replay();
			_mockRequest.Expect(r => r.Browser).Return(stubBrowser);
			_mockResponse.Expect(r => r.ContentEncoding = Encoding.UTF8);
			_mockResponse.Expect(r => r.ContentType = MimeTypes.TextCss);
			_mockResponse.Expect(r => r.AddHeader(Arg<string>.Is.Equal(HttpHeaders.ContentLength), Arg<string>.Is.NotNull));
			_mockResponse.Expect(r => r.OutputStream).Return(new MemoryStream(8092)).Repeat.Twice();
			_mockResponse.Expect(r => r.Cache).Return(_mockCachePolicy);
			if (expectedContentEncoding != null)
			{
				_mockResponse.Expect(r => r.AppendHeader(HttpHeaders.ContentEncoding, expectedContentEncoding));
			}
			_mockCachePolicy.Expect(cp => cp.SetETag(Arg<string>.Matches(etag => etag.StartsWith("foo") && etag.EndsWith(_cssCombination.LastModifiedAt.Ticks.ToString()))));
			var cacheFor = TimeSpan.FromMinutes(30);
			_mockCachePolicy.Expect(cp => cp.SetCacheability(HttpCacheability.Public));
			_mockCachePolicy.Expect(cp => cp.SetExpires(Clock.UtcNow.Add(cacheFor))).IgnoreArguments();
			_mockCachePolicy.Expect(cp => cp.SetMaxAge(cacheFor)).IgnoreArguments();
			_mockCachePolicy.Expect(cp => cp.SetValidUntilExpires(true));
			_mockCachePolicy.Expect(cp => cp.SetLastModified(lastModifiedAt)).IgnoreArguments();
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(_cssCombination);

			var stubSettings = _mocks.Stub<IIncludeHandlingSettings>();
			stubSettings.Expect(x => x.Types[IncludeType.Css]).Return(new CssTypeElement());
			stubSettings.Replay();
			var result = new IncludeCombinationResult(_stubCombiner, "foo", Clock.UtcNow, stubSettings);
			Assert.DoesNotThrow(() => result.ExecuteResult(_controllerContext));

			_mocks.VerifyAll();
		}

		[Fact]
		[FreezeClock(2009, 10, 1, 1, 1, 1)]
		public void WhenCacheForIsSet_ShouldAppendCacheHeaders()
		{
			var lastModifiedAt = Clock.UtcNow;
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
			_mockCachePolicy.Expect(cp => cp.SetExpires(Clock.UtcNow.Add(cacheFor)));
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
			Assert.DoesNotThrow(() => result.ExecuteResult(_controllerContext));

			_mocks.VerifyAll();
		}
	}
}