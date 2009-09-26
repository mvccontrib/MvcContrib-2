using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MvcContrib.UI.Grid
{
    /// <summary>
    /// Any IEnumerable&lt;T&gt; based list that holds state for sorting.
    /// </summary>
    /// <typeparam name="T">The type of the obje underlying enumerable's instance.</typeparam>
    public interface ISortableDataSource<T> : IEnumerable<T>
    {
        SortOrder SortOrder {get;set;}
        String SortBy{get;set;}
    }
}
