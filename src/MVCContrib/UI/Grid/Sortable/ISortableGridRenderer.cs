using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
    /// <summary>
    /// A IGridRenderer extension that takes a sortable data source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortableGridRenderer<T> : IGridRenderer<T> where T : class
    {
        void Render(IGridModel<T> gridModel, ISortableDataSource<T> dataSource, TextWriter output, ViewContext viewContext);
    }
}
