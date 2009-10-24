using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace MvcContrib.UI.InputBuilder.ViewEngine
{
	public class AssemblyResource
	{
		public string VirtualPath { get; set; }
		public string Namespace { get; set; }
		public Type TypeToLocateAssembly { get; set; }
		public string GetFullyQualifiedTypeFromPath(string path)
		{
			string replace = path.ToLower().Replace("~/", Namespace.ToLower());
			if(!string.IsNullOrEmpty(VirtualPath))
				replace = replace.Replace(VirtualPath.ToLower(), "");
			return replace.Replace("/", ".").ToLower();
		}

		public string GetResourceNameFromPath(string path)
		{
			var name = GetFullyQualifiedTypeFromPath(path);
			return TypeToLocateAssembly.Assembly.GetManifestResourceNames().Where(s => s.ToLower().Equals(name)).FirstOrDefault();
		}


	}
	public class AssemblyResourceProvider : VirtualPathProvider
	{
		static AssemblyResourceProvider()
		{
			ResourcePaths = new Dictionary<string, AssemblyResource>();
			var resource = new AssemblyResource() { VirtualPath = "/views/inputbuilders", TypeToLocateAssembly = typeof(AssemblyResourceProvider), Namespace = "MvcContrib.UI.InputBuilder." };
			AddResource(resource);
		}
		public static void AddResource(AssemblyResource assemblyResource)
		{
			ResourcePaths.Add(assemblyResource.VirtualPath, assemblyResource);
		}
		public AssemblyResourceProvider()
		{
			
		}

		private static Dictionary<string, AssemblyResource> ResourcePaths { get; set; }

		public bool IsAppResourcePath(string virtualPath)
		{
			
			String checkPath = VirtualPathUtility.ToAppRelative(virtualPath).ToLower();
			foreach(var resourcePath in ResourcePaths)
			{
				if(checkPath.Contains(resourcePath.Key) && 
					ResourceExists(resourcePath.Value,checkPath))
					return true;
			}
			return false;
		}

		private bool ResourceExists(AssemblyResource assemblyResource, string path)
		{
			var name = assemblyResource.GetFullyQualifiedTypeFromPath(path);
			return assemblyResource.TypeToLocateAssembly.Assembly.GetManifestResourceNames().Any(s => s.ToLower().Equals(name));
		}


		public AssemblyResource GetResource(string virtualPath)
		{
			String checkPath = VirtualPathUtility.ToAppRelative(virtualPath).ToLower();
			foreach (var resourcePath in ResourcePaths)
			{
				if (checkPath.Contains(resourcePath.Key))
					return resourcePath.Value;
			}
			return null;
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
				var resource = GetResource(virtualPath);// ResourcePaths[virtualPath.ToLower()];
				return new AssemblyResourceVirtualFile(virtualPath, resource);
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
					virtualPathDependencies.OfType<string>().Where(s => !s.ToLower().Contains("/views/inputbuilders")).ToArray();
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