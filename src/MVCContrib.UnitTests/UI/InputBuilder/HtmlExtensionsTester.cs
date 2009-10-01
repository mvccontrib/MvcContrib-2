using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class HtmlExtensionsTester
	{
		[Test]
		public void testname()
		{
			//arrange
			HtmlExtensions.Render = (a, b) => "";

			var helper = InputModelPropertyFactoryTester.CreateHelper(new Model());
			//act

			var result = helper.InputButtons();
            
			//assert
			Assert.IsEmpty(result);
		}
	}
}