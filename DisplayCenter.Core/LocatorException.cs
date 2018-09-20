using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core
{
    public class LocatorException : RuntimeException
    {
        public LocatorException(string msg) : base(msg) { }
    }

    public class DoubleLocatedLocatorException : LocatorException
    {
        public DoubleLocatedLocatorException(string msg) : base(msg) { }
    }
}
