using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Html;
using NUnit.Framework;

using Rhino.Mocks;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MvcContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class ModelVisualizerTester
	{
		private MockRepository _mocks;
		private HttpContextBase _context;
		private HtmlHelper _helper;
		private ViewContext _viewContext;

		private class CategoryInfo
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Key { get; set; }
		}

		private class ProductInfo
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Key { get; set; }
			public decimal Price { get; set; }
			public CategoryInfo Category { get; set; }
		}

		private enum CategoryTypes
		{
			ProductCategory,
			DiscountCategory
		}

		private void SetupHtmlHelper(object model , Dictionary<string,object> viewData)
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicHttpContextBase();
			SetupResult.For(_context.Request.FilePath).Return("Test.mvc");
			var view = _mocks.DynamicMock<IView>();
			_viewContext = new ViewContext(new ControllerContext(_context, new RouteData(), _mocks.DynamicMock<ControllerBase>()), view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
			_helper = new HtmlHelper(_viewContext, new ViewPage());

			if (model != null)
			{
				_viewContext.ViewData.Model = model;
			}
			if (viewData!=null)
			{
				foreach(string key in viewData.Keys)
				{
					_viewContext.ViewData.Add(key, viewData[key]);
				}

			}

			_mocks.ReplayAll();

		}

		[Test]
		public void ModelVisualizer_empty_when_no_viewdata()
		{
			SetupHtmlHelper( null, null);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf("There is no data in ViewData") > -1);
		}

		[Test]
		public void ModelVisualizer_select_list()
		{
			List<CategoryInfo> categoryList = new List<CategoryInfo>();
			categoryList.Add(new CategoryInfo { Id = 1, Name = "Food", Key = "FD" });
			categoryList.Add(new CategoryInfo { Id = 2, Name = "Drink", Key = "DR" });
			categoryList.Add(new CategoryInfo { Id = 3, Name = "Music", Key = "MU" });

			IEnumerable<SelectListItem> categorySelectList = 
					 from category in categoryList
					 select new SelectListItem
					 {
							 Text = category.Name,
							 Value = category.Id.ToString()
					 };

			Dictionary<string, object> viewData = new Dictionary<string, object>();
			viewData.Add("CategoriesSelectList", categorySelectList);

			SetupHtmlHelper(null, viewData);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf(@"<table border=1  ><tr><td colspan=""3"">SelectList</td></tr><tr><td>Name</td><td>Value</td><td>Selected</td></tr><tr><td>Food</td><td>1</td><td></td></tr><tr><td>Drink</td><td>2</td><td></td></tr><tr><td>Music</td><td>3</td><td></td></tr></table>") > -1);
		}

		[Test]
		public void ModelVisualizer_enum()
		{
			CategoryTypes categoryType = CategoryTypes.DiscountCategory;

			Dictionary<string, object> viewData = new Dictionary<string, object>();
			viewData.Add("CategoryType", categoryType);

			SetupHtmlHelper(null, viewData);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf(@"<table border=1  ><tr><td>CategoryType</td><td>DiscountCategory</td></tr></table>") > -1);
		}

		[Test]
		public void ModelVisualizer_generic_list()
		{
			List<CategoryInfo> categoryList = new List<CategoryInfo>();
			categoryList.Add(new CategoryInfo { Id = 1, Name = "Food", Key = "FD" });
			categoryList.Add(new CategoryInfo { Id = 2, Name = "Drink", Key = "DR" });
			categoryList.Add(new CategoryInfo { Id = 3, Name = "Music", Key = "MU" });

			Dictionary<string, object> viewData = new Dictionary<string, object>();
			viewData.Add("Categories",categoryList);

			SetupHtmlHelper(null, viewData);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf(@"<table border=1  ><tr><td><b>Id</b></td><td><b>Name</b></td><td><b>Key</b></td></tr><tr><td>1</td><td>Food</td><td>FD</td></tr><tr><td>2</td><td>Drink</td><td>DR</td></tr><tr><td>3</td><td>Music</td><td>MU</td></tr></table>") > -1);
		}

		[Test]
		public void ModelVisualizer_strongly_typed_view_with_aggregation()
		{
			CategoryInfo category = new CategoryInfo { Id = 1, Name = "Food", Key = "FD" };
			ProductInfo product = new ProductInfo { Id = 11, Name = "Fish", Key = "FS", Category = category };

			SetupHtmlHelper(product, null);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf(@"<table border=1  ><tr><td>Category</td><td><table border=""1"" ><tr><td colspan=""2"" >MvcContrib.UnitTests.UI.Html.ModelVisualizerTester+CategoryInfo</td></tr><tr><td>Id</td><td>1</td></tr><tr><td>Key</td><td>FD</td></tr><tr><td>Name</td><td>Food</td></tr></table></td></tr><tr><td>Id</td><td>11</td></tr><tr><td>Key</td><td>FS</td></tr><tr><td>Name</td><td>Fish</td></tr><tr><td>Price</td><td>0</td></tr></table>")> -1);
		}

		[Test]
		public void ModelVisualizer_strongly_typed_view_null_value()
		{
			CategoryInfo category = new CategoryInfo { Id = 1, Name =null, Key = "FD" };

			SetupHtmlHelper(category, null);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf(@"<table border=1  ><tr><td>Id</td><td>1</td></tr><tr><td>Key</td><td>FD</td></tr><tr><td>Name</td><td>null</td></tr></table>") > -1);
		}

		[Test]
		public void ModelVisualizer_strongly_typed_view_renders_table()
		{
			SetupHtmlHelper(new CategoryInfo { Id = 1, Name = "Food", Key = "FD" }, null);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf("<table border=1  ><tr><td>Id</td><td>1</td></tr><tr><td>Key</td><td>FD</td></tr><tr><td>Name</td><td>Food</td></tr></table>") > -1);
		}

		[Test]
		public void ModelVisualizer_viewdata_untyped_dictionary()
		{
			Dictionary<string,object> viewData = new Dictionary<string,object>();
			viewData.Add("Id" , 1);
			viewData.Add("Name" , "Food");
			viewData.Add("Key" , "FD");

			SetupHtmlHelper(null, viewData);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf("<table border=1  ><tr><td>Id</td><td>1</td></tr><tr><td>Key</td><td>FD</td></tr><tr><td>Name</td><td>Food</td></tr></table>") > -1);
		}

		[Test]
		public void ModelVisualizer_viewdata_untyped_dictionary_null()
		{
			Dictionary<string, object> viewData = new Dictionary<string, object>();

			viewData.Add("Name", null);


			SetupHtmlHelper(null, viewData);

			string result = _helper.ModelVisualizer();
			Assert.IsTrue(result.IndexOf("<table border=1  ><tr><td>Name</td><td>null</td></tr></table>") > -1);
		}
	}
}
