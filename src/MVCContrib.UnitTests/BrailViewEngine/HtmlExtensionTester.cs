using System.Collections;
using System.IO;
using Boo.Lang;
using MvcContrib.BrailViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.BrailViewEngine
{

	[TestFixture]
	[Category("BrailViewEngine")]
	public class HtmlExtensionTester
	{
		private HtmlExtension _html;

		[SetUp]
		public void SetUp()
		{
			_html = new HtmlExtension(new StringWriter());
			_html.Flush();
		}

		[Test]
		public void Can_Output_Empty_Tag()
		{
			_html.Tag("a");

			Assert.AreEqual("<a></a>", _html.Output.ToString());
		}

		[Test]
		public void Can_Output_Tag_With_Block()
		{
			_html.Tag("a", new Callable(_html.Output));

			Assert.AreEqual("<a>Text</a>", _html.Output.ToString());
		}

		[Test]
		public void Can_Output_Tag_With_Attributes()
		{
			IDictionary attributes = new Hashtable();
			attributes["href"] = "http://mvccontrib.org";
			_html.Tag("a", attributes, null);

			Assert.AreEqual("<a href=\"http://mvccontrib.org\"></a>", _html.Output.ToString());
		}

		[Test]
		public void html_Outputs_Html_Tag()
		{
			_html.html(null);

			Assert.AreEqual("<html></html>", _html.Output.ToString());
		}

		[Test]
		public void text_Outputs_Argument()
		{
			_html.text("text");

			Assert.AreEqual("text", _html.Output.ToString());
		}

		[Test]
		public void p_Outputs_Paragraph_Tag()
		{
			_html.p(null);

			Assert.AreEqual("<p></p>", _html.Output.ToString());
		}

		class Callable : ICallable
		{
			private readonly TextWriter _output;

			public Callable(TextWriter output)
			{
				_output = output;
			}

			public object Call(object[] args)
			{
				_output.Write("Text");
				return null;
			}
		}
	}
}
