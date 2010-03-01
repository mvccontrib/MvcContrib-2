using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper.FluentController;
using MvcContrib.UnitTests.TestHelper.FluentController.UI;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper.FluentController
{
    /// <summary>
    /// Summary description for UserRoutesTest
    /// </summary>
    [TestFixture]
    public class UserRoutesTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}",                                         // URL with parameters
                new { controller = "User", action = "Index", id = "" }  // Parameter defaults
                );

        }

        [Test]
        public void UserIndex()
        {
            "~/user"
                .ShouldMapTo<UserController>(action => action.Index());
        }


        [Test]
        [Ignore("Pending")]
        public void UserPut()
        {
            "~/user"
                .GivenIncomingAs(HttpVerbs.Put)
                .ShouldMapTo<UserController>(action => action.New());
        }

        [Test]
        [Ignore("Pending")]
        public void UserPutMimickedUsingPostWithPutMethodOnForm()
        {
            "~/user"
                .GivenIncomingAs(HttpVerbs.Post, HttpVerbs.Put)
                .ShouldMapTo<UserController>(action => action.New());
        }
    }
}