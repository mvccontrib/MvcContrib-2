using System.Collections;
using System.IO;
using Boo.Lang;
using MvcContrib.BrailViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.BrailViewEngine
{

	[TestFixture]
	[Category("BrailViewEngine")]
	public class XmlExtenstionTester
	{
		private XmlExtension _xml;

		[SetUp]
		public void SetUp()
		{
			_xml = new XmlExtension(new StringWriter());
		}

		[Test]
		public void Can_Output_Empty_Tag()
		{
			_xml.Tag("a");
			_xml.Flush();

			Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?><a />", _xml.Output.ToString());
		}

		[Test]
		public void Can_Output_Tag_With_Block()
		{
			_xml.Tag("a", new Callable(_xml));
			_xml.Flush();

			Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?><a>Text</a>", _xml.Output.ToString());
		}

		[Test]
		public void Can_Output_Tag_With_Attributes()
		{
			IDictionary attributes = new Hashtable();
			attributes["href"] = "http://mvccontrib.org";
			_xml.Tag("a", attributes, null);
			_xml.Flush();

			Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?><a href=\"http://mvccontrib.org\" />", _xml.Output.ToString());
		}

		class Callable : ICallable
		{
			private readonly XmlExtension _xml;

			public Callable(XmlExtension xml)
			{
				_xml = xml;
			}

			public object Call(object[] args)
			{
				_xml.text("Text");
				return null;
			}
		}
	}
}
