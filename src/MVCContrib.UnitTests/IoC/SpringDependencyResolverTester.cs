using System.IO;
using System.Text;
using MvcContrib.Spring;
using NUnit.Framework;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;

namespace MvcContrib.UnitTests.IoC
{
    public class SpringDependencyResolverTester
    {
        [TestFixture]
        public class WhenAValidTypeIsPassed : WhenAValidTypeIsPassedBase
        {
            public override void TearDown()
            {
                
            }
            public override void Setup()
            {
                string objectXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " +
                   "  <objects xmlns=\"http://www.springframework.net\" " +
                   "    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                   "    xsi:schemaLocation=\"http://www.springframework.net http://www.springframework.net/xsd/spring-objects.xsd\"> " +
                   "    <object id=\"SimpleDependency\" singleton=\"false\" type=\"MvcContrib.UnitTests.IoC.SimpleDependency\"/> " +
                   "    <object id=\"NestedDependency\" singleton=\"false\" type=\"MvcContrib.UnitTests.IoC.NestedDependency\" > " +
                   "      <constructor-arg> " +
                   "        <object type=\"MvcContrib.UnitTests.IoC.SimpleDependency\" /> " +
                   "      </constructor-arg> " +
                   "    </object> " +
                   "  </objects>";
                Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(objectXml));
                IResource resource = new InputStreamResource(stream, "In memory xml");
                IObjectFactory factory = new XmlObjectFactory(resource);
                _dependencyResolver = new SpringDependencyResolver(factory);

            }
        }
    }
}
