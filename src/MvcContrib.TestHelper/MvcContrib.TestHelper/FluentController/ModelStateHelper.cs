using System.Web.Mvc;
using MvcContrib.TestHelper.FluentController;

namespace MvcContrib.TestHelper.FluentController
{
    public static class ModelStateHelper
    {
        public static void SetModelStateValid<T>(ActionExpectations<T> action)
            where T : Controller, new()
        {
            action.MockController.Object.ModelState.Clear();
        }

        public static void SetModelStateInvalid<T>(ActionExpectations<T> action)
            where T : Controller, new()
        {
            action.MockController.Object.ModelState.AddModelError("Test Error", "Error message");
        }
       
    }
}