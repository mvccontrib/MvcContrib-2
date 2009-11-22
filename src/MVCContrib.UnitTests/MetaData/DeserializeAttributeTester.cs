using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Attributes;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Specialized;

namespace MvcContrib.UnitTests.MetaData
{
	[Obsolete("Consider using System.Web.Mvc.DefaultModelBinder instead.")]
	[TestFixture]
	public class DeserializeAttributeTester
	{
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			var context = MvcMockHelpers.DynamicHttpContextBase();

			context.Request.QueryString["ids[0]"] = "1";
			context.Request.QueryString["dupe[0]"] = "1";

			context.Request.Form["ids[1]"] = "2";
			context.Request.Form["dupe[0]"] = "2";

			context.Request.Cookies.Add(new HttpCookie("ids[2]", "3"));
			context.Request.Cookies.Add(new HttpCookie("dupe[0]", "3"));

			context.Request.ServerVariables["ids[3]"] = "4";
			context.Request.ServerVariables["dupe[0]"] = "4";

			var controller = MockRepository.GenerateStub<ControllerBase>();
			controller.TempData = new TempDataDictionary();
			controller.TempData["ids[4]"] = 5;
			controller.TempData["dupe[0]"] = 5;

			var routeData = new RouteData();
			routeData.Values.Add("ids[5]", 6);
			routeData.Values.Add("dupe[0]", 6);

			var requestContext = new RequestContext(context, routeData);
			_controllerContext = new ControllerContext(requestContext, controller);
		}

		private ModelBindingContext CreateContext(Type type)
		{
			return new ModelBindingContext() { ModelMetadata = new ModelMetadata(new EmptyModelMetadataProvider(), null, null, type, null)};
		}

		[Test]
		public void CanCreateAttribute()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Params);
		}

		[Test]
		public void CanDeserializeFromQueryString()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.QueryString);

			var ids = (int[])attr.BindModel(_controllerContext, CreateContext(typeof(int[])));
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(1, ids[0]);
		}

		[Test]
		public void CanDeserializeFromForm()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Form);

			var ids = (int[])attr.BindModel(_controllerContext, CreateContext(typeof(int[])));
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(2, ids[0]);
		}

		[Test]
		public void CanDeserializeFromCookies()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Cookies);

			var ids = (int[])attr.BindModel(_controllerContext, CreateContext(typeof(int[])));
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(3, ids[0]);
		}

		[Test]
		public void CanDeserializeFromServerVariables()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.ServerVariables);

			var ids = (int[])attr.BindModel(_controllerContext, CreateContext(typeof(int[])));	
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(4, ids[0]);
		}

		[Test]
		public void CanDeserializeFromParams()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Params);

			var ids = (int[])attr.BindModel(_controllerContext, CreateContext(typeof(int[])));
			Assert.IsNotNull(ids);
			Assert.AreEqual(4, ids.Length);
		}

		[Test]
		public void CanDeserializeFromTempData()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.TempData);

			var ids = (int[])attr.BindModel(_controllerContext, CreateContext(typeof(int[])));	
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(5, ids[0]);
		}

		[Test]
		public void CanDeserializeFromRouteData()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.RouteData);

			var ids = (int[])attr.BindModel( _controllerContext,CreateContext(typeof(int[])));
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(6, ids[0]);
		}

		[Test]
		public void CanDeserializeFromAll()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.All);

			var ids = (int[])attr.BindModel(_controllerContext,CreateContext(typeof(int[])));
			Assert.IsNotNull(ids);
			Assert.AreEqual(6, ids.Length);
		}

		[Test]
		public void Duplicates_Create_CSV_In_QString_Form_Cookies_SvrVars_TempData_RouteData_Order()
		{
			var attr = new DeserializeAttribute("dupe", RequestStore.All);

			var dupe = (string[])attr.BindModel(_controllerContext,CreateContext(typeof(string[])));
			Assert.IsNotNull(dupe);
			Assert.AreEqual("1,2,3,4,5,6", dupe[0]);
		}

		[Test]
    public void GetBinder_ReturnsInstanceOfDeserializeAttribute()
    {
      var binder = new DeserializeAttribute("ids", RequestStore.All);
      var modelBinder = binder.GetBinder();
      Assert.IsNotNull(modelBinder);
      Assert.AreEqual(modelBinder, binder);
    }
	}
}