using System;
using System.Collections.Generic;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeStorageTester
	{
		private IncludeCombination _combination;
		private MockRepository _mocks;
		private IIncludeStorage _storage;
		private IKeyGenerator _stubKeyGen;

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_stubKeyGen = _mocks.Stub<IKeyGenerator>();
			_storage = new StaticIncludeStorage(_stubKeyGen);
			_combination = new IncludeCombination(IncludeType.Css, new[] { "~/content/css/foo.css" }, "#foo {color:red}", DateTime.UtcNow, new CssTypeElement());
			_mocks.ReplayAll();
		}

		[Test]
		public void StoreInclude_ShouldThrowWhenNull()
		{
			const Include include = null;
			Assert.Throws<ArgumentNullException>(() => _storage.Store(include));
		}

		[Test]
		public void StoreInclude_DoesNotThrow_WhenIncludeIsValid()
		{
			var include = new Include(IncludeType.Js, "/foo.js", "alert('foo');", DateTime.UtcNow);
			_storage.Store(include);
			Assert.IsTrue(true);
		}

		[Test]
		public void StoreIncludeTwice_DoesNotThrow()
		{
			var include = new Include(IncludeType.Js, "/foo.js", "alert('foo');", DateTime.UtcNow);
			_storage.Store(include);
			_storage.Store(include);
			Assert.IsTrue(true);
		}

		[Test]
		public void StoreCombination_ShouldThrowWhenNull()
		{
			const IncludeCombination combination = null;
			Assert.Throws<ArgumentNullException>(() => _storage.Store(combination));
		}

		[Test]
		public void StoreCombination_DoesNotThrow_WhenCombinationIsValid()
		{
			_stubKeyGen.Expect(kg => kg.Generate(_combination.Sources)).Return("foo");
			string key = _storage.Store(_combination);
			Assert.AreEqual("foo", key);
		}

		[Test]
		public void StoreCombinationTwice_DoesNotThrow()
		{
			_stubKeyGen.Expect(kg => kg.Generate(_combination.Sources)).Return("foo").Repeat.Twice();
			_storage.Store(_combination);
			_storage.Store(_combination);
			Assert.IsTrue(true);
		}

		[Test]
		public void GetCombination_WhenCombinationExists_DoesNotThrow()
		{
			_stubKeyGen.Expect(kg => kg.Generate(_combination.Sources)).Return("foo");
			var key = _storage.Store(_combination);
			IncludeCombination result = _storage.GetCombination(key);

			Assert.AreEqual(_combination.Content, result.Content);
		}

		[Test]
		public void GetCombination_WhenCombinationDoesNotExist_ReturnsNull()
		{
			IncludeCombination result = _storage.GetCombination("flsihjdf");
			Assert.IsNull(result);
		}

		[Test]
		public void GetAllIncludes_WillReturnAllIncludes()
		{
			IEnumerable<Include> includes = _storage.GetAllIncludes();
			Assert.IsNotNull(includes);
		}

		[Test]
		public void GetAllCombinations_WillReturnAllCombinations()
		{
			IDictionary<string, IncludeCombination> combinations = _storage.GetAllCombinations();
			Assert.IsNotNull(combinations);
		}
	}
}