using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
    [TestFixture]
    public class InputPropertySpecificationTester
    {
        [Test]
        public void The_toString_should_call_the_render_method()
        {
            //arrange
            var spec = new InputPropertySpecification();
            var property = new PropertyViewModel();
            spec.Model = property;
            spec.Render = (a, b) =>
                              {
                                  Assert.AreEqual(property, b);
                                  return "foo";
                              };
            //act
            var result = spec.ToString();
            
			//assert
			Assert.AreEqual("foo",result);
		}
	}
}