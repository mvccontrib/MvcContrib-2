using System.Web.Mvc;
using Rhino.Mocks;

namespace MvcContrib.TestHelper.FluentController
{
	/// <summary>
	/// Given Controller provides a test context as a controller for writing tests around the flow of actions. 
	/// <example>
	/// This example provides the context of the UserController. In this case, the action redirects when create actions is complete
	/// <code>
	///     GivenController.As&lt;UserController>()
	///         .Should(x => x.AssertHttpRedirect())
	///         .WhenCalling(x => x.Create());
	/// </code>
	/// </example>
	/// </summary>
	public static class GivenController
	{
		/// <summary>
		/// As chains from a <seealso cref="GivenController"/>.
		/// <example>
		/// <code>GivenController.As&lt;UserController>()</code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static ActionExpectations<T> As<T>() where T : ControllerBase, new()
		{
			return new ActionExpectations<T>
			{
				MockController = MockRepository.GeneratePartialMock<T>()
			};
		}
	}
}