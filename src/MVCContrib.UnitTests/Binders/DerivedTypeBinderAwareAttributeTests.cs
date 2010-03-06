using MvcContrib.Attributes;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Binders
{
    [TestFixture]
    public class DerivedTypeBinderAwareAttributeTests
    {
        [Test]
        public void attribute_instantiates_and_sets_property_correctly()
        {
            Assert.That(new DerivedTypeBinderAwareAttribute(typeof(DerivedTypeBinderAwareAttributeTests)).DerivedType.Name, 
                            Is.EqualTo(typeof(DerivedTypeBinderAwareAttributeTests).Name));
        }
    }
}
