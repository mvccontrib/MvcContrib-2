using MvcContrib.UI.InputBuilder;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class VirtualFileTester
    {

        [Test]
        public void The_file_should_locate_a_embedded_resource()
        {
            //arrange
            var file = new AssemblyResourceVirtualFile("~/Views/InputBuilders/String.aspx");
            
            //act
            var result = file.Open();

            //assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void The_file_should_not_locate_an_invalid_path()
        {
            //arrange
            var file = new AssemblyResourceVirtualFile("~/foo");

            //act
            var result = file.Open();

            //assert
            Assert.IsNull(result);
        }
    }
}