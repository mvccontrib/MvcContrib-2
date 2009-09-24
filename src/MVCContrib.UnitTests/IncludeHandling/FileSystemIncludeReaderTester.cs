using System;
using MvcContrib.IncludeHandling;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class FileSystemIncludeReaderTester
	{
		private IIncludeReader _reader;

		[SetUp]
		public void TestSetup()
		{
			_reader = new FileSystemIncludeReader("/", "c:\\");
		}

		[Test]
		public void ToAbsolute_ConvertsPathWhenSourceIsRelative()
		{
			var result = _reader.ToAbsolute("~/foo.css");
			Assert.AreEqual("/foo.css", result);
		}

		[Datapoint]
		public string nothing = null;

		[Datapoint] 
		public string blank = "";

		[Theory]
		[ExpectedException(typeof(ArgumentException))]
		public void WhenSourceMissing_WillThrow(string source)
		{
			_reader.Read(source, IncludeType.Css);
		}
	}

	public class FileSystemIncludeReaderIntegrationTester
	{
		private IIncludeReader _reader;

		[SetUp]
		public void TestSetup()
		{
			_reader = new FileSystemIncludeReader("/", Environment.CurrentDirectory);
		}

		[Test]
		public void WhenFileExists_WillReadIt()
		{
			Include include = _reader.Read("IncludeHandling\\exists.txt", IncludeType.Js);
			Assert.AreEqual("hello world, i exist!", include.Content);
		}

		[Test]
		public void WhenFileNotFound_WillThrow()
		{
			Assert.Throws<InvalidOperationException>(() => _reader.Read("c:\\doesNotExist.txt", IncludeType.Css));
		}
	}
}