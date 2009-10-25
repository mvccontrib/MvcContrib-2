namespace PortableAreaSpike
{
	public class UserRepository : IUserRepository
	{
		public User GetByUsername(string username)
		{
			return new User {Username = username};
		}
	}
}