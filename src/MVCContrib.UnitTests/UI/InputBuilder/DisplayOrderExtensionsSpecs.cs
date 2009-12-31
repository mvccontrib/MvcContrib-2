using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Attributes;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
    [TestFixture]
    public class DisplayOrderExtensionsSpecs
    {
        [Test]
        public void When_order_attributes_are_present_the_properties_should_be_ordered()
        {
            var model = new ModelWithOrderedProperties { A = "A", B = "B", C = "C" };
            AssertOrderPreSort(model, "C", "B", "A");

            var result = model.GetType().GetProperties().ReOrderProperties();
            AssertOrderPostSort(model, result, "A", "B", "C");
        }

        [Test]
        public void When_no_order_attributes_are_present_the_properties_should_not_be_re_ordered()
        {
            var model = new ModelWithNoOrderedProperties { A = "A", B = "B", C = "C" };
            AssertOrderPreSort(model, "B", "A", "C");

            var result = model.GetType().GetProperties().ReOrderProperties();
            AssertOrderPostSort(model, result, "B", "A", "C");
        }

        private static void AssertOrderPreSort(object obj, params string[] expectedOrder)
        {
            var properties = obj.GetType().GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(obj, null) as string;
                value.ShouldEqual(expectedOrder[i]);
            }
        }

        private static void AssertOrderPostSort<T>(T model, PropertyInfo[] properties, params string[] expectedOrder)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(model, null) as string;
                value.ShouldEqual(expectedOrder[i]);
            }
        }

        public class ModelWithOrderedProperties
        {
            [DisplayOrder(3)]
            public string C { get; set; }

            [DisplayOrder(2)]
            public string B { get; set; }

            [DisplayOrder(1)]
            public string A { get; set; }
        }

        public class ModelWithNoOrderedProperties
        {
            public string B { get; set; }
            public string A { get; set; }
            public string C { get; set; }
        }
    }
}