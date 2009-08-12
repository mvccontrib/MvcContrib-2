using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class XmlResultTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<ControllerBase>());
			_mocks.ReplayAll();
		}

		[Test]
		public void ObjectToSerialize_should_return_the_object_to_serialize()
		{
			var result = new XmlResult(new Person {Id = 1, Name = "Bob"});
			Assert.That(result.ObjectToSerialize, Is.InstanceOfType(typeof(Person)));
			Assert.That(((Person)result.ObjectToSerialize).Name, Is.EqualTo("Bob"));
		}

		[Test]
		public void Should_set_content_type()
		{
			var result = new XmlResult(new[] {2, 3, 4});
			result.ExecuteResult(_controllerContext);
			Assert.AreEqual("text/xml", _controllerContext.HttpContext.Response.ContentType);
		}

		[Test]
		public void Should_serialise_xml()
		{
			var result = new XmlResult(new Person {Id = 5, Name = "Jeremy"});
			result.ExecuteResult(_controllerContext);

			var doc = new XmlDocument();
			doc.LoadXml(_controllerContext.HttpContext.Response.Output.ToString());
			Assert.That(doc.SelectSingleNode("/Person/Name").InnerText, Is.EqualTo("Jeremy"));
			Assert.That(doc.SelectSingleNode("/Person/Id").InnerText, Is.EqualTo("5"));
		}

		public class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}
