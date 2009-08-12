namespace MvcContrib.XsltViewEngine.Messages
{
	public class ErrorHtmlMessage : Message
	{
		public ErrorHtmlMessage(string message) : base(MessageType.ErrorHtml, message)
		{
		}

		public ErrorHtmlMessage(string message, string controlId) : base(MessageType.ErrorHtml, message, controlId)
		{
		}
	}
}