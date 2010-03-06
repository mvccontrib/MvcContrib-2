using MvcContrib.FluentController;

namespace MvcContrib.TestHelper.FluentController
{
	public static class CallSuccess
	{
		/// <summary>
		/// Ifs the call succeeds.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void SuccessfulCreateRedirectsToIndex()
		///        {
		///            GivenController.As&lt;UserController>
		///                .ShouldRedirectTo("Index")
		///                .IfCallSucceeds()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <returns></returns>
		public static ActionExpectations<T> IfCallSucceeds<T>(this ActionExpectations<T> action)
			where T : AbstractFluentController, new()
		{
			return IfCallSucceeds<T, object>(action, null);
		}


		/// <summary>
		/// If the call succeeds.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void SuccessfulCreateRedirectsToIndex()
		///        {
		///            GivenController.As&lt;UserController>
		///                .ShouldRedirectTo("Index")
		///                .IfCallSucceeds()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="action"></param>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		public static ActionExpectations<T> IfCallSucceeds<T, TResult>(this ActionExpectations<T> action, TResult model)
			where T : AbstractFluentController, new()
		{
			ModelStateHelper.SetModelStateValid(action);
			action.MockController.ViewData.Model = model;
			return action;
		}

		/// <summary>
		/// If the call fails.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void SuccessfulCreateRedirectsToIndex()
		///        {
		///            GivenController.As&lt;UserController>
		///                .ShouldRedirectTo("Index")
		///                .IfCallFails()
		///                .WhenCalling(x => x.Create(null));
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <returns></returns>
		public static ActionExpectations<T> IfCallFails<T>(this ActionExpectations<T> action)
			where T : AbstractFluentController, new()
		{
			ModelStateHelper.SetModelStateInvalid(action);
			return action;
		}
	}
}