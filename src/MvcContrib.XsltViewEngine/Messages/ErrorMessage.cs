namespace MvcContrib.XsltViewEngine.Messages
{
    public class ErrorMessage : Message
    {
        public ErrorMessage(string message) : base(MessageType.Error, message){}
        public ErrorMessage(string message, string controlId) : base(MessageType.Error, message, controlId){}
    }
}