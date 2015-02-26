using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    /// <summary>
    /// 数据转换器
    /// </summary>
    public static class ValueConverter
    {
        #region ToInt16
        public static Int16 ToInt16(this object value, Int16 _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            Int16 result;
            if (value.ToString().IsInt16(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToInt32
        public static Int32 ToInt32(this object value, Int32 _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            Int32 result;
            if (value.ToString().IsInt32(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToInt64
        public static Int64 ToInt64(this object value, Int64 _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            Int64 result;
            if (value.ToString().IsInt64(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToDouble
        public static double ToDouble(this object value, double _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            double result;
            if (value.ToString().IsDouble(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToDecimal
        public static decimal ToDecimal(this object value, decimal _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            decimal result;
            if (value.ToString().IsDecimal(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToFloat
        public static float ToFloat(this object value, float _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            float result;
            if (value.ToString().IsFloat(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToBool
        public static bool ToBool(this object value, bool _default = false)
        {
            if (value == null)
            {
                return _default;
            }
            bool result;
            if (value.ToString().IsBool(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToShort
        public static short ToShort(this object value, short _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            short result;
            if (value.ToString().IsShort(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToLong
        public static long ToLong(this object value, long _default = 0)
        {
            if (value == null)
            {
                return _default;
            }
            long result;
            if (value.ToString().IsLong(out result))
            {
                return result;
            }
            return _default;
        }
        #endregion

        #region ToDateTime
        public static DateTime ToDateTime(this object value, string _defaule = "1900-01-01")
        {
            if (value == null)
            {
                return DateTime.Parse(_defaule);
            }
            DateTime result;
            if (value.ToString().IsDateTime(out result))
            {
                return result;
            }
            return DateTime.Parse(_defaule);
        }
        #endregion

        #region ToByte
        public static byte ToByte(this object value, byte _defaule = 0)
        {
            if (value == null)
            {
                return _defaule;
            }
            byte result;
            if (value.ToString().IsByte(out result))
            {
                return result;
            }
            return _defaule;
        }
        #endregion

    }
}
