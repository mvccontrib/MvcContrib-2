using System;
using System.Collections;
using System.Linq;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

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
			var mockBehavior1 = MockRepository.GenerateMock<IBehavior<IMemberElement>>();
			var mockBehavior2 = MockRepository.GenerateMock<IBehavior<IMemberElement>>();
			target = (T)Activator.CreateInstance(typeof(T), mockBehavior1, mockBehavior2);
			target.Behaviors.ShouldCount(3);
			target.Behaviors.Where(x => x is ValidationBehavior);
			Assert.Contains(mockBehavior1, (ICollection)target.Behaviors);
			Assert.Contains(mockBehavior2, (ICollection)target.Behaviors);
		}
	}
}