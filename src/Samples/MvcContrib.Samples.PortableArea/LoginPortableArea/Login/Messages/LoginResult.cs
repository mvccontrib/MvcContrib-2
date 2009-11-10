using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Messages
{
	public class LoginResult : ICommandResult
	{
		public bool Success { get; set; }

		public string Message { get; set; }

		public string Username { get; set; }
	}
}