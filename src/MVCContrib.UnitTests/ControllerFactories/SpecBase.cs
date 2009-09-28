using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ControllerFactories
{
	public abstract class SpecBase
	{
		protected MockRepository _mocks;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			BeforeEachSpec();
		}

		protected IDisposable Record()
		{
			return _mocks.Record();
		}

		protected IDisposable Playback()
		{
			return _mocks.Playback();
		}

		[TearDown]
		public void Teardown()
		{
			AfterEachSpec();
		}

		protected abstract void BeforeEachSpec();
		protected abstract void AfterEachSpec();
	}
}