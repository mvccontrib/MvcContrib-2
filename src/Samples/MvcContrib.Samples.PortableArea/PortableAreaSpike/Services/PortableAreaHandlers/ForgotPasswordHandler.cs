using LoginPortableArea.Login.Messages;
using MvcContrib.PortableAreas;

namespace PortableAreaSpike
{
	public class ForgotPasswordHandler : MessageHandler<ForgotPasswordInputMessage>
	{
		private readonly IUserRepository _userRepository;

		public ForgotPasswordHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public ForgotPasswordHandler() : this(new UserRepository())
		{
		}

		public override void Handle(ForgotPasswordInputMessage message)
		{
			User user = _userRepository.GetByUsername(message.Input.Username);
			if (user != null)
			{
				message.Result.Success = true;
				message.Result.Message = "An email has been sent to foo@bar.com";
			}
			else
			{
				message.Result.Message = "Username or Password was incorrect";
			}
		}
	}
}