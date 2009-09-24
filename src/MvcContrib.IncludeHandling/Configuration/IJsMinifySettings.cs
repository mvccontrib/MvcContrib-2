namespace MvcContrib.IncludeHandling.Configuration
{
	public interface IJsMinifySettings
	{
		bool Obfuscate { get; }
		bool PreserveSemiColons { get; }
		bool DisableOptimizations { get; }
	}
}