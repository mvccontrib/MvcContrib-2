using System;
using System.Net;
using System.Web.Mvc;
using MvcContrib.TestHelper.FluentController;
using MvcContrib.UnitTests.TestHelper.FluentController.Core;
using MvcContrib.UnitTests.TestHelper.FluentController.UI;
using NUnit.Framework;
using MvcContrib.SimplyRestful;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.TestHelper.FluentController
{
    /// <summary>
    /// Duplicated tests as per above testing the new frameworks
    /// </summary>
    [TestFixture]
    public class FluentControllerTest
    {
        [Test]
        public void SuccessfulIndex()
        {
            GivenController.As<UserController>()
                .ShouldRenderItself(RestfulAction.Index)
                .WhenCalling(x => x.Index());
        }

        [Test]
        public void SuccessfulCreateRedirectsToIndex_UsingRestfulAction()
        {

            GivenController.As<UserController>()
                .ShouldRedirectTo(RestfulAction.Index)
                .IfCallSucceeds()
                .WhenCalling(x => x.Create(null));
        }

        [Test]
        public void UnsuccessfulCreateDisplaysNew_UsingString()
        {
            GivenController.As<UserController>()
                .ShouldRenderView("New")
                .IfCallFails()
                .WhenCalling(x => x.Create(null));
        }

        [Test]
        public void ShowReturns200()
        {
            GivenController.As<UserController>()
                .ShouldReturnHead(HttpStatusCode.OK)
                .WhenCalling(x => x.Show());
        }

        [Test]
        public void GenericShould()
        {
            GivenController.As<UserController>()
                .Should(x => x.AssertResultIs<HeadResult>().StatusCode.ShouldBe(HttpStatusCode.OK))
                .WhenCalling(x => x.Show());
        }

        [Test]
        public void ModelIsPassedIntoIfSuccess()
        {
            var result = new CustomerResult { FirstName = "Bob" };

            GivenController.As<UserController>()
                .ShouldRenderView(RestfulAction.New)
                .Should(x => x.AssertResultIs<ViewResult>().ViewData.Model.ShouldNotBeNull())
                .IfCallSucceeds(result)
                .WhenCalling(x => x.CreateWithModel());

        }

        [Test]
        public void EmptyIfSuccessMethodDoesntThrowError()
        {
            GivenController.As<UserController>()
                .ShouldRenderView(RestfulAction.New)
                .IfCallSucceeds()
                .WhenCalling(x => x.CreateWithModel());
        }

        [Test]
        public void ShouldEmptyMatchesEmptyAction()
        {
            GivenController.As<UserController>()
                .ShouldReturnEmpty()
                .WhenCalling(x => x.EmptyAction());
        }

        [Test]
        public void NullResultIsAssertedAsEmpty()
        {
            GivenController.As<UserController>()
                .ShouldReturnEmpty()
                .WhenCalling(x => x.NullAction());
        }

        [Test]
        public void HeaderSetForLocation()
        {
            GivenController.As<UserController>()
                .WithLocation("http://localhost")
                .ShouldReturnEmpty()
                .WhenCalling(x => x.NullAction());
        }
        [Test]
        public void GenericHeaderSet()
        {
            GivenController.As<UserController>()
                .WithRequest(x => x.Stub(location => location.Url).Return(new Uri("http://localhost")))
                .ShouldReturnEmpty()
                .WhenCalling(x => x.CheckHeaderLocation());
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        [Ignore("Pending: unclear on why failing")]
        public void GenericWithoutHeaderSet_ThrowsException()
        {
            GivenController.As<UserController>()
                .ShouldReturnEmpty()
                .WhenCalling(x => x.CheckHeaderLocation());
        }
    }
}