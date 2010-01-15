using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Builds grid columns
	/// </summary>
	public class ColumnBuilder<T> : ICollection<GridColumn<T>> where T : class 
	{
		private readonly List<GridColumn<T>> _columns = new List<GridColumn<T>>();

		/// <summary>
		/// Specifies a column should be constructed for the specified property.
		/// </summary>
		/// <param name="propertySpecifier">Lambda that specifies the property for which a column should be constructed</param>
		public IGridColumn<T> For(Expression<Func<T, object>> propertySpecifier)
		{
			var memberExpression = GetMemberExpression(propertySpecifier);
			var type = GetTypeFromMemberExpression(memberExpression);
			var inferredName = memberExpression == null ? null : memberExpression.Member.Name;

			var column = new GridColumn<T>(propertySpecifier.Compile(), inferredName, type);
			Add(column);
			return column;
		}

		protected IList<GridColumn<T>> Columns
		{
			get { return _columns; }
		}

		/// <summary>
		/// Specifies that a custom column should be constructed with the specified name.
		/// </summary>
		/// <param name="name"></param>
		public IGridColumn<T> For(string name) 
		{
			var column = new GridColumn<T>(x => string.Empty, name, null);
			Add(column);
			return column.Partial(name);
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return _columns.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static MemberExpression GetMemberExpression(LambdaExpression expression)
		{
			return RemoveUnary(expression.Body) as MemberExpression;
		}

		private static Type GetTypeFromMemberExpression(MemberExpression memberExpression) 
		{
			if (memberExpression == null) return null;

			var dataType = GetTypeFromMemberInfo(memberExpression.Member, (PropertyInfo p) => p.PropertyType);
			if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (MethodInfo m) => m.ReturnType);
			if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (FieldInfo f) => f.FieldType);

			return dataType;
		}

		private static Type GetTypeFromMemberInfo<TMember>(MemberInfo member, Func<TMember, Type> func) where TMember : MemberInfo 
		{
			if (member is TMember) 
			{
				return func((TMember)member);
			}
			return null;
		}

		private static Expression RemoveUnary(Expression body)
		{
			var unary = body as UnaryExpression;
			if(unary != null)
			{
				return unary.Operand;
			}
			return body;
		}

		protected virtual void Add(GridColumn<T> column)
		{
			_columns.Add(column);
		}

		void ICollection<GridColumn<T>>.Add(GridColumn<T> column)
		{
			Add(column);
		}

		void ICollection<GridColumn<T>>.Clear()
		{
			_columns.Clear();
		}

		bool ICollection<GridColumn<T>>.Contains(GridColumn<T> column)
		{
			return _columns.Contains(column);
		}

		void ICollection<GridColumn<T>>.CopyTo(GridColumn<T>[] array, int arrayIndex)
		{
			_columns.CopyTo(array, arrayIndex);
		}

		bool ICollection<GridColumn<T>>.Remove(GridColumn<T> column)
		{
			return _columns.Remove(column);
		}

		int ICollection<GridColumn<T>>.Count
		{
			get { return _columns.Count; }
		}

		bool ICollection<GridColumn<T>>.IsReadOnly
		{
			get { return false; }
		}
	}
}