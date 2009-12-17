using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.BrailViewEngine;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	[TestFixture]
	[Category("BrailViewEngine")]
	public class BooViewEngineTester
	{
		private BooViewEngine _viewEngine;
		private ViewContext _viewContext;
		private HttpContextBase _httpContext;
		private MockRepository _mocks;
		private StringWriter _output;
		private Controller _controller;

		private static readonly string VIEW_ROOT_DIRECTORY = @"BrailViewEngine\Views";
	    private ControllerContext controllerContext;

	    [SetUp]
		public void SetUp()
		{
			_output = new StringWriter();
			_mocks = new MockRepository();
			_httpContext = _mocks.DynamicMock<HttpContextBase>(); //new TestHttpContext();
			var response = _mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(response.Output).Return(_output);
			SetupResult.For(_httpContext.Request).Return(_mocks.DynamicMock<HttpRequestBase>());
			SetupResult.For(_httpContext.Response).Return(response);
//			SetupResult.For(_httpContext.Session).Return(_mocks.DynamicMock<HttpSessionStateBase>());
			var requestContext = new RequestContext(_httpContext, new RouteData());
            _controller = _mocks.StrictMock<Controller>();
			_mocks.Replay(_controller);
			
			controllerContext = new ControllerContext(requestContext, _controller);
//			_viewContext = new ViewContext(controllerContext, null, new ViewDataDictionary(), null);

			_viewEngine = new BooViewEngine
			              	{
			              		ViewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY),
			              		Options = new BooViewEngineOptions()
			              	};
			_viewEngine.Initialize();
			_mocks.Replay(_httpContext);
			
		}

		[Test]
		public void Can_Render_View_With_Master_And_SubView()
		{
			_mocks.ReplayAll();

			string expected = "Master View SubView";
			string actual = GetViewOutput("view", "/Master");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Request_ApplicationPath_Is_Placed_In_ViewData_With_SiteRoot_Key()
		{
			SetupResult.For(_httpContext.Request.ApplicationPath).Return("/ApplicationPath");
			_mocks.ReplayAll();

			string expected = "Current apppath is /ApplicationPath/";
			string actual = GetViewOutput("apppath");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Changing_View_Causes_Recompile()
		{
			SetupResult.For(_httpContext.Request.ApplicationPath).Return("/ApplicationPath");
			_mocks.ReplayAll();

			IViewSource viewSource = _viewEngine.ViewSourceLoader.GetViewSource("apppath.brail");
			string originalSource;
			using(TextReader reader = File.OpenText(viewSource.FullName))
			{
				originalSource = reader.ReadToEnd();
			}

			string expected = "Current apppath is /ApplicationPath/";
			string actual = GetViewOutput("apppath");
			Assert.AreEqual(expected, actual);

			string newSource = "newSource";
			using(TextWriter writer = File.CreateText(viewSource.FullName))
			{
				writer.Write(newSource);
			}

			Thread.Sleep(100);
			
			//TODO: Clear output
			_output.GetStringBuilder().Remove(0, _output.GetStringBuilder().Length);
			//_httpContext.Response.ClearOutput();
			actual = GetViewOutput("apppath");

			try
			{
				Assert.AreEqual(newSource, actual);
			}
			finally
			{
				using (TextWriter writer = File.CreateText(viewSource.FullName))
				{
					writer.Write(originalSource);
				}
			}
		}

		[Test]
		public void Can_Render_SubView_with_custom_ViewData()
		{
			_mocks.ReplayAll();
			string expected = "View Test";
			string actual = GetViewOutput("view_CustomViewData");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Layout_And_View_Should_Have_ViewContext()
		{
			_mocks.ReplayAll();
			BrailBase view = _viewEngine.Process("view", "/Master");
            _viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());             
            view.Render(_viewContext, _httpContext.Response.Output);
			Assert.IsNotNull(view.ViewContext);
			Assert.AreEqual(view.ViewContext, view.Layout.ViewContext);
		}

		[Test]
		public void Should_Use_Custom_Base_Class()
		{
			_mocks.ReplayAll();
			_viewEngine.Options.AssembliesToReference.Add(System.Reflection.Assembly.Load("MVCContrib.UnitTests"));
			_viewEngine.Options.BaseType = "MvcContrib.UnitTests.BrailViewEngine.TestBrailBase";
			BrailBase view = _viewEngine.Process("view", null);
			Assert.IsInstanceOf<TestBrailBase>(view);
		}

		private string GetViewOutput(string viewName)
		{
			return GetViewOutput(viewName, null);
		}

		private string GetViewOutput(string viewName, string masterName)
		{
			BrailBase view = _viewEngine.Process(viewName, masterName);
            _viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()); 
            view.Render(_viewContext, _httpContext.Response.Output);
			return _httpContext.Response.Output.ToString();
		}
	}

	public abstract class TestBrailBase : BrailBase
	{
		protected TestBrailBase(BooViewEngine viewEngine) : base(viewEngine)
		{
		}
	}
}
