namespace MvcContrib.XsltViewEngine.Messages
{
	public class InfoHtmlMessage : Message
	{
		public InfoHtmlMessage(string message) : base(MessageType.InfoHtml, message)
		{
		}

		public InfoHtmlMessage(string message, string controlId) : base(MessageType.InfoHtml, message, controlId)
		{
		}
	}
}