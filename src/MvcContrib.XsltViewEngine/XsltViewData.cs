using System.Collections.Generic;
using MvcContrib.XsltViewEngine.Messages;

namespace MvcContrib.XsltViewEngine
{
	public class XsltViewData
	{
		private readonly List<XslDataSource> dataSourceCollection;
		private readonly List<Message> messages;
		private readonly Dictionary<string, string> pageVars;


		public XsltViewData()
		{
			pageVars = new Dictionary<string, string>();
			dataSourceCollection = new List<XslDataSource>();
			messages = new List<Message>();
		}


		public List<Message> Messages
		{
			get { return messages; }
		}

		public List<XslDataSource> DataSources
		{
			get { return dataSourceCollection; }
		}

		public Dictionary<string, string> PageVars
		{
			get { return pageVars; }
		}
	}
}