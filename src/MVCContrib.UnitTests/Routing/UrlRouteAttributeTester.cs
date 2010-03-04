using System;
using MvcContrib.Routing;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Routing
{
    /// <summary>
    /// Contains tests for the <see cref="UrlRouteAttribute"/> class.
    /// </summary>
    [TestFixture]
    public class UrlRouteAttributeTester
    {
        [Test]
        public void SetPath_WhenProvidedWithAValidOneWordPath_SetsCorrectly()
        {
            const string routePath = "Google";
            var attribute = new UrlRouteAttribute();

            attribute.Path = routePath;

            Assert.AreEqual(routePath, attribute.Path);
        }

        [Test]
        public void SetPath_WhenProvidedWithAValidMultiWordPath_SetsCorrectly()
        {
            const string routePath = "Search/Api/Google/Redirect";
            var attribute = new UrlRouteAttribute();

            attribute.Path = routePath;

            Assert.AreEqual(routePath, attribute.Path);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SetPath_WhenProvidedWithPathPrefixedWithASlash_ThrowsArgumentException()
        {
            const string routePath = "/Google";
            var attribute = new UrlRouteAttribute();

            attribute.Path = routePath;

            // Exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SetPath_WhenProvidedWithPathSuffixedWithASlash_ThrowsArgumentException()
        {
            const string routePath = "Google/";
            var attribute = new UrlRouteAttribute();

            attribute.Path = routePath;
            
            // Exception
        }
    }
}