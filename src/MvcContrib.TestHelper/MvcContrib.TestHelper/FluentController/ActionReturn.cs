using System.Net;
using System.Web.Mvc;

namespace MvcContrib.TestHelper.FluentController
{
	public static class ActionReturn
	{
		/// <summary>
		/// Shoulds the return head.
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
		/// <param name="statusCode">The status code.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldReturnHead<T>(this ActionExpectations<T> fluentAction,
		                                                        HttpStatusCode statusCode)
			where T : ControllerBase, new()
		{
			return
				fluentAction.Should(
					actionResult => actionResult.AssertResultIs<HeadResult>().StatusCode.ShouldBe(statusCode));
		}

		/// <summary>
		/// Shoulds the return file.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void NewReturnsItself()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldReturnFile("C:\person.jpg")
		///                .WhenCalling(x => x.ShowImage());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <param name="fileDownloadName">Name of the file download.</param>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldReturnFile<T>(this ActionExpectations<T> fluentAction,
		                                                        string fileDownloadName)
			where T : ControllerBase, new()
		{
			return
				fluentAction.Should(
					actionResult =>
					actionResult.AssertResultIs<FileResult>().FileDownloadName.ShouldBe(fileDownloadName));
		}

		/// <summary>
		/// Shoulds the return empty.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void SaveToWatchListShouldReturnEmptyResult()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldReturnEmpty()
		///                .WhenCalling(x => x.SaveToWatchList());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <returns></returns>
		public static ActionExpectations<T> ShouldReturnEmpty<T>(this ActionExpectations<T> fluentAction)
			where T : ControllerBase, new()
		{
			return fluentAction.Should(actionResult =>
			{
				if(actionResult == null)
				{
					return;
				}
				actionResult.AssertResultIs<EmptyResult>();
			});
		}
	}
}