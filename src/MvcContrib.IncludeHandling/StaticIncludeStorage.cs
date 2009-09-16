using System;
using System.Collections.Generic;
using MvcContrib;

namespace MvcContrib.IncludeHandling
{
	public class StaticIncludeStorage : IIncludeStorage
	{
		private static readonly IDictionary<string, IncludeCombination> _combinations;
		private static readonly IDictionary<string, Include> _includes;

		private readonly IKeyGenerator _keyGen;

		static StaticIncludeStorage()
		{
			_includes = new Dictionary<string, Include>();
			_combinations = new Dictionary<string, IncludeCombination>();
		}

		public StaticIncludeStorage(IKeyGenerator keyGen)
		{
			_keyGen = keyGen;
		}

		#region IIncludeStorage Members

		public void Store(Include include)
		{
			if (include == null)
			{
				throw new ArgumentNullException("include");
			}
			if (!_includes.ContainsKey(include.Source))
			{
				_includes.Add(include.Source, include);
			}
			else
			{
				_includes[include.Source] = include;
			}
		}

		public string Store(IncludeCombination combination)
		{
			if (combination == null)
			{
				throw new ArgumentNullException("combination");
			}
			var key = _keyGen.Generate(combination.Sources);

			if (!_combinations.ContainsKey(key))
			{
				_combinations.Add(key, combination);
			}
			else
			{
				_combinations[key] = combination;
			}
			return key;
		}

		public IncludeCombination GetCombination(string key)
		{
			try
			{
				return _combinations[key];
			}
			catch (KeyNotFoundException)
			{
				return null;
			}
		}

		public IEnumerable<Include> GetAllIncludes()
		{
			return _includes.Values;
		}

		public IDictionary<string, IncludeCombination> GetAllCombinations()
		{
			return _combinations;
		}

		public void Clear()
		{
			_includes.Clear();
			_combinations.Clear();
		}

		#endregion
	}
}