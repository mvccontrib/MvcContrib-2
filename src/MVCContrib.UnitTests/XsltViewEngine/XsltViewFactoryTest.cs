using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XsltViewFactoryTest : ViewTestBase
	{
		private ControllerBase _fakeController;

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
            _fakeController = MockRepository.GenerateStub<ControllerBase>();
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void ThrowExceptionWhenDataTypeIsInvalid()
		{
			var routeData = new RouteData();
			routeData.Values["controller"] = "MyController";
            
			XsltViewFactory viewFactory = new XsltViewFactory(new XsltTestVirtualPathProvider());

            viewFactory.FindView(new ControllerContext(HttpContext, routeData, _fakeController), "MyView", null,false);
		}
	}
}
