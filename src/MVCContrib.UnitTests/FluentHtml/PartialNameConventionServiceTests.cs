using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib.FluentHtml;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
    [TestFixture]
    public class PartialNameConventionServiceTests
    {
        [Test]
        public void validate_default_convention_name()
        {
            Assert.That(PartialNameConventionService.PartialNameConvention, Is.EqualTo("{0}TypePartial"));
        }

        [Test]
        public void validate_partial_name_generation()
        {
            Assert.That(PartialNameConventionService.GeneratePartialName(typeof(PartialNameConventionServiceTests)),
                Is.EqualTo("PartialNameConventionServiceTestsTypePartial"));
        }

        [Test]
        public void validate_change_to_partial_name_convention()
        {
            var originalValue = PartialNameConventionService.PartialNameConvention;
            const string nameConvention = "foo{0}bar";

            PartialNameConventionService.PartialNameConvention = nameConvention;
            Assert.That(PartialNameConventionService.PartialNameConvention, Is.EqualTo(nameConvention));

            PartialNameConventionService.PartialNameConvention = originalValue;
        }

    }
}
