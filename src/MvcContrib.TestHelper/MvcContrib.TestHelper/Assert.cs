using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace MvcContrib.TestHelper
{
    public static class Assert
    {
        public static void ShouldNotBeNull(this object Actual, string message)
        {
            if(Actual==null)
            {
                throw new AssertionException(message);
            }
        }
        public static void ShouldEqual(this object actual, object expected, string message)
        {
            if(actual == null && expected ==null)
            {
                return;
            }
            if(!actual.Equals(expected))
            {
                throw new AssertionException(message);
            }
        }
    }
}
