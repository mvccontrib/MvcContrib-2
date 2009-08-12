using System.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class HttpContextExtensionsTester
	{
		private MockRepository _mocks;
        private HttpContextBase _context;

		[SetUp]
        public void Initialize()
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicHttpContextBase();
			_mocks.ReplayAll();
		}

		[Test]
		public void Is_Ajax_Recognizes_Ajax_Header()
		{
			_context.Request.Headers["Ajax"] = "true";

			Assert.IsTrue(_context.Request.IsAjax(), "Request was not recognized as an ajax request");
		}

		[Test]
		public void Is_Ajax_Recognizes_XMLHttpRequest_Header()
		{
			_context.Request.Headers["X-Requested-With"] = "XMLHttpRequest";

			Assert.IsTrue(_context.Request.IsAjax(), "Request was not recognized as an ajax request");
		}
	}
}
