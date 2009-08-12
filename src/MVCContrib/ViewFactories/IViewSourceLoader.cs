// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
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

//This file adapted and modified from the original
using System.IO;

namespace MvcContrib.ViewFactories
{
	public interface IViewSourceLoader
	{
		bool HasView(string viewPath);

		IViewSource GetViewSource(string viewPath);

		string ViewRootDirectory { get; set; }

		string[] ListViews(string directoryName);

		event FileSystemEventHandler ViewRootDirectoryChanged;
	}

	public interface IViewSource
	{
		Stream OpenViewStream();

		string FullName { get; }

		long LastModified { get; }
	}
}
