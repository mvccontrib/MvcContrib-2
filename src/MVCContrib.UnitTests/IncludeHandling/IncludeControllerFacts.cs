using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;
using Rhino.Mocks;
using Xunit;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class IncludeControllerFacts
	{
		private readonly IncludeController _controller;
		private readonly IIncludeCombiner _mockCombiner;
		private readonly MockRepository _mocks;
		private readonly IIncludeHandlingSettings _mockSettings;

		public IncludeControllerFacts()
		{
			_mocks = new MockRepository();
			_mockSettings = _mocks.StrictMock<IIncludeHandlingSettings>();
			_mockCombiner = _mocks.StrictMock<IIncludeCombiner>();
			_controller = new IncludeController(_mockSettings, _mockCombiner);
			_mocks.ReplayAll();
		}

		[Fact]
		public void Css_ShouldAskCombinerForCombinationMatchingKey()
		{
			var combination = new IncludeCombination(IncludeType.Css, new[] { "foo.css" }, "#Foo{color:red;}", DateTime.UtcNow, new CssTypeElement());
			_mockSettings.Expect(s => s.Types[IncludeType.Css]).Return(new CssTypeElement());
			_mockCombiner.Expect(c => c.GetCombination("foo")).Return(combination);
			ActionResult result = null;
			Assert.DoesNotThrow(() => result = _controller.Css("foo"));

			Assert.IsType<IncludeCombinationResult>(result);
			Assert.Equal(combination, ((IncludeCombinationResult) result).Combination);
			_mocks.VerifyAll();
		}

		[Fact]
		public void Js_ShouldAskCombinerForCombinationMatchingKey()
		{
			var combination = new IncludeCombination(IncludeType.Js, new[] { "foo.js" }, "alert('foo!');", DateTime.UtcNow, new JsTypeElement());
			_mockSettings.Expect(s => s.Types[IncludeType.Js]).Return(new JsTypeElement());
			_mockCombiner.Expect(c => c.GetCombination("foo")).Return(combination);
			ActionResult result = null;
			Assert.DoesNotThrow(() => result = _controller.Js("foo"));

			Assert.IsType<IncludeCombinationResult>(result);
			Assert.Equal(combination, ((IncludeCombinationResult) result).Combination);
			_mocks.VerifyAll();
		}

		[Fact]
		public void Index_ShouldAskCombinerForAllCombinations_AndAllIncludes()
		{
			_mockCombiner.Expect(c => c.GetAllIncludes()).Return(new List<Include>());
			_mockCombiner.Expect(c => c.GetAllCombinations()).Return(new Dictionary<string, IncludeCombination>());

			ActionResult result = null;
			Assert.DoesNotThrow(() => result = _controller.Index());
			Assert.IsType<ViewResult>(result);
			_mocks.VerifyAll();
		}

		[Fact]
		public void Clear_ShouldTellCombinerToClear()
		{
			_mockCombiner.Expect(c => c.Clear());
			ActionResult result = null;
			Assert.DoesNotThrow(() => result = _controller.Clear());
			Assert.IsType<RedirectToRouteResult>(result);
			_mocks.VerifyAll();
		}
	}
}