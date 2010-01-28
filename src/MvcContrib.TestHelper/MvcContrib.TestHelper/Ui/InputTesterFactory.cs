using System;
using System.Collections.Generic;

namespace MvcContrib.TestHelper.Ui
{
    public class InputTesterFactory : List<IInputTesterFactory>
    {
        public static Func<IList<IInputTesterFactory>> Default = () => new InputTesterFactory();

        public InputTesterFactory()
        {
            Add(new TextInputTesterFactory());
        }
    }
}