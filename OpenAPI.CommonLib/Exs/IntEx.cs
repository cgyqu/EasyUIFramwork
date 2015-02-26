using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAPI.CommonLib.Exs
{
    public static class IntEx
    {
        public static int ZeroCheck(this int value, int defaultValue = 1)
        {
            if (value <= 0)
            {
                return defaultValue;
            }
            return value;
        }

        public static bool IsBetween(this int value, int bengin, int end)
        {
            return value >= bengin && value <= end;
        }

        public static bool IsBetween(this long value, int bengin, int end)
        {
            return value >= bengin && value <= end;
        }


    }
}
