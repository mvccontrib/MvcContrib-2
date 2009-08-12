using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml.Xsl;
using Mvp.Xml.Common.Xsl;
using System.Web;

namespace MvcContrib.XsltViewEngine
{
	public class XsltView : IViewDataContainer,IView
	{
		private readonly string ajaxDeclaration;
		private readonly XsltViewData viewData;
		private readonly XsltTemplate viewTemplate;
		private readonly MvpXslTransform xslTransformer;
		private readonly XmlResponseBuilder construct;
		private ViewContext viewContext;

		public XsltView(XsltTemplate viewTemplate, XsltViewData viewData, string ajaxDeclaration, HttpContextBase httpContext)
		{
			this.viewTemplate = viewTemplate;
			this.viewData = viewData;
			this.ajaxDeclaration = ajaxDeclaration;

			construct = new XmlResponseBuilder(httpContext);

			InitializeConstruct();

			xslTransformer = viewTemplate.XslTransformer;
		}

		public ViewDataDictionary ViewData
		{
			get { return viewContext.ViewData; }
			set { throw new NotSupportedException(); }
		}

		private void InitializeConstruct()
		{
			construct.InitMessageStructure();

			viewData.DataSources.ForEach(dataSource => construct.AppendDataSourceToResponse(dataSource.XmlFragment));

			viewData.Messages.ForEach(message =>{
				if (string.IsNullOrEmpty(message.ControlID))
					construct.AddMessage(message.Content, message.MessageType.ToString().ToUpperInvariant());
				else
					construct.AddMessage(message.Content, message.MessageType.ToString().ToUpperInvariant(), message.ControlID);
			});
		}


        public void Render(ViewContext viewContext, TextWriter writer)
        {
			this.viewContext = viewContext;

			string url = viewContext.HttpContext.Request.Url.ToString();
			construct.AppendPage("", url, viewData.PageVars);

			var args = new XsltArgumentList();
			args.AddExtensionObject("urn:HtmlHelper", new HtmlHelper(viewContext, this));

			args.AddParam("AjaxProScriptReferences", "", ajaxDeclaration);

			xslTransformer.Transform(new XmlInput(construct.Message.CreateNavigator()), args, new XmlOutput(writer));
        }

    }
}
