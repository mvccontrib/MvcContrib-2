using System.Configuration;

namespace MvcContrib.IncludeHandling.Configuration
{
	public class JsOptionsElement : ConfigurationElement, IJsMinifySettings
	{
		private const string DISABLEOPTIMIZATIONS = "disableOptimizations";
		private const string OBFUSCATE = "obfuscate";
		private const string PRESERVESEMICOLONS = "preserveSemiColons";

		#region IJsMinifySettings Members

		[ConfigurationProperty(OBFUSCATE, DefaultValue = true)]
		public bool Obfuscate
		{
			get { return (bool) this[OBFUSCATE]; }
		}

		[ConfigurationProperty(PRESERVESEMICOLONS, DefaultValue = true)]
		public bool PreserveSemiColons
		{
			get { return (bool) this[PRESERVESEMICOLONS]; }
		}

		[ConfigurationProperty(DISABLEOPTIMIZATIONS, DefaultValue = false)]
		public bool DisableOptimizations
		{
			get { return (bool) this[DISABLEOPTIMIZATIONS]; }
		}

		#endregion
	}
}