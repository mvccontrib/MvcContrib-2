using System.Xml;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XslDataSourceTest
	{
		[Test]
		public void Instantiate()
		{
			string formatString = "<{0}><MyElementID>1</MyElementID></{0}>";
			string expectedRootName = "MyRoot";
			string expectedSnippet = string.Format(formatString, expectedRootName);
			var dataSource = new XslDataSource(expectedRootName, new MockXslDataSource(formatString));

			Assert.IsNotNull(dataSource);
			Assert.IsTrue(dataSource.DataSource is MockXslDataSource);
			Assert.AreEqual(expectedRootName, dataSource.RootName);
			Assert.AreEqual(expectedSnippet, dataSource.XmlFragment.OuterXml);
		}

		[Test]
		public void Instantiate_WithoutRootName()
		{
			string expectedSnippet = "<Root><MyElementID>1</MyElementID></Root>";
			var dataSource = new XslDataSource(new MockXslDataSource(expectedSnippet));

			Assert.IsNotNull(dataSource);
			Assert.IsTrue(dataSource.DataSource is MockXslDataSource);
			Assert.AreEqual(expectedSnippet, dataSource.XmlFragment.OuterXml);
		}
	}

	internal class MockXslDataSource : IXmlConvertible
	{
		private readonly string formatString;

		public MockXslDataSource(string formatString)
		{
			this.formatString = formatString;
		}

		#region IXmlConvertible Members

		public XmlNode ToXml()
		{
			return ToXml("MockXslDataSource");
		}

		public XmlNode ToXml(string root)
		{
			var doc = new XmlDocument();
			doc.LoadXml(string.Format(formatString, root));

			return doc.DocumentElement;
		}

		#endregion
	}
}
