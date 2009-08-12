using System.Xml;

namespace MvcContrib.XsltViewEngine
{
	public class XslDataSource
	{
		public XslDataSource(IXmlConvertible dataSource)
		{
			DataSource = dataSource;
		}

		public XslDataSource(string rootName, IXmlConvertible dataSource)
		{
			RootName = rootName;
			DataSource = dataSource;
		}

		public string RootName
		{
			get;
			set;
		}

		public IXmlConvertible DataSource
		{
			get;
			set;
		}

		public XmlNode XmlFragment
		{
			get { return string.IsNullOrEmpty(RootName) ? DataSource.ToXml() : DataSource.ToXml(RootName); }
		}
	}
}