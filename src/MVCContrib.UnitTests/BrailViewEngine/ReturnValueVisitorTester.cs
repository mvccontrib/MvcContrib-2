using Boo.Lang.Compiler.Ast;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class ReturnValueVisitorTester
	{
		[Test]
		public void ForCoverage()
		{
			var block = new Block();
			var statement = new ReturnStatement(new StringLiteralExpression("literal"));
			block.Statements.Add(statement);

			var visitor = new ReturnValueVisitor();
			bool found = visitor.Found;
			visitor.OnReturnStatement(statement);
		}
	}
}
