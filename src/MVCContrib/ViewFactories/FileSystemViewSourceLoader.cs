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

using System;
using System.ComponentModel;
using System.IO;
using System.Web;

namespace MvcContrib.ViewFactories
{
	public class FileSystemViewSourceLoader : IViewSourceLoader
	{
		private string _viewRootDirectory;
		private FileSystemWatcher _viewRootDirectoryWatcher;
		private readonly object _syncRoot = new object();
		private readonly EventHandlerList _events = new EventHandlerList();

		public FileSystemViewSourceLoader()
			: this(GetDefaultViewRootDirectory())
		{	
		}

		public FileSystemViewSourceLoader(string viewRootDirectory)
		{
			_viewRootDirectory = viewRootDirectory;
		}

		public virtual bool HasView(string viewPath)
		{
			return CreateFileInfo(viewPath).Exists;
		}

		protected virtual FileInfo CreateFileInfo(string viewPath)
		{
			if (Path.IsPathRooted(viewPath))
			{
				viewPath = viewPath.Substring(Path.GetPathRoot(viewPath).Length);
			}

			return new FileInfo(Path.Combine(_viewRootDirectory, viewPath));
		}

		public virtual IViewSource GetViewSource(string viewPath)
		{
			FileInfo fileInfo = CreateFileInfo(viewPath);

			if (fileInfo.Exists)
			{
				return new FileViewSource(fileInfo);
			}

			return null;
		}

		public string ViewRootDirectory
		{
			get { return _viewRootDirectory; }
			set { _viewRootDirectory = value; }
		}

		public string[] ListViews(string directoryName)
		{
			if( ViewRootDirectory == null ) return new string[0];

			var directory = new DirectoryInfo(Path.Combine(ViewRootDirectory, directoryName));

			if (directory.Exists)
			{
				return Array.ConvertAll(directory.GetFiles(), file => Path.Combine(directoryName, file.Name));
			}

			return new string[0];
		}

		private static readonly object ViewRootDirectoryChangedEvent = new object();

		public event FileSystemEventHandler ViewRootDirectoryChanged
		{
			add
			{
				lock(_syncRoot)
				{
					if( _viewRootDirectoryWatcher == null )
					{
						CreateViewRootDirectoryWatcher();
					}
					_events.AddHandler(ViewRootDirectoryChangedEvent, value);
				}
			}
			remove 
			{
				lock (_syncRoot)
				{
					_events.RemoveHandler(ViewRootDirectoryChangedEvent, value);
					Delegate handler = _events[ViewRootDirectoryChangedEvent];
					if(handler == null)
					{
						DisposeViewRootDirectoryWatcher();
					}
				}
			}
		}

		private void CreateViewRootDirectoryWatcher()
		{
			if (Directory.Exists(ViewRootDirectory))
			{
				_viewRootDirectoryWatcher = new FileSystemWatcher(ViewRootDirectory)
				                            	{
				                            		IncludeSubdirectories = true,
				                            		EnableRaisingEvents = true
				                            	};
				_viewRootDirectoryWatcher.Changed += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Created += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Deleted += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Renamed += OnViewRootDirectoryChanged;
			}
		}

		private void DisposeViewRootDirectoryWatcher()
		{
			if (_viewRootDirectoryWatcher != null)
			{
				_viewRootDirectoryWatcher.EnableRaisingEvents = false;
				_viewRootDirectoryWatcher.Changed -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Created -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Deleted -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Renamed -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Dispose();
				_viewRootDirectoryWatcher = null;
			}
		}

		protected virtual void OnViewRootDirectoryChanged(object sender, FileSystemEventArgs e)
		{
			var handler = (FileSystemEventHandler)_events[ViewRootDirectoryChangedEvent];
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected static string GetDefaultViewRootDirectory()
		{
			HttpContext current = HttpContext.Current;
			if( current != null )
			{
				return current.Request.MapPath("~/Views");
			}

			return null;
		}
	}
}
