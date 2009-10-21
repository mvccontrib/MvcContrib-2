using System.Collections.Generic;
using MvcContrib.UI.InputBuilder.Conventions;

namespace MvcContrib.UI.InputBuilder
{
	public class DefaultConventionsFactory : List<IPropertyViewModelFactory>
	{
		public DefaultConventionsFactory()
		{
			Add(new GuidPropertyConvention());
			Add(new EnumPropertyConvention());
			Add(new DateTimePropertyConvention());
			Add(new DefaultProperyConvention());
		}
	}
}