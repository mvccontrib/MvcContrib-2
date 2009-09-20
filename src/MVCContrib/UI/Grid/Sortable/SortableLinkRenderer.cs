using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace MvcContrib.UI.Grid
{
    /// <summary>
    /// Renders links for sortable column headers in a sorted grid based on current HttpRequest params and column state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortableLinkRenderer<T> where T : class
    {
        HttpRequestBase _request;
        GridColumn<T> _column;

        public SortableLinkRenderer(GridColumn<T> column, RenderingContext context) 
        {
            _column = column;
            _request = context.ViewContext.RequestContext.HttpContext.Request;
        }

        /// <summary>
        /// The link to use in a sortable column that will provide data to help sort the underlying grid's data source.
        /// </summary>
        /// <returns>an html hyperlink tag with the url and name set.</returns>
        public string SortLink() 
        {
            string qs = SortQueryString();
            string baseUrl = _request.Path;
            return String.Format("<a href=\"{0}{1}\">{2}</a>"
                , baseUrl
                , qs
                , _column.DisplayName);
        }

        private string SortQueryString()
        {
            string additionalParams = AdditionalParametersQuery(_request);
            string qsFormat = "?{0}={1}&{2}={3}{4}";
            return String.Format(qsFormat
                 , _column.SortOptions.SortByQueryParameterName
                 , _column.Name
                 , _column.SortOptions.SortOrderQueryParameterName
                 , _column.SortOptions.SortOrder
                 , additionalParams);
        }

        private string AdditionalParametersQuery(HttpRequestBase request)
        {
            string additionalParams = String.Empty;
            foreach (string key in request.QueryString.Keys)
            {
                if (key.ToLower().Equals(_column.SortOptions.SortOrderQueryParameterName, StringComparison.InvariantCultureIgnoreCase) == false &&
                    key.ToLower().Equals(_column.SortOptions.SortByQueryParameterName, StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    additionalParams += String.Format("&{0}={1}", key, request.QueryString[key]);
                }
            }
            return additionalParams;
        }
    }
}
