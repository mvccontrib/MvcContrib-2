using System.Web.Mvc;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ActionRedirect
	{
		/// <summary>
		/// Shoulds the redirect to.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void SuccessfulCreateRedirectsToIndex()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldRedirectTo("Index")
		///                .IfCallSucceeds()
		///                .WhenCalling(x => x.Create(null));
		///        }
		///</code>
		/// </example>
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRedirectTo<T>(this ActionExpectations<T> fluentAction,
		                                                        string action)
			where T : ControllerBase, new()
		{
			return fluentAction.Should(actionResult => actionResult.AssertActionRedirect().ToAction(action));
		}

		/// <summary>
		/// Shoulds the redirect to.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void SuccessfulCreateRedirectsToIndexActionOfHomeController()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldRedirectTo("Home", "Index")
		///                .IfCallSucceeds()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <param name="controller">The controller of the action.</param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRedirectTo<T>(this ActionExpectations<T> fluentAction,
		                                                        string controller, string action)
			where T : ControllerBase, new()
		{
			return fluentAction.Should(
				actionResult =>
				actionResult.AssertActionRedirect().ToAction(action).ToController(controller)
				);
		}
	}
}