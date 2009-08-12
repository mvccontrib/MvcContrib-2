using System;
using MvcContrib.UI.InputBuilder;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class AssemblyResourceProviderTester
    {
        [Test]
        public void GetCacheKey_should_return_null()
        {
            //arrange
            var provider = new AssemblyResourceProvider();
            
            //act
            var result = provider.GetCacheKey("");

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void App_resource_path_should_find_input_builders()
        {
            //arrange
            var provider = new AssemblyResourceProvider();

            //act
            var result = provider.IsAppResourcePath("~/Views/InputBuilders/String.aspx");

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Get_cache_dep_should_return_null_for_builders()
        {
            //arrange
            var provider = new AssemblyResourceProvider();
            
            //act
            var result = provider.GetCacheDependency("~/Views/InputBuilders/foo.aspx", new string[0], DateTime.Now);

            //assert
            Assert.IsNull(result);

        }

        [Test]
        public void File_exists()
        {
            //arrange
            var provider = new AssemblyResourceProvider();

            //act
            var result = provider.FileExists("~/foo");

            //assert
            Assert.IsFalse(result);

        }
        
    }
}