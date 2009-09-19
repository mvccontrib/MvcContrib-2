using System;
using System.Web.Hosting;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class InputBuilderTester
    {
        private VirtualPathProvider _virtualPathProvider;

        [Test]
        public void The_bootstrapper_should_wire_up_the_view_engine()
        {
            //arrange
            ViewEngines.Engines.Add(new WebFormViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());

            MvcContrib.UI.InputBuilder.InputBuilder.RegisterPathProvider=FakeRegister;
            //act
            MvcContrib.UI.InputBuilder.InputBuilder.BootStrap();

            //assert
            Assert.AreEqual(ViewEngines.Engines.Count, 1);
            Assert.IsInstanceOf<AssemblyResourceProvider>(_virtualPathProvider);
        }

        private void FakeRegister(VirtualPathProvider virtualPathProvider)
        {
            _virtualPathProvider = virtualPathProvider;
        }
    }
}