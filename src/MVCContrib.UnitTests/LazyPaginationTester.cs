using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using MvcContrib.Pagination;


namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class LazyPaginationTester
	{
		[Test]
		public void Should_create_pagination_with_default_page_size()
		{
			var strings = new List<string> {"First", "Second", "Third"};
			var pagination = strings.AsPagination(1);
			Assert.That(pagination.PageSize, Is.EqualTo(20));
		}

		[Test]
		public void Should_create_pagination_with_custom_page_size()
		{
			var strings = new List<string> {"First", "Second", "Third"};
			var pagination = strings.AsPagination(1, 5);
			Assert.That(pagination.PageSize, Is.EqualTo(5));
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Should_throw_if_page_number_is_less_than_1()
		{
			var strings = new List<string>();
			strings.AsPagination(0);
		}

		[Test]
		public void Should_execute_query()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(1, 2);
			Assert.That(pagination.TotalItems, Is.EqualTo(4));
			Assert.That(pagination.TotalPages, Is.EqualTo(2));
			Assert.That(pagination.PageNumber, Is.EqualTo(1));
			Assert.That(pagination.Count(), Is.EqualTo(2)); 
		}

		[Test]
		public void FirstItem_should_return_index_of_first_item_in_current_page()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(1, 2);
			Assert.That(pagination.FirstItem, Is.EqualTo(1));
		}

		[Test]
		public void LastItem_should_return_index_of_last_item_in_current_page()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(1, 2);
			Assert.That(pagination.LastItem, Is.EqualTo(2));
		}

		[Test]
		public void HasPreviousPage_should_return_true_when_not_on_first_page()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(2, 2);
			Assert.That(pagination.HasPreviousPage);
		}

		[Test]
		public void HasPreviousPage_should_return_false_when_on_first_page()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(1, 2);
			Assert.That(pagination.HasPreviousPage, Is.False);
		}

		[Test]
		public void HasNextPage_should_return_true_when_on_first_page()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(1, 2);
			Assert.That(pagination.HasNextPage);
		}

		[Test]
		public void HasNextPage_should_return_false_when_on_last_page()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(4, 2);
			Assert.That(pagination.HasNextPage, Is.False);
		}

		[Test]
		public void For_Coverage()
		{
			var strings = new List<string> { "First", "Second", "Third", "Fourth" };
			var pagination = strings.AsPagination(4, 2);
			((IEnumerable)pagination).GetEnumerator();
		}
	}
}