// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// MODIFICATIONS HAVE BEEN MADE TO THIS FILE

using MvcContrib.ViewFactories;

namespace MvcContrib.BrailViewEngine
{
	using System.Web.Mvc;
	using System.Collections;
	using Boo.Lang.Extensions;

	public class BooViewEngineOptions
	{
		private readonly IList assembliesToReference = new ArrayList();
		private readonly IList namespacesToImport = new ArrayList();
		private string commonScriptsDirectory = "CommonScripts";
		private string saveDirectory = "Brail_Generated_Code";
		private string baseType = "MvcContrib.BrailViewEngine.BrailBase";

		public BooViewEngineOptions()
		{
			AssembliesToReference.Add(typeof(BooViewEngineOptions).Assembly); //Brail's assembly
			AssembliesToReference.Add(typeof(Controller).Assembly); //MVC Framework's assembly
			AssembliesToReference.Add(typeof(AssertMacro).Assembly); //Boo.Lang.Extensions assembly
			AssembliesToReference.Add(typeof(IViewSourceLoader).Assembly); // MvcContrib assembly

			NamespacesToImport.Add("MvcContrib.UI.Html");
			NamespacesToImport.Add("MvcContrib.BrailViewEngine");
			NamespacesToImport.Add("System.Web.Mvc");
			namespacesToImport.Add("System.Web.Mvc.Html");
		}

		public bool Debug { get; set; }

		public bool SaveToDisk { get; set; }

		public bool BatchCompile { get; set; }

		public string CommonScriptsDirectory
		{
			get { return commonScriptsDirectory; }
			set { commonScriptsDirectory = value; }
		}

		public string SaveDirectory
		{
			get { return saveDirectory; }
			set { saveDirectory = value; }
		}

		public IList AssembliesToReference
		{
			get { return assembliesToReference; }
		}

		public IList NamespacesToImport
		{
			get { return namespacesToImport; }
		}

		public string BaseType
		{
			get { return baseType; }
			set { baseType = value; }
		}
	}
}
