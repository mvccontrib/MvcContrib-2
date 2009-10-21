using System;
using MvcContrib.UI.InputBuilder.Attributes;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Helpers;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.InputSpecification
{
	public class DefaultTypeViewModelFactoryConvention : ITypeViewModelFactory {
		public bool CanHandle(Type type)
		{
			return true;
		}

		public TypeViewModel Create(Type type)
		{
			return new TypeViewModel()
			{
				Label = LabelForTypeConvention(type),				
				PartialName = "Form",
				Type = type,
                
			};
		}

		public string LabelForTypeConvention(Type type)
		{
			if (type.AttributeExists<LabelAttribute>())
			{
				return type.GetAttribute<LabelAttribute>().Label;
			}
			return type.Name.ToSeparatedWords();
		}
	}
}