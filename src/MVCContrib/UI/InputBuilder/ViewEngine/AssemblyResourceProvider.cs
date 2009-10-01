using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace MvcContrib.UI.InputBuilder.ViewEngine
{
	public class AssemblyResourceProvider : VirtualPathProvider
	{
		public bool IsAppResourcePath(string virtualPath)
		{
			String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
			return checkPath.StartsWith("~/Views/Inputbuilders/",
			                            StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool FileExists(string virtualPath)
		{
			bool exists = base.FileExists(virtualPath);
			return exists ? exists : IsAppResourcePath(virtualPath);
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			if(IsAppResourcePath(virtualPath))
			{
				return new AssemblyResourceVirtualFile(virtualPath);
			}
			else
			{
				return base.GetFile(virtualPath);
			}
		}

		public override CacheDependency
			GetCacheDependency(string virtualPath,
			                   IEnumerable virtualPathDependencies,
			                   DateTime utcStart)
		{
			if(IsAppResourcePath(virtualPath))
			{
				return null;
			}
			else
			{
				string[] dependencies =
					virtualPathDependencies.OfType<string>().Where(s => !s.Contains("/Views/InputBuilders")).ToArray();
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