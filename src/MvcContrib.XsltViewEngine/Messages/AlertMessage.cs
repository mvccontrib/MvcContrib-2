namespace MvcContrib.XsltViewEngine.Messages
{
	public class AlertMessage : Message
	{
		public AlertMessage(string message) : base(MessageType.Alert, message)
		{
		}

		public AlertMessage(string message, string controlId) : base(MessageType.Alert, message, controlId)
		{
		}
	}
}