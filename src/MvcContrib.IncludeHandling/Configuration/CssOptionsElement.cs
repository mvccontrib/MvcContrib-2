using System.Configuration;
using Yahoo.Yui.Compressor;

namespace MvcContrib.IncludeHandling.Configuration
{
	public class CssOptionsElement : ConfigurationElement, ICssMinifySettings
	{
		private const string COMPRESSIONTYPE = "compressionType";

		[ConfigurationProperty(COMPRESSIONTYPE, DefaultValue = CssCompressionType.StockYuiCompressor)]
		public CssCompressionType CompressionType
		{
			get
			{
				try
				{
					var type = this[COMPRESSIONTYPE].ToString();
					return type.CastToEnum<CssCompressionType>();
				}
				catch
				{
					return CssCompressionType.StockYuiCompressor;
				}
			}
		}
	}
}