using NUnit.Framework;

namespace MvcContrib.UnitTests
{
	public static class AssertionExtensions
	{
		public static void ShouldNotBeNull(this object actual)
		{
			Assert.IsNotNull(actual);
		}

		public static void ShouldEqual(this object actual, object expected)
		{
			Assert.AreEqual(expected, actual);
		}

		public static void ShouldBeTheSameAs(this object actual, object expected)
		{
			Assert.AreSame(expected, actual);
		}

        public static void ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
        }

		public static void ShouldBeNull(this object actual)
		{
			Assert.IsNull(actual);
		}

		public static void ShouldBeFalse(this bool value)
		{
			Assert.IsFalse(value);
		}

		public static void ShouldBeTrue(this bool value)
		{
			Assert.IsTrue(value);
		}

		public static void ShouldBe<T>(this object obj)
		{
			Assert.IsInstanceOf<T>(obj);
		}
	}
}