namespace MvcContrib.XsltViewEngine.Messages
{
	public class InfoMessage : Message
	{
		public InfoMessage(string message) : base(MessageType.Info, message)
		{
		}

		public InfoMessage(string message, string controlId) : base(MessageType.Info, message, controlId)
		{
		}
	}
}