using System;
using System.Collections.Generic;

namespace MvcContrib.IncludeHandling
{
	public interface IIncludeCombiner
	{
		string RenderIncludes(IEnumerable<string> sources, IncludeType type, bool isInDebugMode);
		string RegisterCombination(IEnumerable<string> sources, IncludeType type, DateTime now);
		Include RegisterInclude(string source, IncludeType type);
		IncludeCombination GetCombination(string key);
		IEnumerable<Include> GetAllIncludes();
		IDictionary<string, IncludeCombination> GetAllCombinations();
		void UpdateCombination(IncludeCombination combination);
		void Clear();
	}
}