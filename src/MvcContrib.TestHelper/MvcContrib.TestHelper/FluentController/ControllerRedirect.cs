using System.Web.Mvc;
using MvcContrib.SimplyRestful;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ControllerRedirect
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
		///            Controller&lt;UserController>
		///                .ShouldRedirectTo("Index")
		///                .IfCallSucceeds()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fluentAction">The fluent action.</param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRedirectTo<T>(this ActionExpectations<T> fluentAction, RestfulAction action)
			where T : ControllerBase, new()
		{
			return fluentAction.ShouldRedirectTo(action.ToString());
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
		///            Controller&lt;UserController>
		///                .ShouldRedirectTo("Home", "Index")
		///                .IfCallSucceeds()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fluentAction">The fluent action.</param>
		/// <param name="controller">The controller of the action.</param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRedirectTo<T>(this ActionExpectations<T> fluentAction, string controller,
		                                                        RestfulAction action)
			where T : ControllerBase, new()
		{
			return fluentAction.ShouldRedirectTo(controller, action.ToString());
		}
	}
}