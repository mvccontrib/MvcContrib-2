using System.Linq;
using MvcContrib.Attributes;
using MvcContrib.Binders;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Binders
{
    [TestFixture]
    [DerivedTypeBinderAware(typeof(int))]
    public class DerivedTypeModelBinderCacheTests
    {
        [Test]
        public void validate_declarative_registration_of_derived_types()
        {
            DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(DerivedTypeModelBinderCacheTests),
                                                             new[] {typeof(string)});

            Assert.That((from p in DerivedTypeModelBinderCache.GetDerivedTypes(typeof(DerivedTypeModelBinderCacheTests))
                         where p.Name == typeof(string).Name
                         select p).FirstOrDefault(), Is.Not.Null);

            DerivedTypeModelBinderCache.Reset();

            // next, let's validate that the cache was cleared by reset
            Assert.That((from p in DerivedTypeModelBinderCache.GetDerivedTypes(typeof(DerivedTypeModelBinderCacheTests))
                         where p.Name == typeof(string).Name
                         select p).FirstOrDefault(), Is.Null);

        }

        [Test]
        public void validate_attribute_scan_on_getDerivedTypes_call()
        {
            DerivedTypeModelBinderCache.Reset();

            Assert.That((from p in DerivedTypeModelBinderCache.GetDerivedTypes(typeof(DerivedTypeModelBinderCacheTests))
                             where p.Name == typeof(int).Name
                             select p).FirstOrDefault(), Is.Not.Null);

            DerivedTypeModelBinderCache.Reset();
        }
    }
}
