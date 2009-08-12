using System;
using System.IO;
using System.Web.Hosting;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XslTemplateTest
	{
		private const string view = "MyView";
		private VirtualPathProvider virtualPathProvider;

		[SetUp]
		public void SetUp()
		{
			virtualPathProvider = new XsltTestVirtualPathProvider();
		}

		[Test]
		public void CreateTransformer()
		{
            var template = new XsltTemplate(virtualPathProvider, "~/Views/MyController/MyView.xslt");
			Assert.IsNotNull(template.XslTransformer);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void XsltTemplate_DependsOn_VirtualPathProvider()
		{
			var template = new XsltTemplate(null, view);
		}
	}
}
