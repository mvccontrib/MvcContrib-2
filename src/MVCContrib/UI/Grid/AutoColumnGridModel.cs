using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.UI.Grid
{
	public class AutoColumnGridModel<T> : GridModel<T> where T : class
	{
		public AutoColumnGridModel()
		{
			var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach(var property in properties)
			{
				var propertyExpression = PropertyToExpression(property);
				Column.For(propertyExpression);
			}
		}

		private Expression<Func<T, object>> PropertyToExpression(PropertyInfo property)
		{
			var parameterExpression = Expression.Parameter(typeof(T), "x");
			Expression propertyExpression = Expression.Property(parameterExpression, property);

			if(property.PropertyType.IsValueType)
			{
				propertyExpression = Expression.Convert(propertyExpression, typeof(object));
			}

			var expression = Expression.Lambda(
				typeof(Func<T, object>),
				propertyExpression,
				parameterExpression
			);

			return (Expression<Func<T, object>>)expression;
		}

	}
}