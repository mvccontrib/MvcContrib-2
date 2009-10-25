namespace PortableAreaSpike
{
	public interface IUserRepository
	{
		User GetByUsername(string username);
	}
}