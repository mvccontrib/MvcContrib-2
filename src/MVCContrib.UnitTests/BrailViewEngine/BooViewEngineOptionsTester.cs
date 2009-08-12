using System.Configuration;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class BooViewEngineOptionsTester
	{

		[Test]
		public void Can_Create_Options()
		{
			 new BooViewEngineOptions
			              	{
			              		Debug = true,
			              		SaveToDisk = true,
			              		BatchCompile = true,
			              		CommonScriptsDirectory = "CommonScripts",
			              		SaveDirectory = "SaveDirectory"
			              	};
		}

		[Test]
		public void Can_Read_From_AppConfig()
		{
			var options = ConfigurationManager.GetSection("brail") as BooViewEngineOptions;
			Assert.IsNotNull(options);
		}
	}
}
