using MvcContrib.FluentHtml;

namespace MvcContrib.Samples.UI.Views
{
	/// <summary>
	/// This is a sample ModelViewPage implementation that illustrates how to add Behaviors.
	/// </summary>
	public class SampleFluentHtmlViewPage<T> : ModelViewPage<T> where T : class
	{
		public SampleFluentHtmlViewPage() : base(new MaxLengthBehavior(), new RequiredBehavior())
		{
			
		}
	}
}