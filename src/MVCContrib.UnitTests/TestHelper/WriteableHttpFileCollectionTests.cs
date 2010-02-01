using System.Web;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using Assert=NUnit.Framework.Assert;

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class WriteableHttpFileCollectionTests
	{
		private MockRepository mocks;
		private HttpPostedFileBase file;
		private IWriteableHttpFileCollection readWrite;
		private HttpFileCollectionBase readOnly;

		[SetUp]
		public void SetUp()
		{
			mocks = new MockRepository();
			file = mocks.DynamicMock<HttpPostedFileBase>();

			var collection = new WriteableHttpFileCollection();
			readWrite = collection;
			readOnly = collection;
		}

		[Test]
		public void CanSetAndGetWhenUsingExplicitInterface()
		{
			readWrite["Foo"] = file;

			Assert.AreSame(file, readWrite["Foo"]);
		}

		[Test]
		public void CanReadFilesWhenCastToHttpFileCollectionBase()
		{
			readWrite["Fubar"] = file;

			Assert.AreSame(file, readOnly["Fubar"]);
		}

		[Test]
		public void AllKeysReturnsFileNames()
		{
			readWrite["Foo"] = file;

			CollectionAssert.AreEquivalent(new[] {"Foo"}, readOnly.AllKeys);
		}

		[Test]
		public void CanCountFilesWhenThereAreNoFiles()
		{
			Assert.AreEqual(readOnly.Count, 0);
		}

		[Test]
		public void CanCountFilesWhenThereAreSomeFiles()
		{
			readWrite["Foo"] = file;
			readWrite["Bar"] = file;

			Assert.AreEqual(readOnly.Count, 2);
		}

		[Test]
		public void CanGetFilesSpecifiedByIndex()
		{
			readWrite["Foo"] = null;
			readWrite["Bar"] = file;

			Assert.AreSame(readOnly[1], file);
		}
	}
}
