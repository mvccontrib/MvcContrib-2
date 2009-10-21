using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Helpers;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder.InputSpecification
{
	public class ViewModelFactory<T> where T : class
	{
		private readonly IPropertyViewModelFactory[] _propertyFactories;
		private readonly ITypeViewModelFactory[] _typeFactories;
		private readonly IPropertyViewModelNameConvention _nameConventions;
		private readonly HtmlHelper<T> _htmlHelper;

		public ViewModelFactory(HtmlHelper<T> htmlHelper, IPropertyViewModelFactory[] propertyFactories, IPropertyViewModelNameConvention nameConvention, ITypeViewModelFactory[] typeFactories)
		{
			_htmlHelper = htmlHelper;
			_typeFactories = typeFactories;
			_propertyFactories = propertyFactories;
			_nameConventions = nameConvention;
		}
	
		public PropertyViewModel Create(Expression<Func<T, object>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.FindPropertyFromExpression(expression);
			return Create(propertyInfo);
		}

		public PropertyViewModel Create(PropertyInfo propertyInfo)
		{
			string name = _nameConventions.PropertyName(propertyInfo);
			return Create(propertyInfo,name);
		}

		public virtual object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model)
		{
			if (model != null)
			{
				object value = propertyInfo.GetValue(model, new object[0]);
				if (value != null)
				{
					return value;
				}
			}
			return string.Empty;
		}

		protected virtual PropertyViewModel Create(PropertyInfo propertyInfo, string name)
		{
			foreach(IPropertyViewModelFactory factory in _propertyFactories)
			{
				if(factory.CanHandle(propertyInfo))
				{
					return factory.Create(propertyInfo, _htmlHelper.ViewData.Model, name);
				}
			}
			throw new InvalidOperationException("Could not find an Input Builder convention(IPropertyViewModelFactory) for type:" + propertyInfo.PropertyType + " and Name:" + name);
		}

		public TypeViewModel Create()
		{
			foreach(ITypeViewModelFactory factory in _typeFactories)
			{
				if(factory.CanHandle(typeof(T)))
				{
					return factory.Create(typeof(T));
				}
			}
			throw new InvalidOperationException("Could not find an Input Builder Type Convention(ITypeViewModelFactory) for type:" + typeof(T).Name);
		}
	}
}