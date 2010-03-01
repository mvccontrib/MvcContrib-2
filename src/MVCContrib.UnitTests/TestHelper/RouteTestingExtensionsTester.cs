using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper;
using MvcContrib.UnitTests.TestHelper.FluentController.UI;
using NUnit.Framework;

using Assert=NUnit.Framework.Assert;
using AssertionException=MvcContrib.TestHelper.AssertionException;

namespace MvcContrib.UnitTests.TestHelper
{
    [TestFixture]
    public class RouteTestingExtensionsTester
    {
		public class FunkyController : Controller
		{
			public ActionResult Index()
			{
				return null;
			}

			public ActionResult Bar(string id)
			{
				return null;
			}

			public ActionResult New()
			{
				return null;
			}

			public ActionResult List(Bar bar)
			{
				return null;
			}

			public ActionResult Foo(int id)
			{
				return null;
			}

			[AcceptVerbs(HttpVerbs.Post)]
			public ActionResult Zordo(int id)
			{
				return null;
			}

			public ActionResult Guid(Guid id)
			{
				return null;
			}

			public ActionResult Nullable(int? id)
			{
				return null;
			}

			public ActionResult DateTime(DateTime id)
			{
				return null;
			}

			public ActionResult NullableDateTime(DateTime? id)
			{
				return null;
			}
		}
		public class Bar
		{
			
		}

        public class AwesomeController : Controller
        {
        }

        [SetUp]
        public void Setup()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.IgnoreRoute("{resource}.gif/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                "default",
                "{controller}/{action}/{id}", 
                new { controller = "Funky", Action = "Index", id ="" });
        }

        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }

        [Test]
        public void should_be_able_to_pull_routedata_from_a_string()
        {
            var routeData = "~/charlie/brown".Route();
            Assert.That(routeData, Is.Not.Null);

            Assert.That(routeData.Values.ContainsValue("charlie"));
            Assert.That(routeData.Values.ContainsValue("brown"));
        }

        [Test]
        public void should_be_able_to_match_controller_from_route_data()
        {
            "~/".Route().ShouldMapTo<FunkyController>();            
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void should_be_able_to_detect_when_a_controller_doesnt_match()
        {
            "~/".Route().ShouldMapTo<AwesomeController>();            
        }

        [Test]
        public void should_be_able_to_match_action_with_lambda()
        {
            "~/".Route().ShouldMapTo<FunkyController>(x => x.Index());
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void should_be_able_to_detect_an_incorrect_action()
        {
            "~/".Route().ShouldMapTo<FunkyController>(x=>x.New());
        }

        [Test]
        public void should_be_able_to_match_action_parameters()
        {
            "~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("widget"));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void should_be_able_to_detect_invalid_action_parameters()
        {
            "~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("something_else"));
        }

        [Test, ExpectedException(typeof(AssertionException), ExpectedMessage = "Value for parameter 'id' did not match: expected 'widget' but was 'something_else'.")]
        public void should_provide_detailed_exception_message_when_detecting_invalid_action_parameters()
        {
            "~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("something_else"));
        }

        [Test]
        public void should_be_able_to_test_routes_directly_from_a_string()
        {
            "~/funky/bar/widget".ShouldMapTo<FunkyController>( x => x.Bar( "widget" ) );
        }

        [Test]
        public void should_be_able_to_test_routes_with_member_expressions_being_used()
        {
            var widget = "widget";

            "~/funky/bar/widget".ShouldMapTo<FunkyController>( x => x.Bar( widget ) );
        }

		[Test]
		public void should_be_able_to_test_routes_with_member_expressions_being_used_but_ignore_null_complex_parameteres()
		{
			

			"~/funky/List".ShouldMapTo<FunkyController>(x => x.List(null));
		}

        [Test]
        public void should_be_able_to_ignore_requests()
        {
            "~/someimage.gif".ShouldBeIgnored();
        }

        [Test]
        public void should_be_able_to_ignore_requests_with_path_info()
        {
            "~/someimage.gif/with_stuff".ShouldBeIgnored();
        }

		[Test]
		public void should_be_able_to_match_non_string_action_parameters()
		{
			"~/funky/foo/1234".Route().ShouldMapTo<FunkyController>(x => x.Foo(1234));
		}

        [Test]
        public void assertion_exception_should_hide_the_test_helper_frames_in_the_call_stack()
        {
            IEnumerable<string> callstack=new string[0];
            try
            {
                "~/badroute that is not configures/foo/1234".Route().ShouldMapTo<FunkyController>(x => x.Foo(1234));
            }
            catch(Exception ex)
            {

                callstack = ex.StackTrace.Split(new string[] { Environment.NewLine },StringSplitOptions.None);
            }
            callstack.Count().ShouldEqual(1);
            
        }
        
        [Test]
        public void should_be_able_to_generate_url_from_named_route()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute(
                "namedRoute",
                "{controller}/{action}/{id}",
                new { controller = "Funky", Action = "Index", id = "" });

            OutBoundUrl.OfRouteNamed("namedRoute").ShouldMapToUrl("/");
        }

        [Test]
        public void should_be_able_to_generate_url_from_controller_action_where_action_is_default()
        {
            OutBoundUrl.Of<FunkyController>(x => x.Index()).ShouldMapToUrl("/");
        }

        [Test]
        public void should_be_able_to_generate_url_from_controller_action()
        {
            OutBoundUrl.Of<FunkyController>(x => x.New()).ShouldMapToUrl("/Funky/New");
        }

        [Test]
        public void should_be_able_to_generate_url_from_controller_action_with_parameter()
        {
            OutBoundUrl.Of<FunkyController>(x => x.Foo(1)).ShouldMapToUrl("/Funky/Foo/1");
        }

        [Test]
        public void should_be_able_to_match_action_with_lambda_and_httpmethod()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute(
                "zordoRoute",
                "{controller}/{action}/{id}",
                new { controller = "Funky", Action = "Zordo", id = "0" },
                new {httpMethod = new HttpMethodConstraint("POST")});
            "~/Funky/Zordo/0".WithMethod(HttpVerbs.Post).ShouldMapTo<FunkyController>(x => x.Zordo(0));

        }

        [Test]
        public void should_not_be_able_to_get_routedata_with_wrong_httpmethod()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute(
                "zordoRoute",
                "{controller}/{action}/{id}",
                new { controller = "Funky", Action = "Zordo", id = "0" },
                new { httpMethod = new HttpMethodConstraint("POST") });
            var routeData = "~/Funky/Zordo/0".WithMethod(HttpVerbs.Get);
            Assert.IsNull(routeData);

        }

    	[Test]
    	public void should_match_guid()
    	{
			"~/funky/guid/80e70232-e660-40ae-af6b-2b2b8e87ee48".Route().ShouldMapTo<FunkyController>(c => c.Guid(new Guid("80e70232-e660-40ae-af6b-2b2b8e87ee48")));
    	}

    	[Test]
    	public void should_match_nullable_int()
    	{
			"~/funky/nullable/24".Route().ShouldMapTo<FunkyController>(c => c.Nullable(24));
    	}

    	[Test]
    	public void should_match_nullable_int_when_null()
    	{
			RouteTable.Routes.Clear();
			RouteTable.Routes.IgnoreRoute("{resource}.gif/{*pathInfo}");
			RouteTable.Routes.MapRoute(
				"default",
				"{controller}/{action}/{id}",
				new { controller = "Funky", Action = "Index", id = (int?)null });

			"~/funky/nullable".Route().ShouldMapTo<FunkyController>(c => c.Nullable(null));
    	}

    	[Test]
    	public void should_be_able_to_generate_url_with_nullable_int_action_parameter()
    	{
			OutBoundUrl.Of<FunkyController>(c => c.Nullable(24)).ShouldMapToUrl("/funky/nullable/24");
    	}

		[Test]
		public void should_match_datetime()
		{
			"~/funky/DateTime/2009-1-1".Route().ShouldMapTo<FunkyController>(x => x.DateTime(new DateTime(2009, 1, 1)));
		}

		[Test]
		public void should_match_nullabledatetime()
		{
			"~/funky/NullableDateTime/2009-1-1".Route().ShouldMapTo<FunkyController>(x => x.NullableDateTime(new DateTime(2009, 1, 1)));
		}
	}
}
