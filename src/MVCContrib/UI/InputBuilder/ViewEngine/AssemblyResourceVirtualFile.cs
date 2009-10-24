using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace MvcContrib.UI.InputBuilder.ViewEngine
{
	public class AssemblyResourceVirtualFile : VirtualFile
	{
		private readonly AssemblyResource _resource;
		private readonly string path;

		public AssemblyResourceVirtualFile(string virtualPath, AssemblyResource resource)
			: base(virtualPath)
		{
			_resource = resource;
			path = VirtualPathUtility.ToAppRelative(virtualPath);
		}

		public override Stream Open()
		{
			Trace.WriteLine("Opening " + path);
			string resourceNameFromPath = _resource.GetResourceNameFromPath(path);
			if(resourceNameFromPath==null)
				return null;
			return _resource.TypeToLocateAssembly.Assembly.GetManifestResourceStream(resourceNameFromPath);
		}
	}
}