using Boo.Lang.Compiler.Ast;
using MvcContrib.BrailViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.BrailViewEngine
{

	[TestFixture]
	[Category("BrailViewEngine")]
	public class HtmlAttributeTester
	{
		[Test]
		public void Can_Transform_Data()
		{
			var html = new HtmlAttribute();

			Assert.AreEqual("MvcContrib.BrailViewEngine.HtmlAttribute.Transform", html.TransformMethodName);
			Assert.AreEqual("this &amp; that", HtmlAttribute.Transform("this & that"));
			Assert.AreEqual("this &amp; that", HtmlAttribute.Transform((object)"this & that"));

			html.Apply(new Method());
		}
	}
}
