using System.Web.Mvc;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ModelStateHelper
	{
		public static void SetModelStateValid<T>(ActionExpectations<T> action)
			where T : Controller, new()
		{
			action.MockController.ViewData.ModelState.Clear();
		}

		public static void SetModelStateInvalid<T>(ActionExpectations<T> action)
			where T : Controller, new()
		{
			action.MockController.ViewData.ModelState.AddModelError("Test Error", "Error message");
		}
	}
}