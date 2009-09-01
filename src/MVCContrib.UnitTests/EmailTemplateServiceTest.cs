using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Services;
using NUnit.Framework;
using Rhino.Mocks;
using MvcViewEngines = System.Web.Mvc.ViewEngines;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class EmailTemplateServiceTest
	{
		private ControllerContext _controllerContext;
		private EmailTemplateService _service;

		private delegate void RenderViewDelegate(ViewContext context, TextWriter writer);

		[SetUp]
		public void Setup()
		{
			MvcViewEngines.Engines.Clear();
			MvcViewEngines.Engines.Add(MockRepository.GenerateMock<IViewEngine>());

			_controllerContext = new ControllerContext
			{
				HttpContext = MvcMockHelpers.DynamicHttpContextBase(),
				Controller = MockRepository.GenerateStub<ControllerBase>()
			};

            _controllerContext.Controller.ViewData = new ViewDataDictionary();
            _controllerContext.Controller.TempData = new TempDataDictionary();

			_service = new EmailTemplateService();

			Response.Stub(x => x.Filter).PropertyBehavior();
			Response.Expect(x => x.ContentEncoding).Return(Encoding.UTF8);
		}

		private HttpResponseBase Response
		{
			get { return _controllerContext.HttpContext.Response; }
		}

		private void WriteToStream(Stream stream, string content)
		{
			var writer = new StreamWriter(stream, Encoding.UTF8);
			writer.Write(content);
			writer.Flush();
		}

		#region Message Rendering

		private void SetupView(string viewName, string viewContents)
		{
			var view = MockRepository.GenerateMock<IView>();

			var viewEngine = MvcViewEngines.Engines[0];
			viewEngine
				.Expect(x => x.FindPartialView(_controllerContext, viewName, true))
				.Return(new ViewEngineResult(view, viewEngine));

			view.Expect(x => x.Render(Arg<ViewContext>.Is.Anything, Arg<TextWriter>.Is.Equal(Response.Output))).Do(
					new RenderViewDelegate((context, stream) => WriteToStream(Response.Filter, viewContents)));
		}

		[Test]
		public void CanRenderMessage()
		{
			int numberOfTimesFlushed = 0;

			Response.Stub(x => x.Flush()).Do(new Action(() => numberOfTimesFlushed++));

			string messageBody = "test message body..." + Environment.NewLine;
			SetupView("foo", messageBody);

			var message = _service.RenderMessage(_controllerContext, "foo");

			Assert.IsNotNull(message);
			Assert.AreEqual(messageBody, message.Body);
			Assert.IsFalse(message.IsBodyHtml);
			Assert.AreEqual(2, numberOfTimesFlushed);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowIfViewCouldNotBeFound()
		{
			MvcViewEngines.Engines[0]
				.Stub(x => x.FindPartialView(_controllerContext, null, true))
				.IgnoreArguments()
				.Return(new ViewEngineResult(new[] { "foo", "bar" }));

			_service.RenderMessage(_controllerContext, "foo");
		}

		[Test]
		public void CanRenderHtmlMessage()
		{
			string messageBody = "<html> <body> <p><b>test</b> message body...</p></body></html>" + Environment.NewLine;
			SetupView("foo", messageBody);
			var message = _service.RenderMessage(_controllerContext, "foo");

			Assert.AreEqual(messageBody, message.Body);
			Assert.IsTrue(message.IsBodyHtml);
		}

		[Test]
		public void CanPreserveResponseFilter()
		{
			var streamStub = MockRepository.GenerateStub<Stream>();
			Response.Filter = streamStub;
			SetupView("foo", "bar");

			_service.RenderMessage(_controllerContext, "foo");

			Assert.AreSame(streamStub, Response.Filter);
		}

		[Test]
		public void CanPreserveReponseFilterOnException()
		{
			var streamStub = MockRepository.GenerateStub<Stream>();
			Response.Filter = streamStub;

			try
			{
				_service.RenderMessage(_controllerContext, "foo");
			}
			catch
			{
			}

			Assert.AreSame(streamStub, Response.Filter);
		}

		#endregion

		#region Message Header Processing

		private MailMessage CanProcessMessageHeaders(string header, string value)
		{
			string messageBody = String.Format("{0}: {1}{2}test message body...", header, value, Environment.NewLine);
			SetupView("foo", messageBody);
			var message = _service.RenderMessage(_controllerContext, "foo");
			return message;
		}

		[Test]
		public void CanProcessSubjectHeader()
		{
			MailMessage message = CanProcessMessageHeaders("subject", "test-subject");
			Assert.AreEqual("test-subject", message.Subject);
		}

		[Test]
		public void CanProcessToHeader()
		{
			MailMessage message = CanProcessMessageHeaders("to", "test@test.com");
			Assert.AreEqual("test@test.com", message.To[0].Address);
		}

		[Test]
		public void CanProcessFromHeader()
		{
			MailMessage message = CanProcessMessageHeaders("from", "test@test.com");
			Assert.AreEqual("test@test.com", message.From.Address);
		}

		[Test]
		public void CanProcessCcHeader()
		{
			MailMessage message = CanProcessMessageHeaders("cc", "test@test.com");
			Assert.AreEqual("test@test.com", message.CC[0].Address);
		}

		[Test]
		public void CanProcessBccHeader()
		{
			MailMessage message = CanProcessMessageHeaders("bcc", "test@test.com");
			Assert.AreEqual("test@test.com", message.Bcc[0].Address);
		}

		[Test]
		public void CanProcessReplyToHeader()
		{
			MailMessage message = CanProcessMessageHeaders("Reply-To", "test@test.com");
			Assert.AreEqual("test@test.com", message.ReplyTo.Address);
		}

		[Test]
		public void CanProcessGenericHeader()
		{
			MailMessage message = CanProcessMessageHeaders("X-Spam", "no");
			Assert.AreEqual("no", message.Headers["X-Spam"]);
		}

		[Test]
		public void CanProcessToHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("to", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.To[0].Address);
			Assert.AreEqual("John Doe", message.To[0].DisplayName);
		}

		[Test]
		public void CanProcessFromHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("from", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.From.Address);
			Assert.AreEqual("John Doe", message.From.DisplayName);
		}

		[Test]
		public void CanProcessCcHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("cc", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.CC[0].Address);
			Assert.AreEqual("John Doe", message.CC[0].DisplayName);
		}

		[Test]
		public void CanProcessBccHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("bcc", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.Bcc[0].Address);
			Assert.AreEqual("John Doe", message.Bcc[0].DisplayName);
		}

		[Test]
		public void CanProcessReplyToHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("Reply-To", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.ReplyTo.Address);
			Assert.AreEqual("John Doe", message.ReplyTo.DisplayName);
		}

		#endregion
	}
}