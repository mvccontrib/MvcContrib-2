namespace PortableAreaSpike
{
	public class MockAuthenticationService : IAuthenticationService
	{
		private readonly bool _b;

		public MockAuthenticationService(bool b)
		{
			_b = b;
		}

		public bool IsValidLogin(string username, string password)
		{
			return username.Equals("admin") && password.Equals("password");
		}
	}
}