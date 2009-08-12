using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.MenuBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.MenuBuilder
{
	[TestFixture]
	public class MenuBuilderTests
	{
		[SetUp]
		public void Setup()
		{
			writer = new StringWriter();

			RouteTable.Routes.Clear();
			RouteTable.Routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = "" }
			);

			var requestContext = GetRequestContext();
			var controller = new HomeController();
			controllerContext = new ControllerContext(requestContext, controller);
		}

		[TearDown]
		public void Teardown()
		{
			RouteTable.Routes.Clear(); //To prevent other tests from failing.
		}

		private RequestContext GetRequestContext()
		{
			HttpContextBase httpcontext = GetHttpContext(@"Home/Index", null, null);
			RouteData rd = new RouteData();
			rd.Values.Add("controller", "Home");
			rd.Values.Add("action", "Index");
			return new RequestContext(httpcontext, rd);
		}


		private HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod)
		{
			var mockContext = MockRepository.GenerateMock<HttpContextBase>();
			var mockRequest = MockRepository.GenerateMock<HttpRequestBase>();
			if (!String.IsNullOrEmpty(appPath))
			{
				mockRequest.Stub(o => o.ApplicationPath).Return(appPath);
			}
			if (!String.IsNullOrEmpty(requestPath))
			{
				mockRequest.Stub(o => o.AppRelativeCurrentExecutionFilePath).Return(requestPath);
			}

			Uri uri = new Uri("http://localhost/");
			mockRequest.Stub(o => o.Url).Return(uri);

			mockRequest.Stub(o => o.PathInfo).Return(String.Empty);
			if (!string.IsNullOrEmpty(httpMethod))
			{
				mockRequest.Stub(o => o.HttpMethod).Return(httpMethod);
			}
			mockContext.Stub(o => o.Request).Return(mockRequest);
			mockContext.Stub(o => o.Session).Return((HttpSessionStateBase)null);

			var mockResponse = MockRepository.GenerateMock<HttpResponseBase>();
			mockResponse.Stub(o => o.ApplyAppPathModifier(null)).IgnoreArguments().Do((Func<string, string>)(s => s));

			//				IgnoreArguments().Return(@"Home\Index");

			mockContext.Stub(o => o.Response).Return(mockResponse);

			mockRequest.Stub(o => o.Path).Return("Home/Index/");

			var principal = MockRepository.GenerateMock<IPrincipal>();
			mockContext.Stub(o => o.User).Return(principal);
			identity = new TestIdentity() { IsAuthenticated = false };
			principal.Stub(o => o.Identity).Return(identity);
			return mockContext;
		}

		/// <summary>
		/// I'm sure there's some magic mocking-foo way of doing this to change the return value of IsAuthenticated mid test, but I'm just
		/// a 2nd level half-dwarf when it comes to such things....
		/// </summary>
		private class TestIdentity : IIdentity
		{
			public string Name
			{
				get { return "Bob"; }
			}

			public string AuthenticationType
			{
				get { return "Manual"; }
			}

			public bool IsAuthenticated { get; set; }
		}


		private TestIdentity identity;
		private TextWriter writer;
		private ControllerContext controllerContext;
		private string Out { get { return writer.ToString(); } }

		[Test]
		public void MenuLinkPrintsCorrectTag()
		{
			MenuItem link = Menu.Link("Url", "Title");
			link.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Url\">Title</a></li>", Out);
		}

		[Test]
		public void MenuLinkIconPrintsCorrectTag()
		{
			MenuItem link = Menu.Link("Url", "Title", "Icon");
			link.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Url\"><img alt=\"Title\" border=\"0\" src=\"Icon\" />Title</a></li>", Out);
		}

		[Test]
		public void MenuLinkIconWithNoTitlePrintsCorrectTag()
		{
			MenuItem link = Menu.Link("Url", null, "Icon");
			link.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Url\"><img border=\"0\" src=\"Icon\" /></a></li>", Out);
		}


		[Test]
		public void MenuItem_Fluent_Tests()
		{
			MenuItem item = new MenuItem();
			item.SetTitle("Title").SetIcon("Icon").SetHelpText("Help").SetActionUrl("Action").SetAnchorClass("AnchorClass").
				SetIconClass("IconClass").SetItemClass("ItemClass");

			Assert.AreEqual("Title", item.Title);
			Assert.AreEqual("Icon", item.Icon);
			Assert.AreEqual("Help", item.HelpText);
			Assert.AreEqual("Action", item.ActionUrl);
			Assert.AreEqual("AnchorClass", item.AnchorClass);
			Assert.AreEqual("IconClass", item.IconClass);
			Assert.AreEqual("ItemClass", item.ItemClass);

			MenuList list = new MenuList();
			list.SetListClass("ListClass");
			Assert.AreEqual("ListClass", list.ListClass);

			ActionMenuItem<HomeController> ai = new ActionMenuItem<HomeController>();
			ai.SetMenuAction(p => p.Index());
			Assert.AreEqual("p.Index()", ai.MenuAction.Body.ToString());

		}

		[Test]
		public void ActionMenuItemRendersLink()
		{
			MenuItem action = Menu.Action<HomeController>(p => p.Index());
			action.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Home/Index/\">Index</a></li>", Out);
		}


		[Test]
		public void ActionMenuItemRendersLinkWithIcon()
		{
			MenuItem action = Menu.Action<HomeController>(p => p.Index(), "Title", "Icon");
			action.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Home/Index/\"><img alt=\"Title\" border=\"0\" src=\"Icon\" />Title</a></li>", Out);
		}

		[Test]
		public void ActionMenuItemRendersLinkWithIconAndNoTitle()
		{
			MenuItem action = Menu.Action<HomeController>(p => p.Index(), null, "Icon");
			action.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Home/Index/\"><img border=\"0\" src=\"Icon\" /></a></li>", Out);
		}

		[Test]
		public void MenuTitleAndMenuHelpAttributesSetValues()
		{
			MenuItem action = Menu.Action<HomeController>(p => p.Index2());
			action.Prepare(controllerContext);
			Assert.AreEqual("Title", action.Title);
			Assert.AreEqual("Help Text", action.HelpText);
		}

		[Test]
		public void SecureMenuItemDisplaysNothingWhenNotAuthenticated()
		{
			MenuItem action = Menu.Secure<HomeController>(p => p.Index());
			action.RenderHtml(controllerContext, writer);
			Assert.AreEqual("", Out);
			identity.IsAuthenticated = true;
			action.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Home/Index/\">Index</a></li>", Out);
		}

		[Test]
		public void CanCreateMenuLists()
		{
			MenuList list = Menu.Items("ListItems",
				Menu.Link("Url1", "Title1"),
				Menu.Link("Url2", "Title2"));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("Title1", list[0].Title);
			Assert.AreEqual("Title2", list[1].Title);

			list.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a>ListItems</a><ul><li><a href=\"Url1\">Title1</a></li><li><a href=\"Url2\">Title2</a></li></ul></li>", Out);
		}

		[Test]
		public void CanCreateMenuListsWithIcon()
		{
			MenuList list = Menu.Items("ListItems", "Icon",
				Menu.Link("Url1", "Title1"),
				Menu.Link("Url2", "Title2"));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("Title1", list[0].Title);
			Assert.AreEqual("Title2", list[1].Title);

			list.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a><img alt=\"ListItems\" border=\"0\" src=\"Icon\" />ListItems</a><ul><li><a href=\"Url1\">Title1</a></li><li><a href=\"Url2\">Title2</a></li></ul></li>", Out);
		}

		[Test]
		public void CanCreateMenuListsWithIconOnly()
		{
			MenuList list = Menu.Items(null, "Icon",
				Menu.Link("Url1", "Title1"),
				Menu.Link("Url2", "Title2"));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("Title1", list[0].Title);
			Assert.AreEqual("Title2", list[1].Title);

			list.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a><img border=\"0\" src=\"Icon\" /></a><ul><li><a href=\"Url1\">Title1</a></li><li><a href=\"Url2\">Title2</a></li></ul></li>", Out);
		}

		[Test]
		public void SecureItemsAreRemovedFromMenuList()
		{
			MenuList list = Menu.Items("ListItems",
									   Menu.Link("Url1", "Title1"),
									   Menu.Link("Url2", "Title2"),
									   Menu.Secure<HomeController>(p => p.Index()));
			list.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a>ListItems</a><ul><li><a href=\"Url1\">Title1</a></li><li><a href=\"Url2\">Title2</a></li></ul></li>", Out);

		}

		[Test]
		public void ListsWithSingleItemsCollapsedIntoNonList()
		{
			MenuList list = Menu.Items("ListItems",
									   Menu.Link("Url1", "Title1"),
									   Menu.Secure<HomeController>(p => p.Index()));
			list.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Url1\">Title1</a></li>", Out);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void CallingRenderOnListWithoutPrepareThrows()
		{
			MenuList list = Menu.Items("ListItems",
									   Menu.Link("Url1", "Title1"),
									   Menu.Secure<HomeController>(p => p.Index()));
			string html = list.RenderHtml();
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void CallingRenderOnItemWithoutPrepareThrows()
		{
			MenuItem link = Menu.Link("Url1", "Title1");
			string html = link.RenderHtml();
		}

		[Test]
		public void TestICollectionMembersOnMenuList()
		{
			MenuList list = new MenuList();
			var item = new MenuItem();
			list.Add(item);
			Assert.IsTrue(list.Contains(item));
			list.Remove(item);
			Assert.IsFalse(list.Contains(item));
			list.Add(item);
			Assert.AreEqual(1, list.Count);
			list.Clear();
			Assert.AreEqual(0, list.Count);
			MenuItem[] array = new MenuItem[1];
			list.Add(item);
			list.CopyTo(array, 0);
			Assert.AreEqual(item, array[0]);
			Assert.IsFalse(list.IsReadOnly);
			foreach (var menuItem in list)
			{
				Assert.AreEqual(item, menuItem);
			}
		}

		[Test]
		public void RenderingListWithNoItemsIsEmpty()
		{
			MenuList list = new MenuList();
			list.RenderHtml(controllerContext, writer);
			Assert.AreEqual("", Out);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void CannotSecureActionMenuItemWithNoAction()
		{
			var item = new SecureActionMenuItem<HomeController>();
			item.Prepare(controllerContext);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void CannotActionMenuItemWithNoAction()
		{
			var item = new ActionMenuItem<HomeController>();
			item.Prepare(controllerContext);
		}

		[Test]
		public void CanAssignTitleViaFluentMenu()
		{
			var item = Menu.Action<HomeController>(p => p.Index(), "A Title");
			var secureItem = Menu.Secure<HomeController>(p => p.Index2(), "A Different Title");
			Assert.AreEqual("A Title", item.Title);
			Assert.AreEqual("A Different Title", secureItem.Title);
		}

		[Test]
		public void MenuBeginCreatesMenuWithNoTitle()
		{
			var item = Menu.Begin(
				Menu.Link("Url1", "Title1"),
				Menu.Link("Url2", "Title2"));
			Assert.AreEqual(null, item.Title);
			item.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<ul><li><a href=\"Url1\">Title1</a></li><li><a href=\"Url2\">Title2</a></li></ul>", Out);
		}

		[Test]
		public void CanSetDefaultIconDirectory()
		{
			Menu.DefaultIconDirectory = "/content/";
			var item = Menu.Link("Url", "Title", "Icon.jpg");
			item.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a href=\"Url\"><img alt=\"Title\" border=\"0\" src=\"/content/Icon.jpg\" />Title</a></li>", Out);
		}

		[Test]
		public void DisabledItemsRenderWithDisabledClass()
		{
			MenuItem secure = Menu.Secure<HomeController>(p => p.Index());
			secure.SetShowWhenDisabled(true).SetDisabledMenuItemClass("DisabledClass");
			secure.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a class=\"DisabledClass\">Index</a></li>", Out);
		}

		[Test]
		public void SelectedItemsShouldRenderAsSuch()
		{
			MenuItem link = Menu.Action<HomeController>(p => p.Index());
			link.SetSelectedClass("selected");
			link.RenderHtml(controllerContext, writer);
			Assert.AreEqual("<li><a class=\"selected\" href=\"Home/Index/\">Index</a></li>", Out);

		}

		public class HomeController : Controller
		{
			[Authorize]
			public ActionResult Index()
			{
				return null;
			}

			[MenuTitle("Title")]
			[MenuHelpText("Help Text")]
			public ActionResult Index2()
			{
				return null;
			}

			public int NonMethodCall { get; set; }
		}


	}
}
