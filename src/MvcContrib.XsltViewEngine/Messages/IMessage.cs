namespace MvcContrib.XsltViewEngine.Messages
{
	public interface IMessage
	{
		string ControlID { get; set; }
		string Content { get; set; }
		MessageType MessageType { get; }
	}
}