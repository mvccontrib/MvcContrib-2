using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper.Fakes;
using Rhino.Mocks;

namespace MvcContrib.TestHelper.FluentController
{
	public class ActionExpectations<T> where T : ControllerBase, new()
	{
		private readonly List<Action<ActionResult>> Expectations = new List<Action<ActionResult>>();
		private HttpRequestBase MockRequest { get; set; }

		public ControllerBase MockController { get; set; }

		/// <summary>
		/// Shoulds the specified assertion.
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void CreateReturnsOK()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .Should(x => x.AssertResultIs&lt;HeadResult>().StatusCode.ShouldBe(HttpStatusCode.OK))
		///                .WhenCalling(x => x.Create());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="assertion">The assertion.</param>
		/// <returns></returns>
		public ActionExpectations<T> Should(Action<ActionResult> assertion)
		{
			if(assertion != null)
			{
				Expectations.Add(assertion);
			}

			return this;
		}

		/// <summary>
		/// Shoulds the specified assertion.
		/// This version also takes a reference to the mock controller for additional verifications
		/// <example>
		/// <code>
		///    [TestClass]
		///    public class UserControllerRedirectsTest
		///    {
		///        [TestMethod]
		///        public void CreateReturnsOK()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .Should((controller, actionResult) => actionResult.AssertResultIs&lt;HeadResult>().StatusCode.ShouldBe(HttpStatusCode.OK))
		///                .WhenCalling(x => x.Create());
		///        }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="assertion"></param>
		/// <returns></returns>
		public ActionExpectations<T> ShouldController(Action<IController, ActionResult> assertion)
		{
			if(assertion != null)
			{
				Expectations.Add(actionResult => assertion(MockController, actionResult));
			}

			return this;
		}

		/// <summary>
		/// Specifies setup on the mock controller.  This should be used when setting
		/// up the test case.
		/// <example>
		/// <code>
		///     GivenController.As&lt;UserController>()
		///         .WithSetup(x => x.SessionRepository = new Mock&lt;ISessionRepository>())
		///         .WhenCalling(x => x.Get());
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="setupAction">An action that is used to setup values on the mock controller.  It is passed
		/// an instance of the mock controller.</param>
		/// <returns>An instance of this option to allow chaining.</returns>
		public ActionExpectations<T> WithSetup(Action<IController> setupAction)
		{
			if(setupAction != null)
			{
				setupAction(MockController);
			}

			return this;
		}

		/// <summary>
		/// Creates the setup conditions for a <see cref="GivenController"/>.As with the <see cref="RequestContext"/> to allow for mocking on the object.
		/// This is the full version and there are shorter wrapper for specific headers <see cref="WithLocation"/>, <see cref="WithReferrer"/> and <see cref="WithUserAgent"/>.
		/// <example>
		/// <code>
		///     GivenController.As&lt;UserController>()
		///         .Should(x => x.AssertResultIs&lt;HeadResult>().StatusCode.ShouldBe(HttpStatusCode.OK))
		///         .WhenCalling(x => x.Show());
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="setupAction">The setup action.</param>
		/// <returns></returns>
		public ActionExpectations<T> WithRequest(Action<HttpRequestBase> setupAction)
		{
			if(setupAction != null)
			{
				if(MockRequest == null)
				{
					var fakeHttpContext = FakeHttpContext.Root();
					MockRequest = MockRepository.GenerateMock<HttpRequestBase>();
					var mockResponse = MockRepository.GenerateMock<HttpResponseBase>();

					fakeHttpContext.SetResponse(mockResponse);
					fakeHttpContext.SetRequest(MockRequest);

					MockController.ControllerContext = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
					                                                         MockController);
				}
				setupAction(MockRequest);
			}
			return this;
		}

		/// <summary>
		/// Creates the Location Header on a Requestfor a <see cref="GivenController"/>.As with the <see cref="RequestContext"/>.
		/// <example>
		/// <code>
		///     GivenController.As&lt;UserController>()
		///         .Should(x => x.WithLocation("http://from-somewhere.com"))
		///         .WhenCalling(x => x.Show());
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="headerLocation">The header location.</param>
		/// <returns></returns>
		public ActionExpectations<T> WithLocation(string headerLocation)
		{
			WithRequest(x => x.Stub(location => location.Url).Return(new Uri(headerLocation)));
			return this;
		}

		/// <summary>
		/// Creates the Referrer Header on a Requestfor a <see cref="GivenController"/>.As with the <see cref="RequestContext"/>.
		/// <example>
		/// <code>
		///     GivenController.As&lt;UserController>()
		///         .Should(x => x.WithReferrer("http://referred-from-somewhereelse.org"))
		///         .WhenCalling(x => x.Show());
		/// </code>
		/// </example>
		/// </summary>
		/// <returns></returns>
		public ActionExpectations<T> WithReferrer(string headerReferrer)
		{
			WithRequest(x => x.Stub(location => location.UrlReferrer).Return(new Uri(headerReferrer)));
			return this;
		}

		/// <summary>
		/// Creates the UserAgent Header on a Requestfor a <see cref="GivenController"/>.As with the <see cref="RequestContext"/>.
		/// <example>
		/// <code>
		///     GivenController.As&lt;UserController>()
		///         .Should(x => x.WithUserAgent("Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.12) Gecko/2009070611 Firefox/3.0.12 (.NET CLR 3.5.30729)"))
		///         .WhenCalling(x => x.Show());
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="headerUserAgent">The header location.</param>
		/// <returns></returns>
		public ActionExpectations<T> WithUserAgent(string headerUserAgent)
		{
			WithRequest(x => x.Stub(location => location.UserAgent).Return(headerUserAgent));
			return this;
		}

		/// <summary>
		/// Whens the calling.  This returns void as it should be the last action in a chain of commands.
		/// 
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
		///
		///        [TestMethod]
		///        public void UnsuccessfulCreateDisplaysNew()
		///        {
		///            GivenController.As&lt;UserController>()
		///                .ShouldRenderView("New")
		///                .IfCallFails()
		///                .WhenCalling(x => x.Create(null));
		///        }
		///
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
		/// <param name="action">The action.</param>
		public void WhenCalling(Func<T, ActionResult> action)
		{
			ActionResult actionResult = action((T)MockController);
			Expectations.ForEach(x => x(actionResult));
			MockController.VerifyAllExpectations();
		}
	}
}