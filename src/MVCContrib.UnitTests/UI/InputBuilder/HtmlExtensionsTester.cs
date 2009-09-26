using System.Web.Mvc;
using MvcContrib.UI.InputBuilder;
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
			HtmlExtensions.Render = (a, b, c) => "";

			var helper = InputModelPropertyFactoryTester.CreateHelper(new Model());
			//act

			var result = helper.SubmitButton();
            
			//assert
			Assert.IsEmpty(result);
		}
	}
}