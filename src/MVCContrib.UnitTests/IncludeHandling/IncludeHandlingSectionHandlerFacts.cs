using System;
using System.Collections.Generic;
using System.Configuration;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;
using Yahoo.Yui.Compressor;


namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeHandlingSectionHandlerFacts
	{
		[Test]
		public void DefaultsAreCorrect()
		{
			var section = (IIncludeHandlingSettings) ConfigurationManager.GetSection("defaultsAreCorrect");

			Assert.AreEqual("~/include/{0}/{1}", section.Css.Path);
			Assert.AreEqual(TimeSpan.FromDays(365), section.Css.CacheFor);
			var expected = new List<ResponseCompression> { ResponseCompression.Gzip, ResponseCompression.Deflate };
			Assert.AreEqual(expected[0], section.Css.CompressionOrder[0]);
			Assert.AreEqual(expected[1], section.Css.CompressionOrder[1]);
			Assert.AreEqual(int.MaxValue, section.Css.LineBreakAt);
			Assert.AreEqual(true, section.Css.Minify);
			Assert.AreEqual(true, section.Css.Compress);

			Assert.AreEqual("~/include/{0}/{1}", section.Js.Path);
			Assert.AreEqual(TimeSpan.FromDays(365), section.Js.CacheFor);
			Assert.AreEqual(expected[0], section.Js.CompressionOrder[0]);
			Assert.AreEqual(expected[1], section.Js.CompressionOrder[1]);
			Assert.AreEqual(int.MaxValue, section.Js.LineBreakAt);
			Assert.AreEqual(true, section.Js.Minify);
			Assert.AreEqual(true, section.Js.Compress);

			Assert.AreEqual(CssCompressionType.StockYuiCompressor, section.Css.CompressionType);

			Assert.AreEqual(false, section.Js.DisableOptimizations);
			Assert.AreEqual(true, section.Js.Obfuscate);
			Assert.AreEqual(true, section.Js.PreserveSemiColons);
			Assert.AreEqual(false, section.Js.Verbose);
		}

		[Test]
		public void CanChangeAllTheDefaultsEvenThoughIShouldntWriteATestWithABigSurfaceAreaLikeThisNaughtyPete()
		{
			var section = (IIncludeHandlingSettings) ConfigurationManager.GetSection("canChangeDefaults");

			Assert.AreEqual("~/foo/{0}/{1}", section.Css.Path);
			Assert.AreEqual(new TimeSpan(10, 10, 10), section.Css.CacheFor);
			var cssRCs = new List<ResponseCompression> { ResponseCompression.Gzip };
			Assert.AreEqual(cssRCs[0], section.Css.CompressionOrder[0]);
			Assert.AreEqual(180, section.Css.LineBreakAt);
			Assert.AreEqual(false, section.Css.Minify);

			Assert.AreEqual("~/bar/{0}/{1}", section.Js.Path);
			Assert.AreEqual(new TimeSpan(11, 11, 11, 11, 100), section.Js.CacheFor);
			var jsRCs = new[] { ResponseCompression.Deflate, ResponseCompression.Gzip };
			Assert.AreEqual(jsRCs[0], section.Js.CompressionOrder[0]);
			Assert.AreEqual(jsRCs[1], section.Js.CompressionOrder[1]);
			Assert.AreEqual(int.MaxValue, section.Js.LineBreakAt);
			Assert.AreEqual(false, section.Js.Minify);

			Assert.AreEqual(CssCompressionType.Hybrid, section.Css.CompressionType);

			Assert.AreEqual(true, section.Js.DisableOptimizations);
			Assert.AreEqual(false, section.Js.Obfuscate);
			Assert.AreEqual(false, section.Js.PreserveSemiColons);
			Assert.AreEqual(true, section.Js.Verbose);
		}

		[Datapoint] public string pathValidation1 = "pathValidation1";
		[Datapoint] public string pathValidation2 = "pathValidation2";

		[Theory]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void WhenPathMissingAFormatPlaceHolder_WillThrow(string sectionName)
		{
			var section = (IIncludeHandlingSettings) ConfigurationManager.GetSection(sectionName);
			string path = null;
			Assert.Throws<ConfigurationErrorsException>(() => path = section.Css.Path);
		}
	}
}