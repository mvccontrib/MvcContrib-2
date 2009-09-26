using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IncludeHandling
{
	[TestFixture]
	public class IncludeControllerTester
	{
		private IncludeController _controller;
		private IIncludeCombiner _mockCombiner;
		private MockRepository _mocks;
		private IIncludeHandlingSettings _mockSettings;

		[SetUp]
		public void TestSetup()
		{
			_mocks = new MockRepository();
			_mockSettings = _mocks.StrictMock<IIncludeHandlingSettings>();
			_mockCombiner = _mocks.StrictMock<IIncludeCombiner>();
			_controller = new IncludeController(_mockSettings, _mockCombiner);
			_mocks.ReplayAll();
		}

		[Test]
		public void Css_ShouldAskCombinerForCombinationMatchingKey()
		{
			var combination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#Foo{color:red;}", DateTime.UtcNow, new CssTypeElement());
			_mockSettings.Expect(s => s.Types[IncludeType.Css]).Return(new CssTypeElement());
			_mockCombiner.Expect(c => c.GetCombination("foo")).Return(combination);
			ActionResult result = _controller.Css("foo");

			Assert.IsInstanceOf<IncludeCombinationResult>(result);
			Assert.AreEqual(combination, ((IncludeCombinationResult) result).Combination);
			_mocks.VerifyAll();
		}

		[Test]
		public void Js_ShouldAskCombinerForCombinationMatchingKey()
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, "alert('foo!');", DateTime.UtcNow, new JsTypeElement());
			_mockSettings.Expect(s => s.Types[IncludeType.Js]).Return(new JsTypeElement());
			_mockCombiner.Expect(c => c.GetCombination("foo")).Return(combination);
			ActionResult result = _controller.Js("foo");

			Assert.IsInstanceOf<IncludeCombinationResult>(result);
			Assert.AreEqual(combination, ((IncludeCombinationResult)result).Combination);
			_mocks.VerifyAll();
		}

		[Test]
		public void Index_ShouldAskCombinerForAllCombinations_AndAllIncludes()
		{
			_mockCombiner.Expect(c => c.GetAllIncludes()).Return(new List<Include>());
			_mockCombiner.Expect(c => c.GetAllCombinations()).Return(new Dictionary<string, IncludeCombination>());

			ActionResult result = _controller.Index();
			Assert.IsInstanceOf<ViewResult>(result);
			_mocks.VerifyAll();
		}

		[Test]
		public void Clear_ShouldTellCombinerToClear()
		{
			_mockCombiner.Expect(c => c.Clear());
			ActionResult result = _controller.Clear();
			Assert.IsInstanceOf<RedirectToRouteResult>(result);
			_mocks.VerifyAll();
		}
	}
}