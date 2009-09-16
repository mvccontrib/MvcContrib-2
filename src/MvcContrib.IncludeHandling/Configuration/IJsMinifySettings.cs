namespace MvcContrib.IncludeHandling.Configuration
{
	public interface IJsMinifySettings
	{
		bool Verbose { get; }
		bool Obfuscate { get; }
		bool PreserveSemiColons { get; }
		bool DisableOptimizations { get; }
	}
}