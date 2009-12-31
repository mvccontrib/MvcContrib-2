using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Attributes;
using MvcContrib.UI.InputBuilder.Conventions;

namespace MvcContrib.UI.InputBuilder.Helpers
{
    public static class DisplayOrderExtensions
    {
        public static PropertyInfo[] ReOrderProperties(this PropertyInfo[] properties)
        {
            var orderableProperties = new Dictionary<int, PropertyInfo>();
            var nonOrderableProperties = new List<PropertyInfo>();

            foreach (var property in properties)
            {
                if (property.AttributeExists<DisplayOrderAttribute>())
                {
                    var order = property.GetAttribute<DisplayOrderAttribute>().Order;
                    orderableProperties.Add(order, property);
                }
                else
                {
                    nonOrderableProperties.Add(property);
                }
            }

            var result = new List<PropertyInfo>();

            foreach (var property in orderableProperties.OrderBy(x => x.Key).ToList())
            {
                result.Add(property.Value);
            }

            foreach (var property in nonOrderableProperties.ToList())
            {
                result.Add(property);
            }

            return result.ToArray();
        }
    }
}