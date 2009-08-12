using System.Xml;

namespace MvcContrib.XsltViewEngine
{
	public interface IXmlConvertible
	{
		XmlNode ToXml();
		XmlNode ToXml(string root);
	}
}