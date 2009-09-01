using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using MvcContrib.XsltViewEngine.Messages;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XsltViewTest : ViewTestBase
	{
		private VirtualPathProvider virtualPathProvider;
		private ControllerContext _context;

		public override void SetUp()
		{
			base.SetUp();
			var routeData = new RouteData();
			routeData.Values["controller"] = "MyController";
			virtualPathProvider = new XsltTestVirtualPathProvider();
			_context = new ControllerContext(HttpContext, routeData, MockRepository.GenerateStub<ControllerBase>());
            _context.Controller.ViewData = new ViewDataDictionary();
			mockRepository.ReplayAll();
		}

		[Test]
		public void RenderViewTest()
		{
			var vData = new XsltViewData();
			string expectedSnippet = "<Root><MyElementID>1</MyElementID></Root>";
			var dataSource = new XslDataSource(new MockXslDataSource(expectedSnippet));
			vData.DataSources.Add(dataSource);
			vData.Messages.Add(new Message(MessageType.Info, "This is a message"));
			vData.Messages.Add(new Message(MessageType.Info, "This is a message for a control", "controlID"));
			vData.Messages.Add(new InfoMessage("This is an info message"));
			vData.Messages.Add(new InfoMessage("This is an info message", "controlId2"));
			vData.Messages.Add(new InfoHtmlMessage("This is a html message"));
			vData.Messages.Add(new InfoHtmlMessage("This is a html message", "controlId3"));
			vData.Messages.Add(new ErrorHtmlMessage("This is an error html message"));
			vData.Messages.Add(new ErrorHtmlMessage("This is an error html message", "controlId4"));
			vData.Messages.Add(new ErrorMessage("This is an error message"));
			vData.Messages.Add(new ErrorMessage("This is an error message", "controlId4"));
			vData.Messages.Add(new AlertMessage("This is an alert message", "controlId4"));
			vData.Messages.Add(new AlertMessage("This is an alert message"));
			vData.Messages.Add(new AlertHtmlMessage("This is an alert html message", "controlId4"));
			vData.Messages.Add(new AlertHtmlMessage("This is an alert html message"));

			Request.QueryString["myQueryString"] = "myQueryStringValue";

		    _context.Controller.ViewData.Model = vData;

            var viewFactory = new XsltViewFactory(virtualPathProvider);
			var viewResult = viewFactory.FindView(_context, "MyView", null,false);

			Assert.IsNotNull(viewResult.View);

            var viewContext = new ViewContext(_context, viewResult.View, new ViewDataDictionary(vData), new TempDataDictionary());
            

			viewResult.View.Render(viewContext, Response.Output);
            
            string actual = Response.Output.ToString().Replace("\r\n", "");

			XmlDocument xDoc = LoadXmlDocument("ViewTest.xml");

			string expected = xDoc.OuterXml;

			Assert.AreEqual(expected, actual);
		}
	}
}
