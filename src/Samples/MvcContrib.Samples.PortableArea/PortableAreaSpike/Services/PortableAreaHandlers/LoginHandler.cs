using LoginPortableArea.Login.Messages;
using MvcContrib.PortableAreas;

namespace PortableAreaSpike
{
	public class LoginHandler : MessageHandler<LoginInputMessage>
	{
		private readonly IAuthenticationService _authenticationService;

		public LoginHandler(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		public LoginHandler() : this(new MockAuthenticationService(false))
		{
		}

		public override void Handle(LoginInputMessage message)
		{
			if (_authenticationService.IsValidLogin(message.Input.Username, message.Input.Password))
			{
				message.Result.Success = true;
				message.Result.Username = message.Input.Username;
			}
			else
			{
				message.Result.Message = "Username or Password was incorrect";
			}
		}
	}
}