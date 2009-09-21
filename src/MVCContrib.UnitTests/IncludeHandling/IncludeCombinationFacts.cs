using System;
using System.Collections.Generic;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeCombinationFacts
	{
		public static IEnumerable<object[]> GetResponseBodyBytes_Data
		{
			get
			{
				yield return new object[] { ResponseCompression.None, "alert('foo');", new byte[] { 97, 108, 101, 114, 116, 40, 34, 102, 111, 111, 34, 41, 59 } };
				yield return new object[]
				{
					ResponseCompression.Gzip, "alert('foo');", new byte[]
					{
						31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 237, 189, 7, 96, 28, 73, 150, 37, 38, 47, 109, 202, 123, 127, 74, 245, 74, 215, 224, 116, 161, 8, 128, 96, 19, 36, 216, 144, 64, 16, 236, 193, 136, 205, 230, 146, 236, 29, 105, 71, 35, 41, 171, 42, 129, 202, 101, 86, 101, 93, 102, 22, 64, 204, 237, 157, 188, 247, 222, 123, 239, 189, 247, 222, 123, 239, 189, 247, 186, 59, 157, 78, 39, 247, 223, 255, 63, 92, 102, 100, 1, 108, 246, 206, 74, 218, 201, 158, 33, 128, 170, 200, 31, 63, 126, 124, 31, 63, 34, 178, 50, 175, 219, 173, 143, 206, 171, 234, 163, 59, 135, 255, 15, 79, 199, 134, 149, 13, 0, 0, 0
					}
				};
				yield return new object[]
				{
					ResponseCompression.Deflate, "alert('foo');", new byte[]
					{
						237, 189, 7, 96, 28, 73, 150, 37, 38, 47, 109, 202, 123, 127, 74, 245, 74, 215, 224, 116, 161, 8, 128, 96, 19, 36, 216, 144, 64, 16, 236, 193, 136, 205, 230, 146, 236, 29, 105, 71, 35, 41, 171, 42, 129, 202, 101, 86, 101, 93, 102, 22, 64, 204, 237, 157, 188, 247, 222, 123, 239, 189, 247, 222, 123, 239, 189, 247, 186, 59, 157, 78, 39, 247, 223, 255, 63, 92, 102, 100, 1, 108, 246, 206, 74, 218, 201, 158, 33, 128, 170, 200, 31, 63, 126, 124, 31, 63, 34, 178, 50, 175, 219, 173, 143, 206, 171, 234, 163, 59, 135, 255, 15
					}
				};
			}
		}

		public static IEnumerable<object[]> Equals_Data
		{
			get
			{
				var now = DateTime.UtcNow;
				var sources = new[] { "foo.js" };
				yield return new object[]
				{
					new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement()),
					new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement()),
					true
				};
				var ic = new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement());
				yield return new object[]
				{
					ic,
					ic,
					true
				};
				yield return new object[]
				{
					new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement()),
					new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#foo{color:red;}", now, new CssTypeElement()),
					false
				};
			}
		}

		[Theory]
		[FreezeClock]
		[PropertyData("GetResponseBodyBytes_Data")]
		public void GetResponseBodyBytes_CorrectlyCompressesCombination(ResponseCompression compression, string content, byte[] expected)
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, content, DateTime.UtcNow, new JsTypeElement());
			byte[] result = combination.Bytes[compression];
			Assert.AreEqual(expected, result);
		}

		[Theory]
		[FreezeClock]
		[PropertyData("Equals_Data")]
		public void Equals_CorrectlyComparesTwoCombinations(IncludeCombination a, IncludeCombination b, bool expected)
		{
			var result = a.Equals(b);
			Assert.AreEqual(expected, result);
		}

		[Theory]
		[FreezeClock]
		[PropertyData("Equals_Data")]
		public void Equals_CorrectlyComparesCombinationToObject(IncludeCombination a, object b, bool expected)
		{
			var result = a.Equals(b);
			Assert.AreEqual(expected, result);
		}

		[Theory]
		[FreezeClock]
		[PropertyData("Equals_Data")]
		public void Equals_CorrectlyComparesObjectToCombination(object a, IncludeCombination b, bool expected)
		{
			var result = a.Equals(b);
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Equals_CorrectlyHandlesNullCombination()
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, "alert('foo');", DateTime.UtcNow, new JsTypeElement());
			var result = combination.Equals(null);
			Assert.IsFalse(result);
		}

		[Test]
		public void Equals_CorrectlyHandlesNullObject()
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, "alert('foo');", DateTime.UtcNow, new JsTypeElement());
			var result = combination.Equals((object) null);
			Assert.IsFalse(result);
		}

		[Test]
		public void Equals_CorrectlyHandlesComparisonToDifferentType()
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, "alert('foo');", DateTime.UtcNow, new JsTypeElement());
			var result = combination.Equals(new string[1]);
			Assert.IsFalse(result);
		}
	}
}