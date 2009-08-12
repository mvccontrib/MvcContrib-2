using System.Linq;
using NUnit.Framework;
using MvcContrib.Pagination;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class CustomPaginationTester
	{
		private IPagination<string> _pagination;

		[SetUp]
		public void Setup()
		{
			_pagination = new CustomPagination<string>(new[] { "First", "Second" }, 1, 2, 4);
		}

		[Test]
		public void Should_store_total_items()
		{
			_pagination.TotalItems.ShouldEqual(4);
		}

		[Test]
		public void Should_store_page_number()
		{
			_pagination.PageNumber.ShouldEqual(1);
		}

		[Test]
		public void Should_store_page_size()
		{
			_pagination.PageSize.ShouldEqual(2);
		}

		[Test]
		public void Should_enumerabe_over_items()
		{
			_pagination.Count().ShouldEqual(2);
			_pagination.First().ShouldEqual("First");
			_pagination.Last().ShouldEqual("Second");
		}

		[Test]
		public void Should_calculate_total_pages()
		{
			_pagination.TotalPages.ShouldEqual(2);
		}

		[Test]
		public void FirstItem_should_return_index_of_first_item_in_current_page()
		{
			_pagination.FirstItem.ShouldEqual(1);
		}

		[Test]
		public void LastItem_should_return_index_of_last_item_in_current_page()
		{
			_pagination.LastItem.ShouldEqual(2);
		}

		[Test]
		public void HasPreviousPage_should_return_true_when_not_on_first_page()
		{
			_pagination = new CustomPagination<string>(new[] { "Third", "Fourth" }, 2, 2, 4);
			_pagination.HasPreviousPage.ShouldBeTrue();
		}

		[Test]
		public void HasPreviousPage_should_return_false_when_on_first_page()
		{
			_pagination.HasPreviousPage.ShouldBeFalse();
		}

		[Test]
		public void HasNextPage_should_return_true_when_on_first_page()
		{
			_pagination.HasNextPage.ShouldBeTrue();
		}

		[Test]
		public void HasNextPage_should_return_false_when_on_last_page()
		{
			_pagination = new CustomPagination<string>(new[] { "Third", "Fourth" }, 2, 2, 4);
			_pagination.HasNextPage.ShouldBeFalse();
		}
	}
}