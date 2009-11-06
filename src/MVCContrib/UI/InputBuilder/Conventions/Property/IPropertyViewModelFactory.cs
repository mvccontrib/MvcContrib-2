using System;
using System.Reflection;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelFactory
	{
		bool CanHandle(PropertyInfo propertyInfo);
		PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type);
	}

	public interface  IRequireViewModelFactory
	{
		void Set(IViewModelFactory factory);
	}
}