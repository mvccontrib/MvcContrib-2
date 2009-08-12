using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XmlResponseBuilderTest : ViewTestBase
	{
		private void BuildRequest(bool usePost)
		{
			if(usePost)
			{
				HttpMethodToReturn = "POST";
				Request.Form["myFormVariable"] = "myFormVariableValue";
			}
			else
			{
				HttpMethodToReturn = "GET";
				Request.QueryString["myQueryString"] = "myQueryStringValue";
			}
		}

		[Test]
		public void AppendDataSourceToResponse_Via_XmlNode()
		{
			string xml = "<MyEntities><MyEntity><ID>1</ID><Name>MyEntityName</Name></MyEntity></MyEntities>";
			BuildRequest(false);

			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithStringXmlDataSource.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();
			responseBuilder.AppendDataSourceToResponse(xml);

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void AppendDataSourceToResponse_Via_XmlReader()
		{
			string xml = "<MyEntities><MyEntity><ID>1</ID><Name>MyEntityName</Name></MyEntity></MyEntities>";
			BuildRequest(false);

			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithStringXmlDataSource.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();
			responseBuilder.AppendDataSourceToResponse(XmlReader.Create(new StringReader(xml)));

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void AppendPage_For_A_GetRequest_WithPageVars()
		{
			BuildRequest(false);

			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithPageVars.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);
			var pageVars = new Dictionary<string, string> {{"myPageVar", "pageVar"}};
			responseBuilder.InitMessageStructure();
			responseBuilder.AppendPage("", "http://mysite.com/mycontroller/mypage", pageVars);

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void CreateNewNode()
		{
			var expected = new XmlDocument();

			expected.LoadXml("<myElement myAttribute=\"attrValue\">myElementValue</myElement>");

			var responseBuilder = new XmlResponseBuilder(HttpContext);
			XmlElement actual = responseBuilder.CreateNewNode("myElement", "myElementValue", "myAttribute", "attrValue");

			Assert.IsNotNull(actual);
			Assert.AreEqual(expected.DocumentElement.InnerXml, actual.InnerXml);
		}

		[Test]
		public void CreateNewNodeExtensionMethod()
		{
			var expected = new XmlDocument();

			expected.LoadXml("<myElement myAttribute=\"attrValue\">myElementValue</myElement>");

			var message = new XmlDocument();
			XmlElement actual = message.CreateNewNode("myElement", "myElementValue", "myAttribute", "attrValue");
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected.DocumentElement.InnerXml, actual.InnerXml);
		}

		[Test]
		public void InitMessageStructure_For_A_GetRequest()
		{
			BuildRequest(false);
			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessage.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InitMessageStructure_For_A_GetRequest_WithMessages()
		{
			BuildRequest(false);
			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithMessages.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();
			responseBuilder.AddMessage("This is the message", "INFO");
			responseBuilder.AddMessage("This is a message for a control", "INFO", "controlId");

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InitMessageStructure_For_A_PostRequest()
		{
			BuildRequest(true);

			XmlDocument expected = LoadXmlDocument("ResponseBuilderPostMessage.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InitMessageStructure_For_A_PostRequest_WithALowEcmaScriptVersion()
		{
			BuildRequest(true);
			EcmaScriptVersionToReturn = new Version(0, 4);
			XmlDocument expected = LoadXmlDocument("ResponseBuilderPostMessageNoJavascript.xml");

			var responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InstantiateTest()
		{
//			Request.SetPhysicalApplicationPath(Environment.CurrentDirectory.Replace("\\", "/"));
			var responseBuilder = new XmlResponseBuilder(HttpContext);

			Assert.IsNotNull(responseBuilder);
			Assert.AreEqual("http://testing/mycontroller/test", responseBuilder.AppPath);
		}
	}
}
