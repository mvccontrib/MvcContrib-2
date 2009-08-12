namespace MvcContrib.SimplyRestful
{
	public enum RestfulAction
	{
		None = 16384,
		Show = 1,
		Create = 2,
		Update = 4,
		Destroy = 8,
		Index = 16,
		New = 32,
		Edit = 64,
		Delete = 128
	}
}