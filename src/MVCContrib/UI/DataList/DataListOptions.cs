using System.Web.UI.WebControls;

namespace MvcContrib.UI.DataList
{
    public class DataListOptions<T>
    {
        private readonly DataList<T> _dataList;

        public DataListOptions(DataList<T> dataList)
        {
            _dataList = dataList;
        }

        public DataList<T> Rows
        {
            get
            {
                _dataList.RepeatDirection = RepeatDirection.Vertical;
                return _dataList;
            }
        }

        public DataList<T> Columns
        {
            get
            {
                _dataList.RepeatDirection = RepeatDirection.Horizontal;
                return _dataList;
            }
        }
    }
}
