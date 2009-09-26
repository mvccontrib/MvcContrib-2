using System.Configuration;
using Yahoo.Yui.Compressor;

namespace MvcContrib.IncludeHandling.Configuration
{
	public class CssTypeElement : IncludeTypeElement, ICssMinifySettings
	{
		[ConfigurationProperty(OPTIONS)]
		private CssOptionsElement cssOptions
		{
			get { return (CssOptionsElement) this[OPTIONS] ?? new CssOptionsElement(); }
		}

		#region ICssMinifySettings Members

		public CssCompressionType CompressionType
		{
			get { return cssOptions.CompressionType; }
		}

		#endregion
	}
}