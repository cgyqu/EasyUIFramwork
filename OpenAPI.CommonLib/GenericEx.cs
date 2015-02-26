using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib
{
	/// <summary>
	/// 泛型操作
	/// add by cgy6094
	/// </summary>
	public static class GenericEx
	{
		/// <summary>
		/// 判断列表是不是有数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool HasValue<T>(this IList<T> list)
		{
			return list != null && list.Count > 0;
		}
		/// <summary>
		/// 获取字典中的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T1"></typeparam>
		/// <param name="dic"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T1 GetValueFromDic<T, T1>(this IDictionary<T, T1> dic, T key)
		{
			if (dic != null && dic.Keys.Contains(key))
			{
				return dic[key];
			}
			return default(T1);
		}
	}

}
