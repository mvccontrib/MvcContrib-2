using Yahoo.Yui.Compressor;

namespace MvcContrib.IncludeHandling.Configuration
{
	public interface ICssMinifySettings
	{
		CssCompressionType CompressionType { get; }
	}
}