namespace MvcContrib.IncludeHandling
{
	public interface IIncludeReader
	{
		string ToAbsolute(string source);
		Include Read(string source, IncludeType type);
	}
}