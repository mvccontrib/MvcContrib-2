using System.Collections.Generic;
using MvcContrib.UI.InputBuilder.InputSpecification;

namespace MvcContrib.UI.InputBuilder
{
	public class DefaultTypeConventionsFactory : List<ITypeViewModelFactory>
	{
		public DefaultTypeConventionsFactory()
		{
			Add(new DefaultTypeViewModelFactoryConvention());
		}
	}
}