using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Model;

namespace UnitTest.Core
{
    public class OldModel
    {
        public string Name { get; set; }
        public string Cat { get; set; }
        public String SubCat { get; set; }
        public string RequestData { get; set; }
        public string Desc { get; set; }
    }

    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
