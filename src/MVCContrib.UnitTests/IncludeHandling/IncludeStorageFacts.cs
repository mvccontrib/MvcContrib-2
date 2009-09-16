using System;
using System.Collections.Generic;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using Rhino.Mocks;
using Xunit;
using Xunit.Extensions;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class IncludeStorageFacts
	{
		private readonly IncludeCombination _combination;
		private readonly MockRepository _mocks;
		private readonly IIncludeStorage _storage;
		private readonly IKeyGenerator _stubKeyGen;

		public IncludeStorageFacts()
		{
			_mocks = new MockRepository();
			_stubKeyGen = _mocks.Stub<IKeyGenerator>();
			_storage = new StaticIncludeStorage(_stubKeyGen);
			_combination = new IncludeCombination(IncludeType.Css, new[] { "~/content/css/foo.css" }, "#foo {color:red}", Clock.UtcNow, new CssTypeElement());
			_mocks.ReplayAll();
		}

		[Fact]
		public void StoreInclude_ShouldThrowWhenNull()
		{
			const Include include = null;
			Assert.Throws<ArgumentNullException>(() => _storage.Store(include));
		}

		[Fact]
		public void StoreInclude_DoesNotThrow_WhenIncludeIsValid()
		{
			var include = new Include(IncludeType.Js, "/foo.js", "alert('foo');", Clock.UtcNow);
			Assert.DoesNotThrow(() => _storage.Store(include));
		}

		[Fact]
		public void StoreIncludeTwice_DoesNotThrow()
		{
			var include = new Include(IncludeType.Js, "/foo.js", "alert('foo');", Clock.UtcNow);
			Assert.DoesNotThrow(() => _storage.Store(include));
			Assert.DoesNotThrow(() => _storage.Store(include));
		}

		[Fact]
		public void StoreCombination_ShouldThrowWhenNull()
		{
			const IncludeCombination combination = null;
			Assert.Throws<ArgumentNullException>(() => _storage.Store(combination));
		}

		[Fact]
		public void StoreCombination_DoesNotThrow_WhenCombinationIsValid()
		{
			_stubKeyGen.Expect(kg => kg.Generate(_combination.Sources)).Return("foo");
			string key = null;
			Assert.DoesNotThrow(() => key = _storage.Store(_combination));
			Assert.Equal("foo", key);
		}

		[Fact]
		public void StoreCombinationTwice_DoesNotThrow()
		{
			_stubKeyGen.Expect(kg => kg.Generate(_combination.Sources)).Return("foo").Repeat.Twice();
			Assert.DoesNotThrow(() => _storage.Store(_combination));
			Assert.DoesNotThrow(() => _storage.Store(_combination));
		}

		[Fact]
		public void GetCombination_WhenCombinationExists_DoesNotThrow()
		{
			_stubKeyGen.Expect(kg => kg.Generate(_combination.Sources)).Return("foo");
			var key = _storage.Store(_combination);
			IncludeCombination result = null;
			Assert.DoesNotThrow(() => result = _storage.GetCombination(key));

			Assert.Equal(_combination.Content, result.Content);
		}

		[Fact]
		public void GetCombination_WhenCombinationDoesNotExist_ReturnsNull()
		{
			IncludeCombination result = null;
			Assert.DoesNotThrow(() => result = _storage.GetCombination("flsihjdf"));
			Assert.Null(result);
		}

		[Fact]
		public void GetAllIncludes_WillReturnAllIncludes()
		{
			IEnumerable<Include> includes = null;
			Assert.DoesNotThrow(() => includes = _storage.GetAllIncludes());
			Assert.NotNull(includes);
		}

		[Fact]
		public void GetAllCombinations_WillReturnAllCombinations()
		{
			IDictionary<string, IncludeCombination> combinations = null;
			Assert.DoesNotThrow(() => combinations = _storage.GetAllCombinations());
			Assert.NotNull(combinations);
		}
	}
}