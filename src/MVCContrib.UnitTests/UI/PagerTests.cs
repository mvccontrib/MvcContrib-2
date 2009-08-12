using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using MvcContrib.UI.Pager;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Pager
{
	[TestFixture]
	public class PagerTests
	{
		private List<object> _datasource;
		private HttpContextBase _context;

		[SetUp]
		public void Setup()
		{
			_datasource = new List<object> {new object(), new object(), new object()};
			_context = MvcMockHelpers.DynamicHttpContextBase();
			_context.Request.Stub(x => x.FilePath).Return("Test.mvc");
		}

		[Test]
		public void Should_render_with_pagination_last_and_next()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=2\">last</a></span></div>";
			RenderPager(1, 2).ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_with_pagination_first_and_previous()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1\">first</a> | <a href=\"Test.mvc?page=1\">prev</a> | next | last</span></div>";
			RenderPager(2, 2).ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_pagination_with_querystring()
		{
			_context.Request.QueryString.Add("a", "b");
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1&amp;a=b\">first</a> | <a href=\"Test.mvc?page=1&amp;a=b\">prev</a> | next | last</span></div>";
			RenderPager(2, 2).ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_pagination_with_different_message_if_pagesize_is_1()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 1 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			RenderPager(1, 1).ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_not_render_pager_links_if_there_is_only_1_page()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 1 - 3 of 3 </span></div>";

			RenderPager(1, 3).ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_not_render_pagination_when_datasource_is_empty()
		{
			_datasource.Clear();
			RenderPager(1, 1).ToString().ShouldBeNull();
		}

		[Test]
		public void Should_render_localized_pagination()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Visar 1 - 2 av 3 </span><span class='paginationRight'>första | föregående | <a href=\"Test.mvc?page=2\">nästa</a> | <a href=\"Test.mvc?page=2\">sista</a></span></div>";
			RenderPager(1, 2)
				.Format("Visar {0} - {1} av {2} ")
				.First("första")
				.Previous("föregående")
				.Next("nästa")
				.Last("sista")
				.ToString()
				.ShouldEqual(expected);
		}

		[Test]
		public void Should_render_localized_pagination_with_different_message_if_pagesize_is_1()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Visar 1 av 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			RenderPager(1, 1).SingleFormat("Visar {0} av {1} ").ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_pagination_with_custom_page_name()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?my_page=2\">next</a> | <a href=\"Test.mvc?my_page=2\">last</a></span></div>";
			RenderPager(1, 2).QueryParam("my_page").ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_create_pager_from_datasource()
		{
			var helper = new HtmlHelper(new ViewContext() { HttpContext = _context }, MockRepository.GenerateStub<IViewDataContainer>());
			helper
				.Pager(_datasource.AsPagination(1, 2))
				.ToString()
				.Contains("Showing 1 - 2 of 3").ShouldBeTrue();
		}


		[Test]
		public void Should_create_pager_from_viewdata()
		{
			var helper = new HtmlHelper(
				new ViewContext
				{
					HttpContext =  _context,
					ViewData = new ViewDataDictionary {{"data", _datasource.AsPagination(1, 2)}}
				},
				MockRepository.GenerateStub<IViewDataContainer>()
				);

			helper.Pager("data")
				.ToString()
				.Contains("Showing 1 - 2 of 3").ShouldBeTrue();
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_when_item_in_viewdata_is_not_ipagination()
		{
			var helper = new HtmlHelper(
				new ViewContext
				{
					ViewData = new ViewDataDictionary {{"data", new object()}}
				},
				MockRepository.GenerateStub<IViewDataContainer>()
				);

			helper.Pager("data");
		}

		[Test]
		public void Should_encode_additional_querystring_input()
		{
			_context.Request.QueryString.Add("foo", "<bar>");
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1&amp;foo=&lt;bar&gt;\">first</a> | <a href=\"Test.mvc?page=1&amp;foo=&lt;bar&gt;\">prev</a> | next | last</span></div>";
			RenderPager(2, 2).ToString().ShouldEqual(expected);
		}

		[Test]
		public void Should_use_Custom_url()
		{
			string expected =
				"<div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"TEST/2\">next</a> | <a href=\"TEST/2\">last</a></span></div>";
			RenderPager(1, 2).Link(p => "TEST/" + p).ToString().ShouldEqual(expected);
		}

		private MvcContrib.UI.Pager.Pager RenderPager(int pageNumber, int pageSize)
		{
			return new MvcContrib.UI.Pager.Pager(_datasource.AsPagination(pageNumber, pageSize), _context.Request);
		}
	}

	


}