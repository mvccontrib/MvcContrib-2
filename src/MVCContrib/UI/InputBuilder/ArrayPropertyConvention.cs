using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder
{
	public class ArrayPropertyConvention : DefaultProperyConvention
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType.IsArray;
		}

		public override object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model, string parentName,
		                                                        IViewModelFactory factory)
		{
			var values = new List<TypeViewModel>();
			object value = base.ValueFromModelPropertyConvention(propertyInfo, model, parentName, factory);

			int index = 0;
			foreach(object o in (IEnumerable)value)
			{
				//new Expression(ExpressionType.ArrayIndex, propertyInfo.PropertyType);
				TypeViewModel item = factory.Create(o.GetType());
				item.Value = o;
				item.Name = parentName + "[" + index + "]";
				item.PartialName = "Subform";
				item.Properties= GetProperies(o,factory,item.Name+".").ToArray();
				item.Layout = "";
				
				values.Add(item);
				index++;
			}
			return values;
		}

		private IEnumerable<PropertyViewModel> GetProperies(object o, IViewModelFactory factory, string parentName)
		{
			foreach(var info in o.GetType().GetProperties())
			{
				PropertyViewModel properies = factory.Create(info, parentName+info.Name , false, o.GetType(),o);
				properies.Layout = "Row";
				yield return properies;
			}
		}

		public override string Layout(PropertyInfo propertyInfo, bool indexed)
		{
			return "Field";
		}

		public override string PartialNameConvention(PropertyInfo propertyInfo, bool indexed)
		{
			return "Array";
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return base.CreateViewModel<IEnumerable<TypeViewModel>>();
		}
	}
}