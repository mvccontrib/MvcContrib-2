using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
    [TestFixture]
    public class IBehaviorTests
    {
        #region Test Behavior Definitions

        #region Base Classes

        private abstract class TestBeanCounter : IBehaviorMarker
        {
            public int Beans { get; private set; }

            protected TestBeanCounter()
            {
                Beans = 0;
            }

            protected void ExecuteInvoked()
            {
                Beans += 1;
            }
        }

        private abstract class TestBehaviorCounter<T> : TestBeanCounter, IBehavior<T>
        {
            #region IBehavior<T> Members

            public void Execute(T elment)
            {
                ExecuteInvoked();
            }

            #endregion
        }

        #endregion

        #region Regular Behavior Based Classes

        private class TestElementBehavior : TestBehaviorCounter<IElement>
        {
        }

        private class TestCheckBoxBehavior : TestBehaviorCounter<CheckBox>
        {
        }

        private class TestTextAreaBehavior : TestBehaviorCounter<TextArea>
        {
        }

        private class TestISupportsModelStateBehavior : TestBehaviorCounter<ISupportsModelState>
        {
        }

        private class TestViewPageBehavior : TestBehaviorCounter<System.Web.Mvc.ViewPage>
        {
        }

        private class TestMultipleBehaviors : TestBeanCounter, IBehavior<IElement>, IBehavior<IMemberElement>, IBehavior<CheckBox>, IBehavior<TextArea>
        {
            private HashSet<Type> TypeExecuted = new HashSet<Type>();

            private void ExecuteBase<T>(T element)
            {
                if (TypeExecuted.Contains(typeof(T)))
                {
                    Assert.Fail("Execute({0}) already called.", typeof(T).Name);
                }
                TypeExecuted.Add(typeof(T));
                ExecuteInvoked();
            }


            public void Execute(IElement element)
            {
                ExecuteBase(element);
            }

            public void Execute(IMemberElement element)
            {
                ExecuteBase(element);
            }

            public void Execute(CheckBox element)
            {
                ExecuteBase(element);
            }

            public void Execute(TextArea element)
            {
                ExecuteBase(element);
            }

        }

        #endregion

        #region Obsolete Behavior Based Classes

        [Obsolete]
        private class TestObsoleteIBehavior : TestBeanCounter, IBehavior
        {
            #region IBehavior<IElement> Members

            public void Execute(IElement elment)
            {
                ExecuteInvoked();
            }

            #endregion
        }

        [Obsolete]
        private class TestObsoleteIMemberBehavior : TestBeanCounter, IMemberBehavior
        {

            #region IBehavior<IMemberElement> Members

            public void Execute(IMemberElement elment)
            {
                ExecuteInvoked();
            }

            #endregion
        }


        #endregion

        #region IEnumerable<IBehaviorMarker>

        IEnumerable<IBehaviorMarker> CreateBehaviors()
        {
            return new IBehaviorMarker[]
            {
                new TestElementBehavior(),
                new TestCheckBoxBehavior(),
                new TestTextAreaBehavior(),
                new TestISupportsModelStateBehavior(),
                new TestViewPageBehavior(),
            };
        }

        [Obsolete]
        IEnumerable<IBehaviorMarker> CreateObsoleteBehaviors()
        {
            return new IBehaviorMarker[]
            {
                new TestObsoleteIBehavior(),
                new TestObsoleteIMemberBehavior(),
                new TestViewPageBehavior(),
            };
        }

        IEnumerable<IBehaviorMarker> CreateMultipleBehaviors()
        {
            return new IBehaviorMarker[]
            {
                new TestMultipleBehaviors(),
            };
        }

        #endregion

        #endregion

        #region Expected Results Checking

        class ExpectedResults : Dictionary<Type, int>
        {
            public ExpectedResults() : base() { }

            public ExpectedResults Add<TType>(int result)
                where TType : TestBeanCounter
            {
                Add(typeof(TType), result);
                return this;
            }

            public ExpectedResults PerformCheck(IEnumerable<IBehaviorMarker> behaviors)
            {
                // Note, this will filter out any non-test behaviors (which fortunately is not
                // a problem because we aren't using any within this testcase.
                foreach (TestBeanCounter marker in behaviors.OfType<TestBeanCounter>())
                {
                    Type type = marker.GetType();
                    int expected;
                    if (TryGetValue(type, out expected))
                    {
                        Assert.AreEqual(expected, marker.Beans,
                            string.Format("{0} not invoked correct number of times.",
                                type.Name));
                        Remove(type);
                    }
                    else
                    {
                        Assert.AreEqual(0, marker.Beans,
                            string.Format("{0} not invoked correct number of times.",
                                type.Name));
                    }
                }

                // Check remaining entries...
                foreach (var kvp in this)
                {
                    Assert.AreEqual(kvp.Value, 0,
                        string.Format("{0} not invoked correct number of times.",
                            kvp.Key.Name));
                }

                Clear();
                return this;
            }
        }

        #endregion

        #region Tests

        [Test]
        public void behaviors_are_selectively_invoked()
        {
            var behaviors = CreateBehaviors();

            new CheckBox("Test", null, behaviors).ToString();

            new ExpectedResults()
                .Add<TestElementBehavior>(1)
                .Add<TestCheckBoxBehavior>(1)
                .Add<TestISupportsModelStateBehavior>(1)
                .PerformCheck(behaviors);
        }

        [Test]
        public void behaviors_are_reusable()
        {
            var behaviors = CreateBehaviors();

            new CheckBox("Check", null, behaviors).ToString();
            new TextArea("TextArea", null, behaviors).ToString();

            new ExpectedResults()
                .Add<TestElementBehavior>(2)
                .Add<TestCheckBoxBehavior>(1)
                .Add<TestISupportsModelStateBehavior>(2)
                .Add<TestTextAreaBehavior>(1)
                .PerformCheck(behaviors);
        }

        [Test]
        [Obsolete]
        public void obsolete_behaviors_still_invoke_correctly()
        {
            var behaviors = CreateObsoleteBehaviors();

            new CheckBox("Test", null, behaviors).ToString();

            new ExpectedResults()
                .Add<TestObsoleteIBehavior>(1)
                .Add<TestObsoleteIMemberBehavior>(1)
                .PerformCheck(behaviors);
        }

        [Test]
        public void multiple_behaviors_within_class_get_called()
        {
            var behaviors = CreateMultipleBehaviors();

            new CheckBox("Test", null, behaviors).ToString();

            new ExpectedResults()
                .Add<TestMultipleBehaviors>(3)
                .PerformCheck(behaviors);
        }
        #endregion
    }
}
