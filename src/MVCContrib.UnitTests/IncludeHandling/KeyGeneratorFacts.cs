using System;
using MvcContrib.IncludeHandling;
using Xunit;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class KeyGeneratorFacts
	{
		private readonly IKeyGenerator _keygen;

		public KeyGeneratorFacts()
		{
			_keygen = new KeyGenerator();
		}

		[Fact]
		public void Generate_WillThrowWhenInputIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => _keygen.Generate(null));
		}

		[Fact]
		public void Generate_WillGenerateAKeyBasedOnInput()
		{
			string key = null;
			Assert.DoesNotThrow(() => key = _keygen.Generate(new[] { "foobar" }));
			Assert.Equal("iEPX-SQWIR3p67lj_0zigSWTKHg@", key);
		}

		[Fact]
		public void Generate_WillNotGenerateTheSameKeyForDifferentInputs()
		{
			var random = new Random(DateTime.UtcNow.Millisecond);
			var input1 = random.Next().ToString();
			var input2 = random.Next().ToString();

			string key1 = null, key2 = null;
			Assert.DoesNotThrow(() => key1 = _keygen.Generate(new[] { input1 }));
			Assert.DoesNotThrow(() => key2 = _keygen.Generate(new[] { input2 }));

			if (input1 == input2)
			{
				Assert.Equal(key1, key2);
			}
			else
			{
				Assert.NotEqual(key1, key2);
			}
		}
	}
}