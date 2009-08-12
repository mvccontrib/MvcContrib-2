using System;
using System.IO;

namespace MvcContrib.ViewFactories
{
	public class FileViewSource : IViewSource
	{
		private readonly FileInfo _fileInfo;

		public FileViewSource(FileInfo fileInfo)
		{
			if (fileInfo == null) throw new ArgumentNullException("fileInfo");

			_fileInfo = fileInfo;
		}

		public Stream OpenViewStream()
		{
			return _fileInfo.OpenRead();
		}

		public string FullName
		{
			get { return _fileInfo.FullName; }
		}

		public long LastModified
		{
			get
			{
				_fileInfo.Refresh();
				return _fileInfo.LastWriteTimeUtc.Ticks;
			}
		}
	}
}