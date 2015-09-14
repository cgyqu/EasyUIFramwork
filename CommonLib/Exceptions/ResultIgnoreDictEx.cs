using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class ResultIgnoreDictEx : Exception
    {
        private static string ExMsg(string fullName, string exp)
        {
            return string.Format("排除字段:{0}已设置 表达式:{1}", fullName, exp);
        }
        public ResultIgnoreDictEx(string fullName, string exp)
            : base(ExMsg(fullName, exp))
        {

        }
    }
}
