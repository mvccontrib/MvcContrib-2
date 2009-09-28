using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace MvcContrib.UI.InputBuilder
{
	public class AssemblyResourceVirtualFile : VirtualFile
	{
		private string path;

		public AssemblyResourceVirtualFile(string virtualPath)
			: base(virtualPath)
		{
			path = VirtualPathUtility.ToAppRelative(virtualPath);
		}

		public override Stream Open()
		{
			Trace.WriteLine("Opening " + path);
			string resourceName = path.Replace("~/", "MvcContrib.UI.InputBuilder.").Replace("/", ".");

			Assembly assembly = GetType().Assembly;

			return assembly.GetManifestResourceStream(resourceName);
		}
	}
}