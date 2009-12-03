using System;
using System.Web.Hosting;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.ViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class InputBuilderTester
	{
		private VirtualPathProvider _virtualPathProvider;

		[Test]
		public void The_bootstrapper_should_wire_up_the_view_engine()
		{
			//arrange
            
			MvcContrib.UI.InputBuilder.InputBuilder.RegisterPathProvider=FakeRegister;
			//act
			MvcContrib.UI.InputBuilder.InputBuilder.BootStrap();

			//assert
			Assert.AreEqual(System.Web.Mvc.ViewEngines.Engines.Count, 2);
			Assert.IsInstanceOf<AssemblyResourceProvider>(_virtualPathProvider);
		}

		private void FakeRegister(VirtualPathProvider virtualPathProvider)
		{
			_virtualPathProvider = virtualPathProvider;
		}
	}
}