using System;
using System.IO;
using System.Reflection;
using MvcContrib.ViewFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("ViewFactories")]
	public class FileViewSourceTester
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FileViewSource_DependsOn_FileInfo()
		{
			new FileViewSource(null);
		}

		[Test]
		public void Can_Open_Existing_File()
		{
			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			var viewSource = new FileViewSource(fileInfo);

			Assert.AreEqual(fileInfo.LastWriteTimeUtc.Ticks, viewSource.LastModified);

			using(Stream viewSourceStream = viewSource.OpenViewStream())
			{
				Assert.IsNotNull(viewSourceStream);
			}
		}
	}
}
