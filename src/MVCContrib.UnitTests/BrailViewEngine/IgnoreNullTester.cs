using MvcContrib.BrailViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.BrailViewEngine
{

	[TestFixture]
	[Category("BrailViewEngine")]
	public class IgnoreNullTester
	{
		[SetUp]
		public void SetUp()
		{
		}

		[Test]
		public void Returns_Itself_For_Null_Targets()
		{
			var ignore = new IgnoreNull(null);

			Assert.AreEqual(ignore, ignore.QuackGet(null, null));
			Assert.AreEqual(ignore, ignore.QuackSet(null, null, null));
			Assert.AreEqual(ignore, ignore.QuackInvoke(null, null));
		}

		[Test]
		public void Wraps_Result_In_IgnoreNull()
		{
			var ignore = new IgnoreNull(new Duck());

			Assert.IsInstanceOf<IgnoreNull>(ignore.QuackGet("Name", null));
			Assert.IsInstanceOf<IgnoreNull>(ignore.QuackSet("Name", null, "Donald"));
			Assert.IsInstanceOf<IgnoreNull>(ignore.QuackInvoke("ToString", null));

			Assert.AreEqual(true, ignore.QuackGet("_IsIgnoreNullReferencingNotNullObject_", null));
		}

		[Test]
		public void Can_Get_With_Parameters()
		{
			var duck = new Duck {Name = "Donald"};
		    var ignore = new IgnoreNull(duck);

			Assert.AreEqual(duck.Name, ignore.QuackGet("Item", new object[1] {1}).ToString());
		}

		[Test]
		public void Can_Set_With_Parameters()
		{
			var duck = new Duck();
			var ignore = new IgnoreNull(duck);

			ignore.QuackSet("Item", new object[1] {1}, "Donald");

			Assert.AreEqual("Donald", duck.Name);
		}

		[Test]
		public void ToString_Returns_Empty_String_For_Null_Target()
		{
			var ignore = new IgnoreNull(null);

			Assert.AreEqual(string.Empty, ignore.ToString());

			ignore = new IgnoreNull(1);
			Assert.AreEqual("1", ignore.ToString());
		}

		[Test]
		public void AreEqual_Returns_Equality_Of_Targets()
		{
			var ignore1 = new IgnoreNull(1);
			var ignore2 = new IgnoreNull(2 - 1);

			Assert.IsTrue(IgnoreNull.AreEqual(ignore1, ignore2));
		}

		class Duck
		{
			private string _name;

			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			public string this[int index]
			{
				get { return Name; }
				set { Name = value; }
			}
		}
	}
}
