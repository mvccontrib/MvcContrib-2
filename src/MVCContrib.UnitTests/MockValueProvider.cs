using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace MvcContrib.UnitTests
{
	public class MockValueProvider : Dictionary<string, ValueProviderResult>
	{
		public MockValueProvider(params Func<object, string>[] hashLiteral) : base(CreateDictionary(hashLiteral))
		{
		}

		private static IDictionary<string, ValueProviderResult> CreateDictionary(Func<object, string>[] lambdas)
		{
			var hash = new Hash<string>(lambdas);
			return hash.Select(x => new { x.Key, Value = new ValueProviderResult(x.Value, x.Value, CultureInfo.CurrentCulture) }).ToDictionary(x => x.Key, x => x.Value);
		}
	}
}