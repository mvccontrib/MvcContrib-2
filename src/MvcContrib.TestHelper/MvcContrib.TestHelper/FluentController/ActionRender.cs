using System.Web.Mvc;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ActionRender
	{
		/// <summary>
		/// Shoulds the render view.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void UnsuccessfulCreateDisplaysNew()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldRenderView("New")
		///                .IfCallFails()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <param name="viewName">Name of the view.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRenderView<T>(this ActionExpectations<T> fluentAction,
		                                                        string viewName)
			where T : ControllerBase, new()
		{
			return fluentAction.Should(actionResult => actionResult.AssertViewRendered().ForView(viewName));
		}

		/// <summary>
		/// Shoulds the render itself.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void NewReturnsItself()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldRenderItself("New")
		///                .WhenCalling(x => x.New());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <param name="actionName">Name of the action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRenderItself<T>(
			this ActionExpectations<T> fluentAction, string actionName)
			where T : ControllerBase, new()
		{
			return fluentAction.Should(actionResult => actionResult.AssertViewRendered().ForViewOrItself(actionName));
		}

		/// <summary>
		/// Shoulds the render partial.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void EditReturnsPartialAddress()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldRenderPartial("Address")
		///                .WhenCalling(x => x.Edit());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <param name="partialViewName">Partial name of the view.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRenderPartial<T>(
			this ActionExpectations<T> fluentAction, string partialViewName)
			where T : ControllerBase, new()
		{
			return
				fluentAction.Should(
					actionResult => actionResult.AssertResultIs<PartialViewResult>().ForView(partialViewName));
		}
	}
}