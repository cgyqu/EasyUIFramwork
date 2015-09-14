using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
	public static class UnixTimeConvertEx
	{
		/// <summary>
		///  Unix时间戳转为C#格式时间
		/// </summary>
		/// <param name="timeStamp">Unix时间戳格式</param>
		/// <returns></returns>
		public static DateTime GetTime(this string timeStamp)
		{
			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long lTime = long.Parse(timeStamp + "0000000");
			TimeSpan toNow = new TimeSpan(lTime);
			return dtStart.Add(toNow);
		}
	}
}
