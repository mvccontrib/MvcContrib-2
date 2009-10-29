using System;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Linq;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	public class XsltTestVirtualPathProvider : VirtualPathProvider
	{
		private static readonly string[] _validViews = new[]
		{
				"~/Views/MyController/MyView.xslt", 
				"~/Views/MyController/Greetings.xslt"
		};

		public override bool FileExists(string virtualPath) 
		{
			return _validViews.Contains(virtualPath);
		}

		public override VirtualFile GetFile(string virtualPath) 
		{
			return new TestVirtualFile(virtualPath);
		}

		private class TestVirtualFile : VirtualFile
		{
			string _basePath = "MvcContrib.UnitTests.XsltViewEngine.Data.Views.MyController";
			private string _viewName;

			public TestVirtualFile(string virtualPath) : base(virtualPath)
			{
				if (! _validViews.Contains(virtualPath)) {
					throw new Exception("Could not find view: " + virtualPath);
				}

				_viewName = virtualPath.Split('/').Last();
			}

			public override Stream Open()
			{
				return Assembly.GetExecutingAssembly().GetManifestResourceStream(_basePath + "." + _viewName);
			}
		}
	}
}