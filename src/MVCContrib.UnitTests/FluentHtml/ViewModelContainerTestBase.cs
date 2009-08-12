using System;
using System.Collections;
using MvcContrib.FluentHtml;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Behaviors;
using Rhino.Mocks;
using System.Linq;

namespace MvcContrib.UnitTests.FluentHtml
{
	public class ViewModelContainerTestBase<T, TModel> where T : IViewModelContainer<TModel> where TModel : class
	{
		protected T target;

		[SetUp]
		public void SetUp()
		{
			target = (T)Activator.CreateInstance(typeof(T));
		}

		[Test]
		public virtual void can_get_html_behaviors()
		{
			var mockBehavior1 = MockRepository.GenerateMock<IMemberBehavior>();
			var mockBehavior2 = MockRepository.GenerateMock<IMemberBehavior>();
			target = (T)Activator.CreateInstance(typeof(T), mockBehavior1, mockBehavior2);
			target.Behaviors.ShouldCount(3);
			target.Behaviors.Where(x => x is ValidationBehavior);
			Assert.Contains(mockBehavior1, (ICollection)target.Behaviors);
			Assert.Contains(mockBehavior2, (ICollection)target.Behaviors);
		}
	}
}