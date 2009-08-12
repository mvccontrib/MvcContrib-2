namespace MvcContrib.XsltViewEngine.Messages
{
	public class Message : IMessage
	{
		private readonly MessageType messageType;

		public Message(MessageType messageType)
		{
			this.messageType = messageType;
		}

		public Message(MessageType messageType, string message) : this(messageType)
		{
			Content = message;
		}

		public Message(MessageType messageType, string message, string controlID) : this(messageType, message)
		{
			ControlID = controlID;
		}

		#region IMessage Members

		public string ControlID
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public MessageType MessageType
		{
			get { return messageType; }
		}

		#endregion
	}
}