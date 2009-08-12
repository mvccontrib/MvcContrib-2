using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class CodeBuilderHelperTester
	{
		[Test]
		public void ForCoverage()
		{
			var statement = new MacroStatement();
			statement.Block.Statements.Add(new MacroStatement());

			CodeBuilderHelper.CreateCallableFromMacroBody(new BooCodeBuilder(new TypeSystemServices()), statement);
		}
	}
}
