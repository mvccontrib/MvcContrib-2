using System;
using MvcContrib.IncludeHandling;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class KeyGeneratorTester
	{
		private IKeyGenerator _keygen;

		[SetUp]
		public void TestSetup()
		{
			_keygen = new KeyGenerator();
		}

		[Test]
		public void Generate_WillThrowWhenInputIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => _keygen.Generate(null));
		}

		[Test]
		public void Generate_WillGenerateAKeyBasedOnInput()
		{
			string key = _keygen.Generate(new[] { "foobar" });
			Assert.AreEqual("iEPX-SQWIR3p67lj_0zigSWTKHg@", key);
		}

		[Test]
		public void Generate_WillNotGenerateTheSameKeyForDifferentInputs()
		{
			var random = new Random(DateTime.UtcNow.Millisecond);
			var input1 = random.Next().ToString();
			var input2 = random.Next().ToString();

			string key1 = _keygen.Generate(new[] {input1});
			var key2 = _keygen.Generate(new[] {input2});

			if (input1 == input2)
			{
				Assert.AreEqual(key1, key2);
			}
			else
			{
				Assert.AreNotEqual(key1, key2);
			}
		}
	}
}