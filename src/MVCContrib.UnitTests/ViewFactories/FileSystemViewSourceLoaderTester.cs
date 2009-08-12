using System.IO;
using MvcContrib.ViewFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("ViewFactories")]
	public class FileSystemViewSourceLoaderTester
	{
		[Test]
		public void HasView_ReturnsFalse_For_Non_Existing_Views()
		{
			var viewSourceLoader = new FileSystemViewSourceLoader("C:\\");

			Assert.IsFalse(viewSourceLoader.HasView("D:\\MyLovelyView"));
		}

		[Test]
		public void GetViewSource_ReturnsNull_For_Non_Existing_Views()
		{
			var viewSourceLoader = new FileSystemViewSourceLoader();

			if (viewSourceLoader.ViewRootDirectory == null)
			{
				viewSourceLoader.ViewRootDirectory = "C:\\";
			}

			Assert.IsNull(viewSourceLoader.GetViewSource("D:\\MyLovelyView"));
		}

		[Test]
		public void ListViews_Returns_Empty_Array_For_Invalid_Directory()
		{
			var viewSourceLoader = new FileSystemViewSourceLoader();

			if (viewSourceLoader.ViewRootDirectory == null)
			{
				viewSourceLoader.ViewRootDirectory = "C:\\";
			}

			string[] views = viewSourceLoader.ListViews("MyViewDir");

			Assert.IsNotNull(views);
			Assert.AreEqual(0, views.Length);
		}

		[Test]
		public void Can_Add_And_Remove_Listeners()
		{
			var viewSourceLoader = new FileSystemViewSourceLoader("C:\\");

			FileSystemEventHandler handler = delegate { };

			viewSourceLoader.ViewRootDirectoryChanged += handler;
			viewSourceLoader.ViewRootDirectoryChanged -= handler;
			viewSourceLoader.ViewRootDirectoryChanged += handler;
			viewSourceLoader.ViewRootDirectoryChanged -= handler;
		}
	}
}
