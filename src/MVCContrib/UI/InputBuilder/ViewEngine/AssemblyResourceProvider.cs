using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MvcContrib.UI.InputBuilder
{
    public class AssemblyResourceProvider : System.Web.Hosting.VirtualPathProvider
    {
        
        public AssemblyResourceProvider() { }

        public bool IsAppResourcePath(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Views/Inputbuilders/",
                                        StringComparison.InvariantCultureIgnoreCase);
        }
        public override bool FileExists(string virtualPath)
        {
        	var exists = base.FileExists(virtualPath);
            return exists ? exists : IsAppResourcePath(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
                return new AssemblyResourceVirtualFile(virtualPath);
            else
                return base.GetFile(virtualPath);
        }
        public override System.Web.Caching.CacheDependency
            GetCacheDependency(string virtualPath,
                               System.Collections.IEnumerable virtualPathDependencies,
                               DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
            {
            	return null;            
            }
            else
            {
                var dependencies = virtualPathDependencies.OfType<string>().Where(s => !s.Contains("/Views/InputBuilders")).ToArray();
                return base.GetCacheDependency(virtualPath,
                                               dependencies, utcStart);
            }
        }

        public override string GetCacheKey(string virtualPath)
		{
				return null;
		}
    }
}