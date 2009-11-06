using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace MvcContrib.UI.DataList
{
    /// <summary>
    /// By default, the DataList displays the items in a single column, However, you can specify any number of columns.
    /// </summary>
    /// <example>
    /// <code>
    ///       Html.DataList(_dataSource)
    ///           .NumberOfColumns(3)
    ///           .RepeatHorizontally()
    ///           .CellTemplate(x =&gt; { Writer.Write(x.ToLower()); })
    ///           .CellCondition(x =&gt; x == &quot;test1&quot;)
    ///           .EmptyDateSourceTemplate(() =&gt; { Writer.Write(&quot;There is no data available.&quot;); })
    ///           .NoItemTemplate(() =&gt; { Writer.Write(&quot;No Data.&quot;); });
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class DataList<T>
    {
        private readonly IEnumerable<T> _dataSource;
        private readonly Hash _tableAttributes;
        private Action _emptyDataSourceTemplate;
        private Action _noItemTemplate;
        private Func<T, bool> _cellCondition = x => true;
        private Hash _cellAttribute;
        private const string TABLE = "table";

        protected TextWriter Writer { get; set; }

        /// <summary>
        /// Gets or sets the repeat direction.
        /// </summary>
        /// <value>The repeat direction.</value>
        public RepeatDirection RepeatDirection { get; set; }

        /// <summary>
        /// Gets or sets the the amount of columns to display.
        /// </summary>
        /// <remarks>
        /// If this property is set to 0, the DataList will displays its items in a single row or column, based on the value of the RepeatDirection property.
        /// </remarks>
        /// <value>The repeat columns.</value>
        public int RepeatColumns { get; protected set; }

        /// <summary>
        /// Gets or sets the item renderer. You should use <see cref="CellTemplate"/>
        /// </summary>
        /// <value>The item renderer.</value>
        public virtual Action<T> ItemRenderer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="writer">The writer.</param>
        public DataList(IEnumerable<T> dataSource, TextWriter writer)
            : this(dataSource, writer, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="tableAttributes">The table attributes.</param>
        public DataList(IEnumerable<T> dataSource, TextWriter writer, Hash tableAttributes)
        {
            _dataSource = dataSource;
            RepeatColumns = 0;
            RepeatDirection = RepeatDirection.Vertical;
            _tableAttributes = tableAttributes;
            Writer = writer;
        }

        /// <summary>
        /// The main cell template.
        /// </summary>
        /// <param name="contentTemplate">The template.</param>
        /// <returns></returns>
        public virtual DataList<T> CellTemplate(Action<T> contentTemplate)
        {
            ItemRenderer = contentTemplate;
            return this;
        }

        protected void Write(string text)
        {
            Writer.Write(text);
        }

        /// <summary>
        /// If you provide an empty date source the it will use this template in the first cell.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public virtual DataList<T> EmptyDateSourceTemplate(Action template)
        {
            _emptyDataSourceTemplate = template;
            return this;
        }

        /// <summary>
        /// If you have lets say two items and you repeat 3 times
        /// then one cell is going to be empty so fill it with this template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public virtual DataList<T> NoItemTemplate(Action template)
        {
            _noItemTemplate = template;
            return this;
        }

        /// <summary>
        /// Filters the items that will be rendered (This should normally be done in the database)
        /// </summary>
        /// <param name="func">The condition to check.</param>
        /// <returns></returns>
        public virtual DataList<T> CellCondition(Func<T, bool> func)
        {
            _cellCondition = func;
            return this;
        }

        /// <summary>
        /// Attributes that go on every cell (&lt;td id=&quot;foo&quot; class=&quot;bar&quot;&gt;).
        /// </summary>
        /// <example>CellAttributes(id =&gt; &quot;foo&quot;, @class =&gt; &quot;bar&quot;)</example>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public virtual DataList<T> CellAttributes(params Func<object, object>[] attributes)
        {
            _cellAttribute = new Hash(attributes);
            return this;
        }

        /// <summary>
        /// Checks if a item should be rendered, this checks <see cref="CellCondition" />.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual bool ShouldRenderCell(T item)
        {
            return _cellCondition(item);
        }

        /// <summary>
        /// The amount of Columns to render.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public virtual DataList<T> NumberOfColumns(int amount)
        {
            RepeatColumns = amount;
            return this;
        }

        /// <summary>
        /// Repeats the items vertically.
        /// </summary>
        /// <returns></returns>
        public DataList<T> RepeatVertically()
        {
            RepeatDirection = RepeatDirection.Vertical;
            return this;
        }

        /// <summary>
        /// Repeats the items horizontally.
        /// </summary>
        /// <returns></returns>
        public DataList<T> RepeatHorizontally()
        {
            RepeatDirection = RepeatDirection.Horizontal;
            return this;
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void RenderCell(T item)
        {
            Writer.Write(string.Format("<td{0}>", BuildHtmlAttributes(_cellAttribute)));
            if (ItemRenderer != null)
                ItemRenderer(item);
            Writer.Write("</td>");
        }

        /// <summary>
        /// Renders this DataList.
        /// </summary>
        public virtual void Render()
        {
            Write(string.Format("<{0}{1}>", TABLE, BuildHtmlAttributes(_tableAttributes)));
            BuildTable();
            Write(string.Format("</{0}>", TABLE));
        }

        /// <summary>
        /// Renders to the TextWriter, and returns null. 
        /// This is by design so that it can be used with inline syntax in views.
        /// </summary>
        /// <returns>null</returns>
        public override string ToString()
        {
            Render();
            return null;
        }

        private void BuildTable()
        {
            IList<T> items = _dataSource.Where(x => _cellCondition(x)).ToList();

            if (items.Count < 1)
            {
                Write("<tr><td>");
                if (_emptyDataSourceTemplate != null) _emptyDataSourceTemplate();
                Write("</td></tr>");
                return;
            }

            int tmpRepeatColumns = RepeatColumns < 1 ? 1 : RepeatColumns;


            if (RepeatDirection == RepeatDirection.Horizontal)
                RenderHorizontal(tmpRepeatColumns, items);
            else
                RenderVertical(tmpRepeatColumns, items);
        }

        private void RenderHorizontal(int repeatColumns, IList<T> items)
        {
            int rows = CalculateAmountOfRows(items.Count, repeatColumns);
            int columns = repeatColumns;

            int i = 0;

            for (int row = 0; row < rows; row++)
            {
                Write("<tr>");
                for (int column = 0; column < columns; column++)
                {
                    if (i + 1 <= items.Count)
                        RenderCell(items[i]);
                    else
                        RenderNoItemCell();
                    i++;
                }
                Write("</tr>");
            }
        }

        private void RenderVertical(int repeatColumns, IList<T> items)
        {
            int rows = CalculateAmountOfRows(items.Count, repeatColumns);
            int columns = repeatColumns;

            int i = 0;

            for (int row = 0; row < rows; row++)
            {
                Write("<tr>");
                for (int column = 0; column < columns; column++)
                {
                    if (i + 1 <= items.Count)
                    {
                        if (column == 0)
                            RenderCell(items[row]);
                        else
                            RenderCell(items[((column * rows) + row)]);
                    }
                    else
                        RenderNoItemCell();
                    i++;
                }
                Write("</tr>");
            }
        }

        private void RenderNoItemCell()
        {
			Write(string.Format("<td{0}>", BuildHtmlAttributes(_cellAttribute)));
            if (_noItemTemplate != null)
                _noItemTemplate();
            Write("</td>");
        }

        private int CalculateAmountOfRows(int itemCount, int repeatColumns)
        {
            int columns = itemCount / repeatColumns;
            if ((itemCount % repeatColumns) > 0)
                columns += 1;

            return columns;
        }

		private string BuildHtmlAttributes(IDictionary<string, object> attributes)
		{
			if (attributes == null || attributes.Count == 0) 
			{
				return string.Empty;
			}

			const string attributeFormat = "{0}=\"{1}\"";

			string[] strings = attributes.Select(pair => string.Format(attributeFormat, pair.Key, pair.Value)).ToArray();

			return string.Format(" {0}", string.Join(" ", strings));
		}
    }
}