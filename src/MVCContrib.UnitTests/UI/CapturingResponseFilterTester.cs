using System.IO;
using System.Text;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI
{
	[TestFixture]
	public class CapturingResponseFilterTester
	{
		[TestFixture]
		public class When_Writing
		{
			[Test]
			public void Then_GetContents_Returns_The_Contents()
			{
				string text = null;
				using(var sink = new MemoryStream())
				using(var filter = new CapturingResponseFilter(sink))
				{
					{
						byte[] bytes = Encoding.UTF8.GetBytes("Goose");
						filter.Write(bytes, 0, bytes.Length);
						text = filter.GetContents(Encoding.UTF8);
					}
					Assert.That(text, Is.EqualTo("Goose"));
				}
			}
		}

		[TestFixture]
		public class With_Sink
		{
			private MockRepository _mocks;
			private CapturingResponseFilter _filter;

			[SetUp]
			public void Setup()
			{
				_mocks = new MockRepository();
			}

			[Test]
			public void When_Flush_Then_Sink_Is_Flushed()
			{
				var sink = _mocks.DynamicMock<Stream>();
				
				using (_mocks.Record())
				{
					sink.Flush();
				}
				using (_mocks.Playback())
				{
					using (_filter = new CapturingResponseFilter(sink))
						_filter.Flush();
				}
			}

			[Test]
			public void When_Closed_Then_Sink_Is_Closed()
			{
				var sink = _mocks.DynamicMock<Stream>();

				using (_mocks.Record())
				{
					sink.Close();
				}
				using (_mocks.Playback())
				{
					using (_filter = new CapturingResponseFilter(sink))
						_filter.Flush();
				}
			}

			[Test]
			public void When_SetLength_Then_Sink_SetLength()
			{
				var sink = _mocks.DynamicMock<Stream>();

				using (_mocks.Record())
				{
					sink.SetLength(100);
				}
				using (_mocks.Playback())
				{
					using (_filter = new CapturingResponseFilter(sink))
						_filter.SetLength(100);
				}
			}

			[Test]
			public void When_Read_Then_Sink_Is_Read()
			{
				var sink = _mocks.DynamicMock<Stream>();
				var buf = new byte[100];
				using (_mocks.Record())
				{
					Expect.Call(sink.Read(buf, 0, 20)).Return(0);
				}
				using (_mocks.Playback())
				{
					using (_filter = new CapturingResponseFilter(sink))
						_filter.Read(buf, 0, 20);
				}
			}
		}

		[TestFixture]
		public class All_Properties
		{
			[Test]
			public void When_CanRead_Then_True()
			{
				using(var filter = new CapturingResponseFilter(new MemoryStream()))
				{
					Assert.That(filter.CanRead, Is.True);
				}
			}

			[Test]
			public void When_CanSeek_Then_False()
			{
				using (var filter = new CapturingResponseFilter(new MemoryStream()))
				{
					Assert.That(filter.CanSeek, Is.False);
				}
			}

			[Test]
			public void When_CanWrite_Then_False()
			{
				using (var filter = new CapturingResponseFilter(new MemoryStream()))
				{
					Assert.That(filter.CanWrite, Is.False);
				}
			}

			[Test]
			public void When_Get_Length_Then_0()
			{
				using (var filter = new CapturingResponseFilter(new MemoryStream()))
				{
					Assert.That(filter.Length, Is.EqualTo(0));
				}
			}

			[Test]
			public void When_Set_Position_Then_Get_Position()
			{
				using (var filter = new CapturingResponseFilter(new MemoryStream()))
				{
					filter.Position = 100;
					Assert.That(filter.Position, Is.EqualTo(100));
				}
			}

			[Test]
			public void When_Seek_Then_0()
			{
				using (var filter = new CapturingResponseFilter(new MemoryStream()))
				{
					Assert.That(filter.Seek(0, SeekOrigin.Current), Is.EqualTo(0));
				}
			}
		}
	}
}
