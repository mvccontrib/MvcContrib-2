using System.Xml;

namespace MvcContrib.XsltViewEngine
{
	public static class XsltViewEngineExtensionMethods
	{

		public static XmlElement CreateNewNode(this XmlDocument xmldoc, string sNode, string sText, params string[] sAttributes)
	{
			var xmlelem = xmldoc.CreateElement("", sNode, "");
		var xmltext = xmldoc.CreateTextNode(sText);
		xmlelem.AppendChild(xmltext);

		for (int i = 0; i < sAttributes.Length; i++)
		{
			if (i % 2 == 0)
			{
				var xmlAttribute = xmldoc.CreateAttribute("", sAttributes[i], "");
				xmlelem.SetAttributeNode(xmlAttribute);

				if ((i + 1) < sAttributes.Length)
				{
				    xmlAttribute.Value = sAttributes[i + 1];
				}

			}

		}
		return xmlelem;
	}
	}
}
