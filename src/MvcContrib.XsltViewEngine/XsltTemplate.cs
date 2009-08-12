using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using MvcContrib.ViewFactories;
using Mvp.Xml.Common.Xsl;

namespace MvcContrib.XsltViewEngine
{
	public class XsltTemplate
	{
		public MvpXslTransform XslTransformer { get; private set; }

		public XsltTemplate(VirtualPathProvider virtualPathProvider, string viewPath)
		{

			if( virtualPathProvider == null )
			{
				throw new ArgumentNullException("virtualPathProvider");
			}

			XslTransformer = new MvpXslTransform();

			var settings = new XmlReaderSettings {ProhibitDtd = false};

			using(var viewSourceStream = virtualPathProvider.GetFile(viewPath).Open())
			{
				using (var xmlReader = XmlReader.Create(viewSourceStream, settings))
				{
					XslTransformer.Load(xmlReader);
				}
			}
		}

	}
}
