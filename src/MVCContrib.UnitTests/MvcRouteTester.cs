using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Routing;
using NUnit.Framework;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class MvcRouteTester
	{
		[Test]
		public void Can_create_mvc_route_with_default_controller_action_and_additional_defaults()
		{
			var route = MvcRoute
				.MappUrl("test/{controller}/{action}/{id}")
				.ToDefaultAction<MvcRouteController>(x => x.Index(), new {id = "def"});

			Assert.AreEqual("MvcRoute", route.Defaults["controller"]);
			Assert.AreEqual("Index", route.Defaults["action"]);
			Assert.AreEqual("def", route.Defaults["id"]);
		}

		[Test]
		public void Can_create_mvc_route_with_default_controller_action_and_parameter_defaults()
		{
			var route = MvcRoute
				.MappUrl("test/{controller}/{action}")
				.ToDefaultAction<MvcRouteController>(x => x.WithTwoArgs("mupp", null));

			Assert.AreEqual("MvcRoute", route.Defaults["controller"]);
			Assert.AreEqual("WithTwoArgs", route.Defaults["action"]);
			Assert.AreEqual("mupp", route.Defaults["arg1"]);
			Assert.IsFalse(route.Defaults.ContainsKey("arg2"));
		}

		[Test]
		public void Can_create_mvc_route_with_constraints()
		{
			var route = MvcRoute
				.MappUrl("test/{controller}/{action}")
				.WithConstraints(new {action = "^[a-Z]+$"})
				.ToDefaultAction<MvcRouteController>(x => x.WithTwoArgs("mupp", null));

			Assert.AreEqual("^[a-Z]+$", route.Constraints["action"]);
		}

		[Test]
		public void Can_create_mvc_route_with_namespaces()
		{
			var namespaces = new[] {"Namespace.One", "Namespace.Two"};

			var route = MvcRoute
				.MappUrl("test/{controller}/{action}")
				.WithNamespaces(namespaces)
				.ToDefaultAction<MvcRouteController>(x => x.WithTwoArgs("mupp", null));

			Assert.AreEqual(namespaces, route.DataTokens["Namespaces"]);
		}

		[Test]
		public void Can_create_mvc_route_and_add_to_route_collection()
		{
			var routes = new RouteCollection();

			MvcRoute
				.MappUrl("test/{controller}/{action}")
				.ToDefaultAction<MvcRouteController>(x => x.WithTwoArgs("mupp", null))
				.AddWithName("TestName", routes);

			Assert.IsNotNull(routes["TestName"]);
		}

		[Test]
		public void Can_create_mvc_route_to_method_with_actionName()
		{
			var route = MvcRoute
				.MappUrl("test/{controller}/{action}")
				.ToDefaultAction<MvcRouteController>(x => x.WithActionNameAttribute());


			Assert.AreEqual("ChangedName", route.Defaults["action"]);
		}

		[Test]
		public void Can_create_mvc_route_to_method_with_string_defaults()
		{
			var route = MvcRoute
				.MappUrl("test/{controller}/{action}")
				.WithDefaults(new {controller = "home", action = "index"});

			Assert.AreEqual("home", route.Defaults["controller"]);
			Assert.AreEqual("index", route.Defaults["action"]);
		}


		public class MvcRouteController : Controller
		{
			public ActionResult Index()
			{
				return null;
			}

			public ActionResult WithTwoArgs(string arg1, string arg2)
			{
				return null;
			}

			[ActionName("ChangedName")]
			public ActionResult WithActionNameAttribute()
			{
				return null;
			}
		}
	}
}