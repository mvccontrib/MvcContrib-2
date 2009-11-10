using LoginPortableArea.Login.Controllers;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Messages
{
	public class ForgotPasswordInputMessage : ICommandMessage<ForgotPasswordResult>
	{
		public ForgotPasswordInput Input { get; set; }
		public ForgotPasswordResult Result { get; set; }
	}
}