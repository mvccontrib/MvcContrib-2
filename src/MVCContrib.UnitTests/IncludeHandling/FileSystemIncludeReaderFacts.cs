using System;
using MvcContrib.IncludeHandling;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class FileSystemIncludeReaderFacts
	{
		private readonly IIncludeReader _reader;

		public FileSystemIncludeReaderFacts()
		{
			_reader = new FileSystemIncludeReader("/", "c:\\");
		}

		[Test]
		public void ToAbsolute_ConvertsPathWhenSourceIsRelative()
		{
			var result = _reader.ToAbsolute("~/foo.css");
			Assert.AreEqual("/foo.css", result);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void WhenSourceMissing_WillThrow(string source)
		{
			Assert.Throws<ArgumentException>(() => _reader.Read(source, IncludeType.Css));
		}
	}

	public class FileSystemIncludeReaderIntegrationFacts
	{
		private readonly IIncludeReader _reader;

		public FileSystemIncludeReaderIntegrationFacts()
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