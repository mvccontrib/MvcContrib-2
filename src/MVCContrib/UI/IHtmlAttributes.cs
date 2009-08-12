using System.Collections;
using System.Collections.Generic;
namespace MvcContrib.UI
{
	public interface IHtmlAttributes: IDictionary<string, string>, IDictionary
	{
		new int Count { get; }
		new IEnumerator<KeyValuePair<string, string>> GetEnumerator();
		int GetEstLength();
	}
}
