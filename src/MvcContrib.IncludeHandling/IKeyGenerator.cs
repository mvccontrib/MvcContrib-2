using System.Collections.Generic;

namespace MvcContrib.IncludeHandling
{
	public interface IKeyGenerator
	{
		string Generate(IEnumerable<string> generateFrom);
	}
}