using System;
using System.IO;
using System.Text;
using System.Web;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI
{
	[TestFixture]
	public class BlockRendererTester
	{
		[TestFixture]
		public class With_Null_Action
		{
			[Test]
			public void Then_Returns_EmptyString()
			{
				var mocks = new MockRepository();
				var context = mocks.DynamicMock<HttpContextBase>();
				
				using(mocks.Record())
				{
				}
				
				using(mocks.Playback())
				{
					string text = new BlockRenderer(context).Capture(null);
					Assert.That(text, Is.EqualTo(string.Empty));
				}
			}
		}

		[TestFixture]
		public class With_Action
		{

			[Test]
			public void Then_Response_Flush_Is_Called_Before_And_After_The_Action()
			{
				var mocks = new MockRepository();
				var context = mocks.DynamicMock<HttpContextBase>();
				var response = mocks.DynamicMock<HttpResponseBase>();
				var action = mocks.DynamicMock<Action>();
				
				using(mocks.Record())
				{
					SetupResult.For(context.Response).Return(response);
					SetupResult.For(response.ContentEncoding).Return(Encoding.ASCII);
					using(mocks.Ordered())
					{
						response.Flush();
						action();
						response.Flush();
					}
				}
				using(mocks.Playback())
				{
					new BlockRenderer(context).Capture(action);
				}
			}
		}

		[TestFixture]
		public class With_Existing_Filter
		{
			[Test]
			public void When_Capture_Then_Original_Filter_Is_Switched_And_PutBack()
			{
				var mocks = new MockRepository();
				var context = mocks.DynamicMock<HttpContextBase>();
				var response = mocks.DynamicMock<HttpResponseBase>();
				var origFilter = mocks.DynamicMock<Stream>();
				
				var action = mocks.DynamicMock<Action>();

				using (mocks.Record())
				{
					SetupResult.For(context.Response).Return(response);
					SetupResult.For(response.ContentEncoding).Return(Encoding.ASCII);
					
					Expect.Call(response.Filter).Return(origFilter);
					
					response.Filter = null;
					LastCall.IgnoreArguments().Constraints(Rhino.Mocks.Constraints.Is.TypeOf(typeof(CapturingResponseFilter)));
					
					response.Filter = origFilter;
				}
				using (mocks.Playback())
				{
					new BlockRenderer(context).Capture(action);
				}
			}

			[Test]
			public void When_Capture_With_Error_Then_Original_Filter_Is_PutBack()
			{
				var mocks = new MockRepository();
				var context = mocks.DynamicMock<HttpContextBase>();
				var response = mocks.DynamicMock<HttpResponseBase>();
				var origFilter = mocks.DynamicMock<Stream>();
				Action action = delegate { throw new InvalidOperationException(); };

				using (mocks.Record())
				{
					SetupResult.For(context.Response).Return(response);
					SetupResult.For(response.ContentEncoding).Return(Encoding.ASCII);
					
					Expect.Call(response.Filter).Return(origFilter);
					
					response.Filter = null;
					LastCall.IgnoreArguments();
					
					response.Filter = origFilter;
				}
				using (mocks.Playback())
				{
					try
					{
						new BlockRenderer(context).Capture(action);
					}
					catch(InvalidOperationException)
					{
						// Swallow
					}
					
				}
			}
		}
	}
}
