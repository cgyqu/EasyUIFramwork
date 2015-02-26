using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class MembershipInterfaceException : Exception
    {
        public MembershipInterfaceException(string m, Exception ex, string post = "", string result = "")
            : base(string.Format("{0},postData:{1},result:{2}", m, post, result), ex)
        {

        }

        public bool IsLogic { get; set; }
    }
}
