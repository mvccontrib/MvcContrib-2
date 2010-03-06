using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace MVCContrib.Application.UnitTests.TestHelper
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
        public void UserShow()
        {
                         "~/user"
                           .GivenIncomingAs(HttpVerbs.Put)
                           .ShouldMapTo<UserController>(action => action.Index());
        }

    }
}