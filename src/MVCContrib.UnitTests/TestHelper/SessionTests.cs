using System;
using System.Collections;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Assert=NUnit.Framework.Assert;

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class SessionTests
	{
		[Test]
		public void CanSpecifySessionVariables()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable"] = "Value";
			var testHelperController = new TestHelperController();
			builder.InitializeController(testHelperController);
			Assert.AreEqual("Value", testHelperController.HttpContext.Session["Variable"]);
		}

		[Test]
		public void CanAddRemoveSessionVariables()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable"] = "Value";
			Assert.AreEqual("Value", builder.Session["Variable"]);
			builder.Session.Remove("Variable");
			Assert.AreEqual(null, builder.Session["Variable"]);
			builder.Session["a1"] = "z1";
			builder.Session["a2"] = "z2";
			builder.Session["a3"] = "z3";
			Assert.AreEqual(3, builder.Session.Count);
			builder.Session.RemoveAt(1);
			Assert.AreEqual(2, builder.Session.Count);
			Assert.AreEqual("z1", builder.Session["a1"]);
			Assert.AreEqual("z3", builder.Session["a3"]);
			Assert.AreEqual(null, builder.Session["a2"]);
			builder.Session[0] = "q1";
			Assert.AreEqual("q1", builder.Session["a1"]);
			Assert.AreEqual("q1", builder.Session[0]);
			builder.Session[builder.Session.Count - 1] = "x1";
			Assert.AreEqual("x1", builder.Session["a3"]);
			builder.Session.Clear();
			Assert.AreEqual(0, builder.Session.Count);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void LowSessionIndexesShouldThrowArgumentOutOfRangeException()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable"] = "Value";
			object o = builder.Session[-1];
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void HighSessionIndexesShouldThrowArgumentOutOfRangeException()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable"] = "Value";
			object o = builder.Session[1];
		}

		[Test]
		public void MissingSessionValueShouldReturnNull()
		{
			var builder = new TestControllerBuilder();
			object o = builder.Session["Variable"];
			Assert.IsNull(o);
		}

		[Test]
		public void CanCopySessionValuesToArray()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable1"] = "Value1";
			builder.Session["Variable2"] = "Value2";
			var entries = new DictionaryEntry[2];
			builder.Session.CopyTo(entries, 0);
			Assert.AreEqual("Value1", entries[0].Value);
			Assert.AreEqual("Value2", entries[1].Value);
			Assert.AreEqual("Variable1", entries[0].Key);
			Assert.AreEqual("Variable2", entries[1].Key);
		}

		[Test]
		public void RemoveAllShouldEmptySession()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable"] = "Value";
			Assert.AreEqual(1, builder.Session.Count);
			builder.Session.RemoveAll();
			Assert.AreEqual(0, builder.Session.Count);
		}

		[Test]
		public void AddShouldAddObject()
		{
			var builder = new TestControllerBuilder();
			builder.Session["Variable1"] = "Value1";
			builder.Session.Add("Variable2", "Value2");
			Assert.AreEqual("Value1", builder.Session["Variable1"]);
			Assert.AreEqual("Value2", builder.Session["Variable2"]);
		}


		[Test]
		public void SessionContentsShouldReturnSessionContents()
		{
			var builder = new TestControllerBuilder();

			Assert.AreEqual(builder.Session.Contents, builder.Session.Contents);
			Assert.AreEqual(
				builder.Session.Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents.
					Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents.Contents, builder.Session);
		}

		[Test]
		public void SessionSyncRootShouldNotBeNull()
		{
			var builder = new TestControllerBuilder();
			Assert.IsNotNull(builder.Session.SyncRoot);
		}

		[Test]
		public void SessionIsSynchronizedShouldNotFalse()
		{
			var builder = new TestControllerBuilder();
			Assert.IsFalse(builder.Session.IsSynchronized);
		}


		//Not Implemented Exceptions
		[Test, ExpectedException(typeof(NotImplementedException))]
		public void Abandon_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			builder.Session.Abandon();
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void SessionID_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			string s = builder.Session.SessionID;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void StaticObjects_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			object o = builder.Session.StaticObjects;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void TimeoutGet_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			int i = builder.Session.Timeout;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void TimeoutSet_IsNotImplemnted()
		{
			new TestControllerBuilder {Session = {Timeout = 0}};
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void CookieMode_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			object o = builder.Session.CookieMode;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void IsCookieless_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			bool b = builder.Session.IsCookieless;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void IsNewSession_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			bool b = builder.Session.IsNewSession;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void IsReadOnly_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			bool b = builder.Session.IsReadOnly;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void Keys_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			object o = builder.Session.Keys;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void LCIDGet_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			int i = builder.Session.LCID;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void LCIDSet_IsNotImplemnted()
		{
			 new TestControllerBuilder {Session = {LCID = 0}};
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void Mode_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			object o = builder.Session.Mode;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void CodePageGet_IsNotImplemnted()
		{
			var builder = new TestControllerBuilder();
			int i = builder.Session.CodePage;
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void CodePageSet_IsNotImplemnted()
		{
			new TestControllerBuilder {Session = {CodePage = 0}};
		}
	}
}
