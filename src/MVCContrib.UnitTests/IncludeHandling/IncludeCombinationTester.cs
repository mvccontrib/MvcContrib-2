using System;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeCombinationTester
	{
		public IncludeCombinationTester()
		{
			var now = DateTime.UtcNow;
			var sources = new[] { "foo.js" };
			var ic = new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement());
			IdenticalCombinationsDifferentReferences = new EqualsData
				{
					A = ic,
					B = new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement()),
					AreEqual = true
				};
			IdenticalCombinationsSameReferences = new EqualsData
				{
					A = ic,
					B = ic,
					AreEqual = true
				};
			DifferentCombinations = new EqualsData
				{
					A = new IncludeCombination(IncludeType.Js, sources, "alert('foo');", now, new JsTypeElement()),
					B = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#foo{color:red;}", now, new CssTypeElement()),
					AreEqual= false
				};

			NoCompression = new ResponseBodyBytes { Compression = ResponseCompression.None, Content = "alert('foo');", ExpectedBytes = new byte[] { 97, 108, 101, 114, 116, 40, 34, 102, 111, 111, 34, 41, 59 } };
			Gzip = new ResponseBodyBytes
				{
					Compression = ResponseCompression.Gzip,
					Content = "alert('foo');",
					ExpectedBytes = new byte[]
					{
						31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 237, 189, 7, 96, 28, 73, 150, 37, 38, 47, 109, 202, 123, 127, 74, 245, 74, 215, 224, 116, 161, 8, 128, 96, 19, 36, 216, 144, 64, 16, 236, 193, 136, 205, 230, 146, 236, 29, 105, 71, 35, 41, 171, 42, 129, 202, 101, 86, 101, 93, 102, 22, 64, 204, 237, 157, 188, 247, 222, 123, 239, 189, 247, 222, 123, 239, 189, 247, 186, 59, 157, 78, 39, 247, 223, 255, 63, 92, 102, 100, 1, 108, 246, 206, 74, 218, 201, 158, 33, 128, 170, 200, 31, 63, 126, 124, 31, 63, 34, 178, 50, 175, 219, 173, 143, 206, 171, 234, 163, 59, 135, 255, 15, 79, 199, 134, 149, 13, 0, 0, 0
					}
				};
			Deflate = new ResponseBodyBytes
				{
					Compression = ResponseCompression.Deflate, Content = "alert('foo');", ExpectedBytes = new byte[]
					{
						237, 189, 7, 96, 28, 73, 150, 37, 38, 47, 109, 202, 123, 127, 74, 245, 74, 215, 224, 116, 161, 8, 128, 96, 19, 36, 216, 144, 64, 16, 236, 193, 136, 205, 230, 146, 236, 29, 105, 71, 35, 41, 171, 42, 129, 202, 101, 86, 101, 93, 102, 22, 64, 204, 237, 157, 188, 247, 222, 123, 239, 189, 247, 222, 123, 239, 189, 247, 186, 59, 157, 78, 39, 247, 223, 255, 63, 92, 102, 100, 1, 108, 246, 206, 74, 218, 201, 158, 33, 128, 170, 200, 31, 63, 126, 124, 31, 63, 34, 178, 50, 175, 219, 173, 143, 206, 171, 234, 163, 59, 135, 255, 15
					}
				};

		}

		[Datapoint]
		public EqualsData IdenticalCombinationsDifferentReferences;

		[Datapoint]
		public EqualsData IdenticalCombinationsSameReferences;

		[Datapoint] 
		public EqualsData DifferentCombinations;

		[Datapoint] 
		public ResponseBodyBytes NoCompression;
		
		[Datapoint]
		public ResponseBodyBytes Gzip;
		
		[Datapoint]
		public ResponseBodyBytes Deflate;

		public class EqualsData
		{
			public EqualsData(){}

			public IncludeCombination A { get; set; }
			public IncludeCombination B { get; set; }
			public bool AreEqual { get; set; }
		}

		[Theory]
		public void GetResponseBodyBytes_CorrectlyCompressesCombination(ResponseBodyBytes responseBodyBytes)
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, responseBodyBytes.Content, DateTime.UtcNow, new JsTypeElement());
			byte[] result = combination.Bytes[responseBodyBytes.Compression];
			Assert.AreEqual(responseBodyBytes.ExpectedBytes, result);
		}

		[Theory]
		public void Equals_CorrectlyComparesTwoCombinations(EqualsData ed)
		{
			var result = ed.A.Equals(ed.B);
			Assert.AreEqual(ed.AreEqual, result);
		}

		[Theory]
		public void Equals_CorrectlyComparesCombinationToObject(EqualsData ed)
		{
			var result = ed.A.Equals((object)ed.B);
			Assert.AreEqual(ed.AreEqual, result);
		}

		[Theory]
		public void Equals_CorrectlyComparesObjectToCombination(EqualsData ed)
		{
			var result = ((object)ed.A).Equals(ed.B);
			Assert.AreEqual(ed.AreEqual, result);
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

		public class ResponseBodyBytes
		{
			public ResponseCompression Compression { get; set; }

			public string Content { get; set; }

			public byte[] ExpectedBytes { get; set; }
		}
	}
}