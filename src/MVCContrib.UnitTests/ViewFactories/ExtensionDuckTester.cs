using System;
using System.Web.Mvc;
using MvcContrib.ViewEngines;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using NVelocity.Util.Introspection;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class ExtensionDuckTester
	{
	    private MockRepository _mocks;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockRepository();
        }

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_Instance()
		{
			new ExtensionDuck(null);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void Does_Not_Support_Get()
		{
			var duck = new ExtensionDuck(new object());
			duck.GetInvoke(null);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void Does_Not_Support_Set()
		{
			var duck = new ExtensionDuck(new object());
			duck.SetInvoke(null, null);
		}

		[Test]
		public void Returns_Null_For_Empty_Invoke()
		{
			var duck = new ExtensionDuck(new object());
			Assert.IsNull(duck.Invoke(string.Empty));
		}

		[Test]
		public void ForCoverage()
		{
			object o = new ExtensionDuck(new object()).Introspector;
		}

        [Test]
        public void CanAddExtensionsToHtmlExtensionDuck()
        {
            var viewContext = _mocks.DynamicViewContext("someView");
            var viewDataContainer = _mocks.DynamicMock<IViewDataContainer>();

            HtmlExtensionDuck.AddExtension(typeof(HtmlExtensionForTesting));
            var htmlExtensionDuck = new HtmlExtensionDuck(viewContext, viewDataContainer);

            object result = htmlExtensionDuck.Invoke("Foo");

            Assert.That(result, Is.EqualTo("Bar"));
                        
        }

		[Test]
		public void CanFindMethodWithAmbiguousOverloads() 
		{
			var viewContext = _mocks.DynamicViewContext("someView");
			var viewDataContainer = _mocks.DynamicMock<IViewDataContainer>();

			HtmlExtensionDuck.AddExtension(typeof(HtmlExtensionForTesting));
			var htmlExtensionDuck = new HtmlExtensionDuck(viewContext, viewDataContainer);

			object result = htmlExtensionDuck.Invoke("Bar", "x");

			Assert.That(result, Is.EqualTo("Bar"));

		}

		[Test]
		public void CanInvokeMethodWithParamsArray()
		{
			var viewContext = _mocks.DynamicViewContext("someView");
			var viewDataContainer = _mocks.DynamicMock<IViewDataContainer>();

			HtmlExtensionDuck.AddExtension(typeof(HtmlExtensionForTesting));
			var htmlExtensionDuck = new HtmlExtensionDuck(viewContext, viewDataContainer);
			Object[] args = {"foo", "bar"};
			object result = htmlExtensionDuck.Invoke("Bar", args);

			Assert.That(result, Is.EqualTo("Bar3"));
		}
	}

    public static class HtmlExtensionForTesting
    {
        public static string Foo(this HtmlHelper html)
        {
            return "Bar";
        }

		public static string Bar(this HtmlHelper htmlHelper, string arg)
		{
			return "Bar";
		}

		public static string Bar(this HtmlHelper htmlHelper, object arg) 
		{
			return "Bar2";
		}

      public static string Bar(this HtmlHelper htmlHelper, params object[] args)
      {
         return "Bar3";
      }
    }
}
