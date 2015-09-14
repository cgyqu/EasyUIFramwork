using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib
{
    /// <summary>
    /// 基础数据类型验证器
    /// </summary>
    public static class _Validator
    {
        #region NullOrEmpty
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 value 参数为 null 或 System.String.Empty，或者如果 value 仅由空白字符组成，则为 true。</returns>
        public static bool NullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串</param>
        /// <returns>如果 value 参数为 null 或 System.String.Empty，或者如果 value 仅由空白字符组成，则为 true。</returns>
        public static bool NullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        #endregion

        #region IsInt16
        public static bool IsInt16(this string value, out Int16 result)
        {
            return Int16.TryParse(value, out result);
        }

        public static bool IsInt16(this string value)
        {
            Int16 result;
            return Int16.TryParse(value, out result);
        }
        #endregion

        #region IsInt32
        public static bool IsInt32(this string value, out Int32 result)
        {
            return Int32.TryParse(value, out result);
        }

        public static bool IsInt32(this string value)
        {
            Int32 result;
            return Int32.TryParse(value, out result);
        }
        #endregion

        #region IsInt64
        public static bool IsInt64(this string value)
        {
            Int64 result;
            return Int64.TryParse(value, out result);
        }

        public static bool IsInt64(this string value, out Int64 result)
        {
            return Int64.TryParse(value, out result);
        }
        #endregion

        #region IsDouble
        public static bool IsDouble(this string value)
        {
            double result;
            return double.TryParse(value, out result);
        }
        public static bool IsDouble(this string value, out double result)
        {
            return double.TryParse(value, out result);
        }
        #endregion

        #region IsDecimal
        public static bool IsDecimal(this string value)
        {
            decimal result;
            return decimal.TryParse(value, out result);
        }
        public static bool IsDecimal(this string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }
        #endregion

        #region IsFloat
        public static bool IsFloat(this string value)
        {
            float result;
            return float.TryParse(value, out result);
        }
        public static bool IsFloat(this string value, out float result)
        {
            return float.TryParse(value, out result);
        }
        #endregion

        #region IsBool
        public static bool IsBool(this string value)
        {
            bool result;
            return bool.TryParse(value, out result);
        }
        public static bool IsBool(this string value, out bool result)
        {
            return bool.TryParse(value, out result);
        }
        #endregion

        #region IsShort
        public static bool IsShort(this string value)
        {
            short result;
            return short.TryParse(value, out result);
        }
        public static bool IsShort(this string value, out short result)
        {
            return short.TryParse(value, out result);
        }
        #endregion

        #region IsLong
        public static bool IsLong(this string value)
        {
            long result;
            return long.TryParse(value, out result);
        }
        public static bool IsLong(this string value, out long result)
        {
            return long.TryParse(value, out result);
        }
        #endregion

        #region IsDateTime
        public static bool IsDateTime(this string value)
        {
            DateTime result;
            return DateTime.TryParse(value, out result);
        }
        public static bool IsDateTime(this string value, out DateTime result)
        {
            return DateTime.TryParse(value, out result);
        }
        #endregion

        #region IsByte
        public static bool IsByte(this string value)
        {
            byte result;
            return byte.TryParse(value, out result);
        }
        public static bool IsByte(this string value, out byte result)
        {
            return byte.TryParse(value, out result);
        }
        #endregion


        public static bool IsMobileNum(this string mobileNum)
        {
			Regex regex = new Regex(@"^(13[0-9]|15[0-9]|17[0-9]|18[0-9]|147)\d{8}$", RegexOptions.IgnoreCase);
            return regex.Match(mobileNum).Success;
        }

        public static bool IsEmail(this string email)
        {
            Regex regex = new Regex(@"^\w+([-+.]\w+)*@(\w+([-.]\w+)*\.)+([a-zA-Z]+)+$", RegexOptions.IgnoreCase);
            return regex.Match(email).Success;
        }
    }

}
