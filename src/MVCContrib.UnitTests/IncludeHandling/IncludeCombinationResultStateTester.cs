using System;
using System.Collections.Specialized;
using System.IO;
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
	public class IncludeCombinationResultStateTester
	{
		private ControllerContext _controllerContext;
		private IncludeCombination _cssCombination;
		private MockRepository _mocks;
		private HttpCachePolicyBase _stubCache;
		private IIncludeCombiner _stubCombiner;
		private ControllerBase _stubController;
		private HttpContextBase _stubHttpContext;
		private HttpRequestBase _stubRequest;
		private HttpResponseBase _stubResponse;

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_stubHttpContext = _mocks.Stub<HttpContextBase>();
			_stubController = _mocks.Stub<ControllerBase>();
			_stubRequest = _mocks.Stub<HttpRequestBase>();
			_stubResponse = _mocks.Stub<HttpResponseBase>();
			_stubCache = _mocks.Stub<HttpCachePolicyBase>();
			_controllerContext = new ControllerContext(_stubHttpContext, new RouteData(), _stubController);
			_stubCombiner = _mocks.Stub<IIncludeCombiner>();

			_mocks.ReplayAll();
			_cssCombination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#foo{color:red;}", DateTime.UtcNow, new CssTypeElement());
		}

		[Test]
		public void WhenConstructedViaCombinerAndKey_CombinationIsSet()
		{
			var mockCombiner = _mocks.StrictMock<IIncludeCombiner>();
			_mocks.ReplayAll();
			mockCombiner.Expect(c => c.GetCombination("foo")).Return(_cssCombination);
			IncludeCombinationResult result = new IncludeCombinationResult(mockCombiner, "foo", DateTime.UtcNow);
			Assert.AreEqual(_cssCombination, result.Combination);
		}

		[Test]
		public void ConstructorThrows_WhenCombinerIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new IncludeCombinationResult(null, "foo", DateTime.UtcNow));
		}

		[Datapoint] public string nothing = null;
		[Datapoint] public string empty = "";

		[Theory]
		public void ConstructorThrows_WhenKeyIsBad(string key)
		{
			Assert.Throws<ArgumentException>(() => new IncludeCombinationResult(_mocks.Stub<IIncludeCombiner>(), key, DateTime.UtcNow));
		}

		[Test]
		public void WhenCombinationContainsNoContent_ShouldNotThrow()
		{
			_stubHttpContext.Expect(hc => hc.Response).Return(_stubResponse);
			_stubHttpContext.Expect(hc => hc.Request).Return(_stubRequest);
			_stubRequest.Expect(r => r.Headers).Return(new NameValueCollection { { HttpHeaders.AcceptEncoding, "" } });
			_stubResponse.ContentEncoding = Encoding.UTF8;
			_stubResponse.ContentType = MimeTypes.TextCss;
			_stubResponse.AddHeader(HttpHeaders.ContentLength, "15");
			_stubResponse.Expect(r => r.OutputStream).Return(new MemoryStream(8092)).Repeat.Twice();
			_stubResponse.Expect(r => r.Cache).Return(_stubCache);

			var emptyCombination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "", DateTime.UtcNow, new CssTypeElement());
			_stubCombiner.Expect(c => c.GetCombination("foo")).Return(emptyCombination);
			var result = new IncludeCombinationResult(_stubCombiner, "foo", DateTime.UtcNow);
			result.ExecuteResult(_controllerContext);
		}
	}
}