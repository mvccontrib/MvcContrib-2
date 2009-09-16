using System;
using System.Collections.Generic;
using System.Configuration;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using Xunit;
using Xunit.Extensions;
using Yahoo.Yui.Compressor;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class IncludeHandlingSectionHandlerFacts
	{
		[Fact]
		public void DefaultsAreCorrect()
		{
			var section = (IIncludeHandlingSettings) ConfigurationManager.GetSection("defaultsAreCorrect");

			Assert.Equal("~/include/{0}/{1}", section.Css.Path);
			Assert.Equal(TimeSpan.FromDays(365), section.Css.CacheFor);
			var expected = new List<ResponseCompression> { ResponseCompression.Gzip, ResponseCompression.Deflate };
			Assert.Equal(expected[0], section.Css.CompressionOrder[0]);
			Assert.Equal(expected[1], section.Css.CompressionOrder[1]);
			Assert.Equal(int.MaxValue, section.Css.LineBreakAt);
			Assert.Equal(true, section.Css.Minify);
			Assert.Equal(true, section.Css.Compress);

			Assert.Equal("~/include/{0}/{1}", section.Js.Path);
			Assert.Equal(TimeSpan.FromDays(365), section.Js.CacheFor);
			Assert.Equal(expected[0], section.Js.CompressionOrder[0]);
			Assert.Equal(expected[1], section.Js.CompressionOrder[1]);
			Assert.Equal(int.MaxValue, section.Js.LineBreakAt);
			Assert.Equal(true, section.Js.Minify);
			Assert.Equal(true, section.Js.Compress);

			Assert.Equal(CssCompressionType.StockYuiCompressor, section.Css.CompressionType);

			Assert.Equal(false, section.Js.DisableOptimizations);
			Assert.Equal(true, section.Js.Obfuscate);
			Assert.Equal(true, section.Js.PreserveSemiColons);
			Assert.Equal(false, section.Js.Verbose);
		}

		[Fact]
		public void CanChangeAllTheDefaultsEvenThoughIShouldntWriteATestWithABigSurfaceAreaLikeThisNaughtyPete()
		{
			var section = (IIncludeHandlingSettings) ConfigurationManager.GetSection("canChangeDefaults");

			Assert.Equal("~/foo/{0}/{1}", section.Css.Path);
			Assert.Equal(new TimeSpan(10, 10, 10), section.Css.CacheFor);
			var cssRCs = new List<ResponseCompression> { ResponseCompression.Gzip };
			Assert.Equal(cssRCs[0], section.Css.CompressionOrder[0]);
			Assert.Equal(180, section.Css.LineBreakAt);
			Assert.Equal(false, section.Css.Minify);

			Assert.Equal("~/bar/{0}/{1}", section.Js.Path);
			Assert.Equal(new TimeSpan(11, 11, 11, 11, 100), section.Js.CacheFor);
			var jsRCs = new[] { ResponseCompression.Deflate, ResponseCompression.Gzip };
			Assert.Equal(jsRCs[0], section.Js.CompressionOrder[0]);
			Assert.Equal(jsRCs[1], section.Js.CompressionOrder[1]);
			Assert.Equal(int.MaxValue, section.Js.LineBreakAt);
			Assert.Equal(false, section.Js.Minify);

			Assert.Equal(CssCompressionType.Hybrid, section.Css.CompressionType);

			Assert.Equal(true, section.Js.DisableOptimizations);
			Assert.Equal(false, section.Js.Obfuscate);
			Assert.Equal(false, section.Js.PreserveSemiColons);
			Assert.Equal(true, section.Js.Verbose);
		}

		[Theory]
		[InlineData("pathValidation1")]
		[InlineData("pathValidation2")]
		public void WhenPathMissingAFormatPlaceHolder_WillThrow(string sectionName)
		{
			var section = (IIncludeHandlingSettings) ConfigurationManager.GetSection(sectionName);
			string path = null;
			Assert.Throws<ConfigurationErrorsException>(() => path = section.Css.Path);
			Assert.Null(path);
		}
	}
}