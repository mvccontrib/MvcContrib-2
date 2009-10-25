namespace PortableAreaSpike
{
	public interface IAuthenticationService
	{
		bool IsValidLogin(string username, string password);
	}
}