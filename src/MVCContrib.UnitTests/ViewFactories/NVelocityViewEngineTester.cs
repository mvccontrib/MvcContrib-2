using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ViewEngines;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture]
	public class NVelocityViewEngineTester
	{
		private IViewEngine _viewEngine;
		private ControllerContext _controllerContext;
		private MockRepository _mocks;
		private StringWriter _output;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_output = new StringWriter();

			string viewPath = "MvcContrib.UnitTests.ViewFactories";

			IDictionary properties = new Hashtable();
			properties["resource.loader"] = "assembly";
			properties["assembly.resource.loader.class"] = "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader, NVelocity";
			properties["assembly.resource.loader.assembly"] = new List<string>() {"MVCContrib.UnitTests"};
			properties["master.folder"] = viewPath;
			_viewEngine = new NVelocityViewEngine(properties);

			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var response = _mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(httpContext.Response).Return(response);
			SetupResult.For(response.Output).Return(_output);

			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<ControllerBase>();
			
			_mocks.ReplayAll();

			_controllerContext = new ControllerContext(requestContext, controller);
			_controllerContext.RouteData.Values.Add("controller", viewPath);
		}

		[Test]
		public void WillAcceptNullProperties()
		{
			new NVelocityViewEngine();
			new NVelocityViewEngine(null);
		}

		[Test]
		public void LoadValidView()
		{
			NVelocityView view = (NVelocityView)_viewEngine.FindView(_controllerContext, "view", null,false).View;
			Assert.IsNotNull(view);
			Assert.IsNotNull(view.ViewTemplate);
		}

		[Test] //TODO: Preview 5 This should not throw according to the new view engine rules.
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidViewThrows()
		{
			//var context = new ViewContext(_controllerContext, "nonExistant", null, null);
			_viewEngine.FindView(_controllerContext, "nonExistant", null,false);
		}

		[Test]
		public void LoadValidViewWithMaster()
		{
			//var context = new ViewContext(_controllerContext, "view", null, null);
			NVelocityView view = (NVelocityView)_viewEngine.FindView(_controllerContext, "view", "master",false).View;
			Assert.IsNotNull(view);
			Assert.IsNotNull(view.ViewTemplate);
			Assert.IsNotNull(view.MasterTemplate);
		}

		[Test] //TODO: Preview 5 This should not throw according to the new view engine rules.
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidMasterThrows()
		{
			//var context = new ViewContext(_controllerContext, "view", null, null);
			_viewEngine.FindView(_controllerContext, "view", "nonExistant",false);
		}

		[Test]
		public void ShouldRenderView()
		{
			string expected = "Master Template View Template";

		    var view = _viewEngine.FindView(_controllerContext, "view", "master",false).View;
            var context = new ViewContext(_controllerContext, view, new ViewDataDictionary(), new TempDataDictionary());
            view.Render(context, _output);
			string output = _output.ToString();
			Assert.AreEqual(expected, output);
		}

		[Test]
		public void ReleaseView_should_not_throw()
		{
			_viewEngine.ReleaseView(null,null);	
		}
	}
}
