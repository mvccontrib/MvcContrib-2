using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Routing;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Routing
{
    /// <summary>
    /// Contains tests for the <see cref="RouteAttributeUtility"/> class.
	/// </summary>
    [TestFixture]
    public class RouteAttributeUtilityTester
    {
        [Test] 
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingValidUrlRoute_ShouldRegisterRouteCorrectly()
        {
            var routeCollection = new RouteCollection();
            
            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingValidUrlRouteController));
            
            Assert.IsInstanceOf(typeof(Route), routeCollection[0]);
            Route googleRedirectRoute = (Route)routeCollection[0];
            Assert.AreEqual("PerformSomeAction", googleRedirectRoute.Defaults["action"].ToString());
            Assert.AreEqual("ControllerContainingValidUrlRoute", googleRedirectRoute.Defaults["controller"].ToString());
            Assert.AreEqual("Google", googleRedirectRoute.Url);
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingRouteWithNoRouteName_ShouldRegisterRouteCorrectly()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingRouteWithNoRouteNameController));
            
            Assert.IsInstanceOf(typeof(Route), routeCollection[0]);
            Route googleRedirectRoute = (Route)routeCollection[0];
            Assert.AreEqual("PerformSomeAction", googleRedirectRoute.Defaults["action"].ToString());
            Assert.AreEqual("ControllerContainingRouteWithNoRouteName", googleRedirectRoute.Defaults["controller"].ToString());
            Assert.AreEqual("Google", googleRedirectRoute.Url);
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingOverriddenActionName_ShouldRegisterRouteCorrectly()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingOverriddenActionNameController));
            
            Assert.IsInstanceOf(typeof(Route), routeCollection[0]);
            Route googleRedirectRoute = (Route)routeCollection[0];
            Assert.AreEqual("OverriddenActionName", googleRedirectRoute.Defaults["action"].ToString());
            Assert.AreEqual("ControllerContainingOverriddenActionName", googleRedirectRoute.Defaults["controller"].ToString());
            Assert.AreEqual("Google", googleRedirectRoute.Url);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Route attribute present which contains no 'Path', cannot map route.  Either remove the attribute or add a path.")]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingUrlRouteAttributeWithNoPath_ShouldThrowInvalidOperationException()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingUrlRouteAttributeWithNoPathController));
           
            // Exception
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerUrlAttributeOrders_ShouldAddRoutesInPriorityOrder()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerWithUrlAttributeOrdersController));

            Assert.AreEqual("PerformSomeAction3", ((Route)routeCollection[0]).Defaults["action"].ToString());
            Assert.AreEqual("PerformSomeAction2", ((Route)routeCollection[1]).Defaults["action"].ToString());
            Assert.AreEqual("PerformSomeAction1", ((Route)routeCollection[2]).Defaults["action"].ToString());
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerWithDuplicateUrlRoutePathsNames_ShouldAddRoutesInPriorityOrder()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerWithDuplicateUrlRoutePathsNamesController));

            Assert.AreEqual("PerformSomeAction3", ((Route)routeCollection[0]).Defaults["action"].ToString());
            Assert.AreEqual("PerformSomeAction2", ((Route)routeCollection[1]).Defaults["action"].ToString());
            Assert.AreEqual("PerformSomeAction1", ((Route)routeCollection[2]).Defaults["action"].ToString());
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerWithUrlRouteConstraint_ShouldAddRouteWithConstraint()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingUrlRouteAndConstraintAttributesController));

            Assert.AreEqual(1, ((Route)routeCollection[0]).Constraints.Count);
            Assert.AreEqual("[a-zA-Z]+", ((Route)routeCollection[0]).Constraints["exampleRouteParameter"]);
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerWithMultipleUrlRouteConstraints_ShouldAddRouteWithAllRouteConstraints()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingMultipleUrlRouteConstraintsController));

            Assert.AreEqual(2, ((Route)routeCollection[0]).Constraints.Count);
            Assert.AreEqual("[a-zA-Z]+", ((Route)routeCollection[0]).Constraints["exampleRouteParameter"]);
            Assert.AreEqual("[a-zA-Z]+2", ((Route)routeCollection[0]).Constraints["exampleRouteParameter2"]);
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingUrlRouteAndDefaultAttributes_ShouldAddRouteWithDefaultParameterValue()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingUrlRouteAndDefaultAttributesController));

            Assert.AreEqual(3, ((Route)routeCollection[0]).Defaults.Count);
            Assert.AreEqual("blah", ((Route)routeCollection[0]).Defaults["exampleRouteParameter"]);
        }

        [Test]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingUrlRouteAndMultipleDefaultAttributes_ShouldAddRouteWithAllDefaultParameterValue()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingUrlRouteAndMultipleDefaultAttributesController));

            Assert.AreEqual(4, ((Route)routeCollection[0]).Defaults.Count);
            Assert.AreEqual("blah", ((Route)routeCollection[0]).Defaults["exampleRouteParameter"]);
            Assert.AreEqual("blah2", ((Route)routeCollection[0]).Defaults["exampleRouteParameter2"]);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingConstraintAttributesWithEmptyName_ShouldThrowInvalidOperationException()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingConstraintAttributesWithEmptyNameController));

            // Exception
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingConstraintAttributesWithEmptyRegex_ShouldThrowInvalidOperationException()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingConstraintAttributesWithEmptyRegexController));

            // Exception
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingDefaultAttributesWithEmptyName_ShouldThrowInvalidOperationException()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingDefaultAttributesWithEmptyNameController));

            // Exception
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterRoutesForController_WhenProvidedWithControllerContainingDefaultAttributesWithEmptyValue_ShouldThrowInvalidOperationException()
        {
            var routeCollection = new RouteCollection();

            RouteAttributeUtility.RegisterRoutesForController(routeCollection, typeof(ControllerContainingDefaultAttributesWithEmptyValueController));

            // Exception
        }

        #region Junk controllers for test support

        private class ControllerContainingValidUrlRouteController : Controller
        {
            [UrlRoute(Name = "Google", Path = "Google")]
            public ActionResult PerformSomeAction()
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingRouteWithNoRouteNameController : Controller
        {
            [UrlRoute(Path = "Google")]
            public ActionResult PerformSomeAction()
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingOverriddenActionNameController : Controller
        {
            [UrlRoute(Path = "Google")]
            [ActionName("OverriddenActionName")]
            public ActionResult PerformSomeAction()
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingUrlRouteAttributeWithNoPathController : Controller
        {
            [UrlRoute]
            public ActionResult PerformSomeAction()
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerWithUrlAttributeOrdersController : Controller
        {
            [UrlRoute(Path = "PerformSomeAction1", Order = 3)]
            public ActionResult PerformSomeAction1()
            {
                return new RedirectResult("http://www.google.com");
            }

            [UrlRoute(Path = "PerformSomeAction2", Order = 2)]
            public ActionResult PerformSomeAction2()
            {
                return new RedirectResult("http://www.google.com");
            }

            [UrlRoute(Path = "PerformSomeAction3", Order = 1)]
            public ActionResult PerformSomeAction3()
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerWithDuplicateUrlRoutePathsNamesController : Controller
        {
            [UrlRoute(Path = "Google", Order = 3)]
            public ActionResult PerformSomeAction1()
            {
                return new RedirectResult("http://www.google.com");
            }

            [UrlRoute(Path = "Google", Order = 2)]
            public ActionResult PerformSomeAction2()
            {
                return new RedirectResult("http://www.google.com");
            }

            [UrlRoute(Path = "Google", Order = 1)]
            public ActionResult PerformSomeAction3()
            {
                return new RedirectResult("http://www.google.com");
            }
        }
        
        private class ControllerContainingUrlRouteAndConstraintAttributesController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterConstraint(Name = "exampleRouteParameter", Regex = "[a-zA-Z]+")]
            public ActionResult PerformSomeAction(string exampleRouteParameter)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingMultipleUrlRouteConstraintsController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterConstraint(Name = "exampleRouteParameter", Regex = "[a-zA-Z]+")]
            [UrlRouteParameterConstraint(Name = "exampleRouteParameter2", Regex = "[a-zA-Z]+2")]
            public ActionResult PerformSomeAction(string exampleRouteParameter, string exampleRouteParameter2)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingUrlRouteAndDefaultAttributesController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterDefault(Name = "exampleRouteParameter", Value = "blah")]
            public ActionResult PerformSomeAction(string exampleRouteParameter)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingUrlRouteAndMultipleDefaultAttributesController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterDefault(Name = "exampleRouteParameter", Value = "blah")]
            [UrlRouteParameterDefault(Name = "exampleRouteParameter2", Value = "blah2")]
            public ActionResult PerformSomeAction(string exampleRouteParameter, string exampleRouteParameter2)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingConstraintAttributesWithEmptyNameController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterConstraint(Regex = "[a-zA-Z]+")]
            public ActionResult PerformSomeAction(string exampleRouteParameter)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingConstraintAttributesWithEmptyRegexController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterConstraint(Name = "exampleRouteParameter", Regex = "")]
            public ActionResult PerformSomeAction(string exampleRouteParameter)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingDefaultAttributesWithEmptyNameController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterDefault(Value = "value")]
            public ActionResult PerformSomeAction(string exampleRouteParameter)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        private class ControllerContainingDefaultAttributesWithEmptyValueController : Controller
        {
            [UrlRoute(Path = "Google")]
            [UrlRouteParameterDefault(Name = "exampleRouteParameter")]
            public ActionResult PerformSomeAction(string exampleRouteParameter)
            {
                return new RedirectResult("http://www.google.com");
            }
        }

        #endregion
    }
}