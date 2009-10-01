using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.UI.Grid
{
    /// <summary>
    /// A generic list container to sort the list by any identified property of the list type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ComparableSortList<T> : ISortableDataSource<T>
    {
        IEnumerable<T> _internalList;
        public ComparableSortList(IEnumerable<T> sourceList) : this(sourceList, null) { }

        public ComparableSortList(IEnumerable<T> sourceList,string defaultSortField)
        {
            _internalList = sourceList;
            this.SortBy = defaultSortField;
        }

        public SortOrder SortOrder
        {
            get;
            set;
        }

        public String SortBy
        {
            get;
            set;
        }

        private Func<TSource, IComparable> OrderDelegate<TSource>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource), "source");
            var sourceProp = Expression.Property(sourceParam, this.SortBy);

            var orderExpression = Expression.Lambda<Func<TSource, IComparable>>(
                Expression.Convert(sourceProp, typeof(IComparable))
                , sourceParam);
            return orderExpression.Compile();
        }

        public IEnumerator<T> Sort()
        {
            if (this.SortOrder == SortOrder.Ascending)
                return _internalList.OrderBy(OrderDelegate<T>()).GetEnumerator();
            return _internalList.OrderByDescending(OrderDelegate<T>()).GetEnumerator();
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            if (String.IsNullOrEmpty(this.SortBy))
                return _internalList.GetEnumerator();
            return Sort();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();  
        }

        #endregion

        public class SortRouteValues
        {
            public SortRouteValues(string by, string order)
            {
                sortBy = by;
                sortOrder = order;
            }
            public string sortBy { get; set; }
            public string sortOrder { get; set; }
        }

        public SortRouteValues SortLinkValues(string sortField)
        {
            string order = SortOrder.Ascending.ToString();
            if (this.SortBy.ToLower().Equals(sortField.ToLower()) && this.SortOrder == SortOrder.Ascending)
                order = SortOrder.Descending.ToString();
            return new SortRouteValues(sortField.ToString(), order);
        }
    }
}
