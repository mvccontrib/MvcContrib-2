using LoginPortableArea.Login.Models;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Messages
{
	public class LoginInputMessage : ICommandMessage<LoginResult>
	{
		public LoginResult Result { get; set; }
		public LoginInput Input { get; set; }
	}
}