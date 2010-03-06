using System.Web.Mvc;
using MvcContrib.SimplyRestful;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ControllerRender
	{
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
		///            Controller&lt;UserController>
		///                .ShouldRenderItself("New")
		///                .WhenCalling(x => x.New());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fluentAction">The fluent action.</param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRenderView<T>(this ActionExpectations<T> fluentAction, RestfulAction action)
			where T : ControllerBase, new()
		{
			return fluentAction.ShouldRenderView(action.ToString());
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
		///            Controller&lt;UserController>
		///                .ShouldRenderItself("New")
		///                .WhenCalling(x => x.New());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fluentAction">The fluent action.</param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldRenderItself<T>(this ActionExpectations<T> fluentAction,
		                                                          RestfulAction action)
			where T : ControllerBase, new()
		{
			return fluentAction.ShouldRenderItself(action.ToString());
		}
	}
}