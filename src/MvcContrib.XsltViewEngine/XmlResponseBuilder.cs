using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace MvcContrib.XsltViewEngine
{
	public class XmlResponseBuilder
	{
		#region Private fields

		// current path
		private readonly string appPath;
		private readonly HttpContextBase httpContext;

		// xml docs
		private readonly XmlDocument xmlMessage;

		#endregion

		#region Constructor

		/// <summary>
		/// Context
		/// </summary>
		/// <param name="httpContext">The HTTP context.</param>
		public XmlResponseBuilder(HttpContextBase httpContext)
		{
			this.httpContext = httpContext;
			appPath = httpContext.Request.PhysicalApplicationPath;

			xmlMessage = new XmlDocument();
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public XmlDocument Message
		{
			get { return xmlMessage; }
		}

		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <value>The response.</value>
		public XmlNode Response
		{
			get { return xmlMessage.SelectSingleNode("Message/Response"); }
		}

		/// <summary>
		/// Gets the app path.
		/// </summary>
		/// <value>The app path.</value>
		public string AppPath
		{
			get { return appPath; }
		}

		#endregion

		#region Create new node

		/// <summary>
		/// Creates the new node.
		/// </summary>
		/// <param name="sNode">The s node.</param>
		/// <param name="sText">The s text.</param>
		/// <param name="sAttributes">The s attributes.</param>
		/// <returns></returns>
		public XmlElement CreateNewNode(string sNode, string sText, params string[] sAttributes)
		{
			XmlElement xmlelem;

			try
			{
				xmlelem = xmlMessage.CreateElement("", sNode.Replace("/", ""), "");
				XmlText xmltext = xmlMessage.CreateTextNode(sText);
				xmlelem.AppendChild(xmltext);

				for(int i = 0; i < sAttributes.Length; i++)
				{
					if(i % 2 == 0)
					{
						XmlAttribute xmlatt = xmlMessage.CreateAttribute("", sAttributes[i], "");
						xmlelem.SetAttributeNode(xmlatt);

						if((i + 1) < sAttributes.Length)
						{
							xmlatt.Value = sAttributes[i + 1];
						}
					}
				}
			}
			finally
			{
				//Nothing to be done here
			}
			return xmlelem;
		}

		#endregion

		#region Init message structure method

		/// <summary>
		/// Initializes the message structure.
		/// </summary>
		public void InitMessageStructure()
		{
			xmlMessage.AppendChild(xmlMessage.CreateNode(XmlNodeType.XmlDeclaration, "", ""));
			xmlMessage.AppendChild(xmlMessage.CreateNewNode("Message", ""));
			AppendHeader();
			AppendRequest();
			AppendResponse();
		}

		#endregion

		#region Append datasource to response methods

		/// <summary>
		/// Appends the data source to response.
		/// </summary>
		/// <param name="xml">The XML.</param>
		public void AppendDataSourceToResponse(string xml)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);

			AppendDataSourceToResponse(xmlDoc.DocumentElement);
		}

		/// <summary>
		/// Appends the data source to response.
		/// </summary>
		/// <param name="xml">The XML.</param>
		public void AppendDataSourceToResponse(XmlReader xml)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(xml);

			AppendDataSourceToResponse(xmlDoc.DocumentElement);
		}

		/// <summary>
		/// Appends the data source to response.
		/// </summary>
		/// <param name="xml">The XML.</param>
		public void AppendDataSourceToResponse(XmlNode xml)
		{
			Response.AppendChild(Message.ImportNode(xml, true));
		}

		#endregion

		#region Add message methods

		/// <summary>
		/// Adds a message to the response xml document.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="type">The type.</param>
		public void AddMessage(string message, string type)
		{
			AddMessage(message, type, "");
		}


		/// <summary>
		/// Adds a message to the response xml document.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="type">The type.</param>
		/// <param name="controlID">The control ID.</param>
		public void AddMessage(string message, string type, string controlID)
		{
			if(Response.SelectSingleNode("Messages") == null)
				Response.AppendChild(CreateNewNode("Messages", ""));

			if(controlID == null || controlID == "")
				Response.SelectSingleNode("Messages").AppendChild(CreateNewNode("Message", message, "Type", type));
			else
				Response.SelectSingleNode("Messages").AppendChild(
					CreateNewNode("Message", message, "Type", type, "ControlID", controlID));
		}

		#endregion

		#region Append page method

		public void AppendPage(string pageName, string pageUri, Dictionary<string, string> pageVars)
		{
			if(Response.SelectSingleNode("Page") == null)
			{
				Response.AppendChild(CreateNewNode("Page", "", "Uri", pageUri));

				XmlNode pageVarsNode = Response.SelectSingleNode("Page").AppendChild(CreateNewNode("PageVars", ""));

				foreach(var pair in pageVars)
				{
					pageVarsNode.AppendChild(CreateNewNode(pair.Key, pair.Value));
				}
			}
		}

		#endregion

		#region Private helper methods

		private void AppendHeader()
		{
			XmlElement xmlHeader = CreateNewNode("Header", "", "SessionID", "");
			xmlMessage.SelectSingleNode("Message").AppendChild(xmlHeader);

			xmlHeader.AppendChild(CreateNewNode("IPAddress", httpContext.Request.UserHostName));
			xmlHeader.AppendChild(CreateNewNode("User", httpContext.User.Identity.Name));
			xmlHeader.AppendChild(CreateNewNode("IsSecure", httpContext.Request.IsSecureConnection.ToString()));
			xmlHeader.AppendChild(
				CreateNewNode("Browser",
				              (httpContext.Request.Browser.Browser + ' ' + httpContext.Request.Browser.Version).Trim()));

			if(httpContext.Request.Browser.EcmaScriptVersion.Major >= 1)
				xmlHeader.AppendChild(CreateNewNode("JavaScript", "True"));
			else
				xmlHeader.AppendChild(CreateNewNode("JavaScript", "False"));
		}

		private void AppendRequest()
		{
			XmlElement xmlRequest =
				CreateNewNode("Request", "", "Page", GetURLPage(AppPath), "Method", httpContext.Request.HttpMethod);
			xmlMessage.SelectSingleNode("Message").AppendChild(xmlRequest);

			if(httpContext.Request.RequestType == "POST")
			{
				foreach(string var in httpContext.Request.Form)
				{
					xmlRequest.AppendChild(
						CreateNewNode("Params", "", "Key", var, "Value", httpContext.Request.Form[var]));
				}
				RequestFileUploads();
			}

			if(httpContext.Request.RequestType == "GET")
			{
				foreach(string var in httpContext.Request.QueryString)
				{
					xmlRequest.AppendChild(
						CreateNewNode("Params", "", "Key", var, "Value", httpContext.Request.QueryString[var]));
				}
			}
		}

		private void RequestFileUploads()
		{
			if(httpContext.Request.Files == null)
				return;

			if(httpContext.Request.Files.Count > 0)
				xmlMessage.SelectSingleNode("Message/Request").AppendChild(CreateNewNode("FileUploads", ""));


			for(int i = 0; i < httpContext.Request.Files.Count; i++)
			{
				HttpPostedFileBase file = httpContext.Request.Files[i];
				string fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);

				if((file.FileName != "") & (file.ContentLength != 0))
				{
					try
					{
						xmlMessage
							.SelectSingleNode("Message/Request/FileUploads")
							.AppendChild(
							CreateNewNode("File", "", "FormElementID", httpContext.Request.Files.AllKeys[i],
							              "FileName", fileName, "ContentType",
							              file.ContentType, "Bytes", file.ContentLength.ToString()));
					}
					catch(Exception ex)
					{
						string temp = ex.Message;
					}
				}
			}
		}

		private static string GetURLPage(string sURL)
		{
			return sURL.Substring(sURL.LastIndexOf("/") + 1).Replace(".aspx", "");
		}

		private void AppendResponse()
		{
			// create response
			XmlElement xmlResponse = CreateNewNode("Response", "");
			xmlMessage.SelectSingleNode("Message").AppendChild(xmlResponse);

			// create service points node
			xmlResponse.AppendChild(CreateNewNode("ServicePoints", ""));
		}

		#endregion
	}
}
