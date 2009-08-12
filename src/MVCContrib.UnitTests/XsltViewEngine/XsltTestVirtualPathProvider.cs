using System;
using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	public class XsltTestVirtualPathProvider : VirtualPathProvider
	{
		public override bool FileExists(string virtualPath) 
		{
			if (virtualPath == "~/Views/MyController/MyView.xslt")
			{
				return true;
			}

			return false;
		}

		public override VirtualFile GetFile(string virtualPath) 
		{
			return new TestVirtualFile(virtualPath);
		}

		private class TestVirtualFile : VirtualFile
		{
			string xsltViewPath = "MvcContrib.UnitTests.XsltViewEngine.Data.Views.MyController.MyView.xslt";

			public TestVirtualFile(string virtualPath) : base(virtualPath)
			{
				if (virtualPath != "~/Views/MyController/MyView.xslt") {
					throw new Exception("Could not find view: " + virtualPath);
				}
			}

			public override Stream Open()
			{
				return Assembly.GetExecutingAssembly().GetManifestResourceStream(xsltViewPath);
			}
		}
	}
}