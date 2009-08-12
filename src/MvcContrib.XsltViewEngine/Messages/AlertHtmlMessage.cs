namespace MvcContrib.XsltViewEngine.Messages
{
	public class AlertHtmlMessage : Message
	{
		public AlertHtmlMessage(string message) : base(MessageType.AlertHtml, message)
		{
		}

		public AlertHtmlMessage(string message, string controlId) : base(MessageType.AlertHtml, message, controlId)
		{
		}
	}
}