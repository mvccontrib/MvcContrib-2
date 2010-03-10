using System;

namespace MvcContrib.TestHelper
{
    /// <summary>
    /// Contains basic extension methods to simplify testing.
    /// </summary>
    public static class GeneralTestExtensions
    {
        /// <summary>
        /// Asserts that the object is of type T.  Also returns T to enable method chaining.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="message">Message to display when assertion fails</param>
        /// <returns></returns>
        public static T ShouldBe<T>(this object actual,string message) where T : class
        {
            //Assert.That(actual, Is.InstanceOfType(typeof(T)));
            if(actual as T ==null)
            {
              throw new AssertionException(message);  
            }
            return (T)actual;
        }

        /// <summary>
        /// Asserts that the object is the expected value.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void ShouldBe(this object actual, object expected)
        {
            
            if(!actual.Equals(expected))
            {
                string message = string.Format("was {0} but expected {1}", actual, expected);
                throw new AssertionException(message);
            }
        }


        /// <summary>
        /// Compares the two strings (case-insensitive).
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AssertSameStringAs(this string actual, string expected)
        {

            if(!string.Equals(actual, expected, StringComparison.InvariantCultureIgnoreCase))
            {
                var message = string.Format("Expected {0} but was {1}", expected, actual);
                throw  new AssertionException(message);
            }
                
        }

		/// <summary>
		/// Compares the two strings (case-insensitive).
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		public static void AssertStringContains(this string actual, string expected)
		{
			if (!actual.Contains(expected))
			{
				var message = string.Format("Expected {0} to contain {1} but did not.", expected, actual);
				throw new AssertionException(message);
			}

		}
        
        ///<summary>
        /// Asserts that the object should not be null.
        ///</summary>
        ///<param name="Actual"></param>
        ///<param name="message"></param>
        ///<exception cref="AssertionException"></exception>
        public static void ShouldNotBeNull(this object Actual, string message)
        {
            if (Actual == null)
            {
                throw new AssertionException(message);
            }
        }

        ///<summary>
        /// Asserts that two objects are equal.
        ///</summary>
        ///<param name="actual"></param>
        ///<param name="expected"></param>
        ///<param name="message"></param>
        ///<exception cref="AssertionException"></exception>
        public static void ShouldEqual(this object actual, object expected, string message)
        {
            if (actual == null && expected == null)
            {
                return;
            }
            if (!actual.Equals(expected))
            {
                throw new AssertionException(message);
            }
        }
    }
}
