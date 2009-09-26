using MvcContrib.UI.InputBuilder.Attributes;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class AttributeTester
	{
		[Test]
		public void Label_should_be_valid()
		{
			//arrange
			var label = new LabelAttribute("label");

			//act
			var result = label.IsValid(null);

			//assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Partial_should_be_valid()
		{
			//arrange
			var label = new PartialViewAttribute("label");

			//act
			var result = label.PartialView;

			//assert
			Assert.AreEqual("label",result);
		}

		[Test]
		public void example_should_be_valid()
		{
			//arrange
			var label = new ExampleAttribute("label");

			//act
			var result = label.IsValid(null);

			//assert
			Assert.IsTrue(result);
		}
        
	}
}