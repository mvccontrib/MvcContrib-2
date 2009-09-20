using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MvcContrib.UI.Grid
{
    /// <summary>
    /// Maintains state data for sorted columns.
    /// </summary>
    public class ColumnSortOptions
    {
        /// <summary>
        /// Has column been identified as the default sorted column?
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// What is the current sort direction for this particular column?
        /// </summary>
        public SortOrder SortOrder { get; set; }
        /// <summary>
        /// The parameter name to render for this columns's link based on the renderer rendering the columns's header.
        /// </summary>
        public string SortByQueryParameterName { get; set; }
        /// <summary>
        /// The parameter name to render for this columns's link based on the renderer rendering the columns's header.
        /// </summary>
        public string SortOrderQueryParameterName { get; set; }
    }
}
