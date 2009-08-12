using System.Web;
using System.Web.Hosting;

namespace MvcContrib.UI.InputBuilder
{
    public class AssemblyResourceVirtualFile : VirtualFile
    {
        string path;
        public AssemblyResourceVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            path = VirtualPathUtility.ToAppRelative(virtualPath);
        }
        public override System.IO.Stream Open()
        {
			System.Diagnostics.Trace.WriteLine("Opening " + path);
            string resourceName = path.Replace("~/", "MvcContrib.UI.InputBuilder.").Replace("/", ".");            

            System.Reflection.Assembly assembly = this.GetType().Assembly;               
                        
            return assembly.GetManifestResourceStream(resourceName);
            
        }
    }
}