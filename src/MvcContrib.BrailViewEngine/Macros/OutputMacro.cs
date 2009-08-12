// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// MODIFICATIONS HAVE BEEN MADE TO THIS FILE

namespace MvcContrib.BrailViewEngine
{
	using System;
	using Boo.Lang.Extensions;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;

	// This take statements such as:
	//  output "something"
	// and turn them into:
	//  outputStream.Write("something")
	public class OutputMacro : LexicalInfoPreservingMacro
	{
		private static readonly ReferenceExpression output = AstUtil.CreateReferenceExpression("OutputStream.Write");

		private static void UnescapeInitialAndClosingDoubleQuotes(MacroStatement macro)
		{
			var value = macro.Arguments[0] as StringLiteralExpression;
			if (value == null) return;
			value.Value = BrailPreProcessor.UnescapeInitialAndClosingDoubleQuotes(value.Value);
		}

		protected override Statement ExpandImpl(MacroStatement macro)
		{
			if (macro.Arguments.Count == 0) throw new Exception("output must be called with arguemnts");
			UnescapeInitialAndClosingDoubleQuotes(macro);
			return PrintMacroModule.expandPrintMacro(macro, output, output);
		}
	}
}
