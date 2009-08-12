using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib.TestHelper;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper
{
		[TestFixture]
		public class AssertionTester
		{
			[Test]
			public void the_assertion_shows_the_wrong_call_stack()
			{
				"This sample will show extra frames in the call stack".AssertionThatShowsExtraCallStackFrames();
			}

			[Test]
			public void the_assertion_shows_the_correct_call_stack()
			{
					"This sample will show extra frames in the call stack".AssertionThatShowsRemovesTheExtraCallStackFrames();
			}
		}
}

namespace MvcContrib.TestHelper
{
public static class stringExtension

{
	public static void AssertionThatShowsExtraCallStackFrames(this string value)
	{
		throw new Exception("the assertion failed");
	}

	public static void AssertionThatShowsRemovesTheExtraCallStackFrames(this string value)
	{
		throw new MvcContrib.TestHelper.AssertionException("This is much Better");
	}
}

}
