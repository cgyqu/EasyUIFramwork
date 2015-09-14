using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class McCacheHelperException : Exception
    {
        public McCacheHelperException(Exception inner, string message = "") : base(message, inner) { }
    }
}
