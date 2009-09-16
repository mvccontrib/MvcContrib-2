using System;
using MvcContrib.IncludeHandling;
using Xunit;
using Xunit.Extensions;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class FileSystemIncludeReaderFacts
	{
		private readonly IIncludeReader _reader;

		public FileSystemIncludeReaderFacts()
		{
			_reader = new FileSystemIncludeReader("/", "c:\\");
		}

		[Fact]
		public void ToAbsolute_ConvertsPathWhenSourceIsRelative()
		{
			string result = null;
			Assert.DoesNotThrow(() => result = _reader.ToAbsolute("~/foo.css"));
			Assert.Equal("/foo.css", result);
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

		[Fact]
		public void WhenFileExists_WillReadIt()
		{
			Include include = null;
			Assert.DoesNotThrow(() => include = _reader.Read("IncludeHandling\\exists.txt", IncludeType.Js));
			Assert.Equal("hello world, i exist!", include.Content);
		}

		[Fact]
		public void WhenFileNotFound_WillThrow()
		{
			Assert.Throws<InvalidOperationException>(() => _reader.Read("c:\\doesNotExist.txt", IncludeType.Css));
		}
	}
}